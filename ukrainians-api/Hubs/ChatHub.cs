using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;
using NomadChat.WebAPI.Constants;
using Ukrainians.Domain.Core.Models;
using Ukrainians.Infrastrusture.Data.Entities;
using Ukrainians.UtilityServices.Models.Common;
using Ukrainians.UtilityServices.Models.File;
using Ukrainians.UtilityServices.Services.Chat;
using Ukrainians.UtilityServices.Services.ChatMessage;
using Ukrainians.UtilityServices.Services.ChatNotification;
using Ukrainians.UtilityServices.Services.ChatRoom;
using Ukrainians.UtilityServices.Services.PushNotificationsSubscription;
using WebPush;

namespace NomadChat.WebAPI.Hubs
{
    public class ChatHub : Hub
    {
        private readonly IChatService _chatService;
        private readonly IChatRoomService<ChatRoomDomain> _chatRoomService;
        private readonly IChatMessageService<ChatMessageDomain> _chatMessageService;
        private readonly IChatNotificationService<ChatNotificationDomain> _chatNotificationService;
        private readonly IPushNotificationsSubscriptionService<PushNotificationsSubscriptionDomain> _subscriptionService;
        private readonly VapidDetails _vapidDetails;
        private readonly UserManager<User> _userManager;

        private static ChatRoomDomain? MainChatRoom;

        public ChatHub(IChatService chatService,
            IChatRoomService<ChatRoomDomain> chatRoomService,
            IChatMessageService<ChatMessageDomain> chatMessageService,
            UserManager<User> userManager,
            IChatNotificationService<ChatNotificationDomain> chatNotificationService,
            VapidDetails vapidDetails,
            IPushNotificationsSubscriptionService<PushNotificationsSubscriptionDomain> subscriptionService)
        {
            _chatService = chatService;
            _chatRoomService = chatRoomService;
            _chatMessageService = chatMessageService;
            _userManager = userManager;
            _chatNotificationService = chatNotificationService;
            _vapidDetails = vapidDetails;
            _subscriptionService = subscriptionService;
        }

        public override async Task OnConnectedAsync()
        {
            await InitializeAndLogInMainChatRoom();

            await Clients.Caller.SendAsync("UserConnected");

            await LoadMainChatRoom();
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            await LogOutOfMainChatRoom();

            await DisplayOnlineUsers();

            await base.OnDisconnectedAsync(exception);
        }

        public async Task AddUserConnectionId(string name)
        {
            _chatService.AddUserConnectionId(name, Context.ConnectionId);

            var currentUser = await _userManager.FindByNameAsync(name);
            if (currentUser != null)
            {
                MainChatRoom!.Users.Add(currentUser);
            }
            await DisplayPrivateChats(name);
            await DisplayOnlineUsers();
            await DisplayNotifications(name);
        }

        public async Task ReceiveMessage(ChatMessageDomain message)
        {
            var newMessage = await _chatMessageService.AddChatMessage(message);
            await Clients.Groups(ChatHubConstants.MainChatRoomName).SendAsync("NewMessage", newMessage);
        }

        public async Task OpenPrivateChat(string from, string to)
        {
            var privateGroupName = GetPrivateGroupName(from, to);

            var privateRoom = await GetOrCreatePrivateChatRoom(privateGroupName, from, to);

            await DisplayPrivateChats(from);

            await GetOrCreateNotification(from, privateRoom.Id, 0);

            await ConnectToPrivateChat(to, privateGroupName);

            await MarkMessagesInChatRoomAsRead(privateRoom.Id, from);

            var messages = await _chatMessageService.GetAllChatMessagesByRoomId(privateRoom.Id);

            var notificationsByName = await _chatNotificationService.GetChatNotificationsByUsername(from);

            await Clients.Client(Context.ConnectionId).SendAsync("OpenPrivateChat", messages, notificationsByName, from, to);
        }

        public async Task ReceivePrivateMessage(ChatMessageDomain message)
        {
            var privateGroupName = GetPrivateGroupName(message.From, message.To);

            var privateRoom = await GetOrCreatePrivateChatRoom(privateGroupName, message.From, message.To);

            await DisplayPrivateChats(message.From);

            await GetOrCreateNotification(message.To, privateRoom.Id);

            await MarkMessagesInChatRoomAsRead(privateRoom.Id, message.From);

            message.ChatRoomId = privateRoom.Id;

            var newMessage = await _chatMessageService.AddChatMessage(message);

            var toConnectionId = _chatService.GetConnectionIdByUser(message.To);

            if (toConnectionId != null)
            {
                await DisplayPrivateChats(message.To, toConnectionId);
                var notificationsByName = await _chatNotificationService.GetChatNotificationsByUsername(message.To);
                await Clients.Clients(toConnectionId).SendAsync("NewPrivateMessage", newMessage, notificationsByName);
            }

            await Clients.Clients(Context.ConnectionId).SendAsync("NewPrivateMessage", newMessage);

            await SendNotification(message.From, message.To, message.Content);
        }

        public async Task RemovePrivateChat(string from, string to)
        {
            var privateGroupName = GetPrivateGroupName(from, to);
            await Clients.Group(privateGroupName).SendAsync("ClosePrivateChat", from, to);

            await Groups.RemoveFromGroupAsync(Context.ConnectionId, privateGroupName);
            var toConnectionId = _chatService.GetConnectionIdByUser(to);
            await Groups.RemoveFromGroupAsync(toConnectionId, privateGroupName);
        }

        public async Task DeleteMessage(ChatMessageDomain message)
        {
            string groupName = ChatHubConstants.MainChatRoomName;

            if (message.To != null)
            {
                groupName = GetPrivateGroupName(message.From, message.To);
            }

            await _chatMessageService.DeleteChatMessage(message.Id);

            await Clients.Group(groupName).SendAsync("DeleteMessage", message);
        }

        public async Task EditMessage(ChatMessageDomain message)
        {
            string groupName = ChatHubConstants.MainChatRoomName;

            if (message.To != null)
            {
                groupName = GetPrivateGroupName(message.From, message.To);
            }

            await _chatMessageService.UpdateChatMessage(message);

            await Clients.Group(groupName).SendAsync("UpdateMessage", message);
        }

        public async Task SubscribeForNotifications(PushSubscription sub, string username)
        {
            var pushSubscription = await _subscriptionService.GetPushNotificationsSubscriptionByUsername(username);
            if (pushSubscription == null)
            {
                await _subscriptionService.AddPushNotificationsSubscription(pushSubscription);
            }
        }

        public async Task UnsubscribeFromNotifications(PushSubscription sub, string username)
        {
            var pushSubscription = await _subscriptionService.GetPushNotificationsSubscriptionByUsername(username);
            if (pushSubscription == null) return;

            await _subscriptionService.DeletePushNotificationsSubscription(pushSubscription.Id);
        }

        public async Task SaveFile(FileUpload fileObj)
        {
            var username = fileObj.Username;
            var user = await _userManager.FindByNameAsync(username);

            if (fileObj.File.Length > 0 && user != null)
            {
                using (var ms = new MemoryStream())
                {
                    fileObj.File.CopyTo(ms);
                    var fileBytes = ms.ToArray();
                    user.ProfilePicture = fileBytes;

                    await _userManager.UpdateAsync(user);
                }
            }
        }

        private async Task SendNotification(string sender, string receiver, string content)
        {
            var subscription = await _subscriptionService.GetPushNotificationsSubscriptionByUsername(receiver);

            if (subscription != null)
            {
                Broadcast(new PushSubscription
                {
                    Auth = subscription.Auth,
                    Endpoint = subscription.Endpoint,
                    P256DH = subscription.P256DH
                },
                new NotificationDomain
                {
                    Message = content,
                    Title = $"New message from {sender}",
                    Url = ""
                },
                _vapidDetails);
            }
        }

        private void Broadcast(PushSubscription pushSubscription, NotificationDomain message, VapidDetails vapidDetails)
        {
            var client = new WebPushClient();
            var serializedMessage = JsonConvert.SerializeObject(message);
            client.SendNotification(pushSubscription, serializedMessage, vapidDetails);
        }

        private async Task InitializeAndLogInMainChatRoom()
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, ChatHubConstants.MainChatRoomName);

            MainChatRoom = await _chatRoomService.GetChatRoomByName(ChatHubConstants.MainChatRoomName);
            if (MainChatRoom == null)
            {
                MainChatRoom = await _chatRoomService.AddChatRoom(new ChatRoomDomain { RoomName = ChatHubConstants.MainChatRoomName });
            }
        }

        private async Task LoadMainChatRoom()
        {
            var messages = await _chatMessageService.GetAllChatMessagesByRoomId(MainChatRoom.Id);
            var roomId = MainChatRoom.Id.ToString();

            await Clients.Groups(ChatHubConstants.MainChatRoomName).SendAsync("InitializeMainRoom", roomId, messages);
        }

        private async Task LogOutOfMainChatRoom()
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, ChatHubConstants.MainChatRoomName);

            var user = _chatService.GetUserByConnectionId(Context.ConnectionId);
            _chatService.RemoveUserFromList(user);

            await _chatRoomService.UpdateChatRoom(MainChatRoom!);
        }

        private async Task<ChatRoomDomain> GetOrCreatePrivateChatRoom(string groupName, string from, string to)
        {
            ChatRoomDomain? privateRoom = await _chatRoomService.GetChatRoomByName(groupName);

            if (privateRoom == null)
            {
                var sender = await _userManager.FindByNameAsync(from);
                var receiver = await _userManager.FindByNameAsync(to);

                if (sender != null && receiver != null)
                {
                    privateRoom = new ChatRoomDomain { Users = new List<User> { sender, receiver }, RoomName = groupName };
                    await _chatRoomService.AddChatRoom(privateRoom);
                }
            }

            return privateRoom;
        }

        private async Task<ChatNotificationDomain> GetOrCreateNotification(string username, Guid privateRoomId, int? initialAmount = null)
        {
            var notification = await _chatNotificationService.GetChatNotificationByUsernameAndRoomId(username, privateRoomId);

            if (notification == null)
            {
                notification = await _chatNotificationService.AddChatNotification(new ChatNotificationDomain
                {
                    ChatRoomId = privateRoomId,
                    UnreadMessages = 1,
                    Username = username
                });

                return notification;
            }

            if (initialAmount.HasValue)
            {
                notification.UnreadMessages = initialAmount.Value;
            }
            else
            {
                notification.UnreadMessages++;
            }

            return await _chatNotificationService.UpdateChatNotification(notification);
        }

        private async Task ConnectToPrivateChat(string usernameToEstablishConnectionWith, string privateGroupName)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, privateGroupName);

            var toConnectionId = _chatService.GetConnectionIdByUser(usernameToEstablishConnectionWith);
            if (toConnectionId != null)
            {
                await Groups.AddToGroupAsync(toConnectionId, privateGroupName);
                await Clients.Client(toConnectionId).SendAsync("MessagesRead");
            }
        }

        private async Task MarkMessagesInChatRoomAsRead(Guid privateRoomId, string usernameWhoRead)
        {
            var messages = await _chatMessageService.GetAllChatMessagesByRoomId(privateRoomId);

            var messagesToUpdate = messages.Where(m => m.Unread && m.To == usernameWhoRead).Select(m =>
            {
                m.Unread = false;
                return m;
            });

            await _chatMessageService.UpdateChatMessages(messagesToUpdate);
        }

        private async Task DisplayNotifications(string name)
        {
            var notificationsByName = await _chatNotificationService.GetChatNotificationsByUsername(name);

            var connectionId = _chatService.GetConnectionIdByUser(name);

            await Clients.Client(connectionId).SendAsync("Notify", notificationsByName);
        }

        private async Task DisplayPrivateChats(string name, string? connectionId = null)
        {
            var privateChats = await _chatRoomService.GetChatRoomsUserInteractedWith(name);

            var lastMessages = privateChats.Select(x =>
            {
                var message = string.Empty;
                var encryptedMessage = x.ChatMessages?.FirstOrDefault();
                if (encryptedMessage != null && !string.IsNullOrEmpty(encryptedMessage.Content))
                {
                    message = _chatMessageService.DecryptMessage(encryptedMessage.Content);
                }

                var username = x.RoomName!.Split('-').FirstOrDefault(s => s != name);
                var user = _userManager.FindByNameAsync(username).GetAwaiter().GetResult();
                if (user == null) return null;

                return new ChatLightModel
                {
                    ChatMessage = message,
                    PrivateChatId = x.Id,
                    User = new UserModel
                    {
                        ProfilePicture = user.ProfilePicture,
                        Name = user.UserName!,
                        Email = user.Email,
                    },
                    Unread = 0
                };
            }).ToList();

            if (connectionId != null)
            {
                await Clients.Client(connectionId).SendAsync("PrivateChats", lastMessages);
            }
            else
            {
                await Clients.Client(Context.ConnectionId).SendAsync("PrivateChats", lastMessages);
            }
        }

        private async Task DisplayOnlineUsers()
        {
            var onlineUsers = _chatService.GetOnlineUsers();
            var users = onlineUsers
                .Select(s =>
                {
                    var user = _userManager.FindByNameAsync(s).GetAwaiter().GetResult();
                    if (user == null) return null;

                    return new UserModel
                    {
                        Email = user.Email,
                        ProfilePicture = user.ProfilePicture,
                        Name = user.UserName!
                    };
                })
                .Where(s => s != null)
                .ToList();

            await Clients.Groups(ChatHubConstants.MainChatRoomName).SendAsync("OnlineUsers", users);
        }

        private string GetPrivateGroupName(string from, string to)
        {
            var stringComparer = string.CompareOrdinal(from, to) < 0;
            return stringComparer ? $"{from}-{to}" : $"{to}-{from}";
        }
    }
}
