using Microsoft.EntityFrameworkCore;
using Ukrainians.Infrastrusture.Data.Context;
using Ukrainians.Infrastrusture.Data.Entities;
using Ukrainians.Infrastrusture.Data.Stores;

namespace Ukrainians.Infrastrusture.Data.Repositories
{
    public class ChatNotificationRepository : IChatNotificationRepository
    {
        private readonly ApplicationContext _context;

        public ChatNotificationRepository(ApplicationContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<ChatNotification>> GetAllByUsername(string username)
        {
            return await _context.ChatNotifications
                .AsNoTracking()
                .Where(x => x.Username == username && !x.IsDeleted)
                .ToListAsync();
        }

        public async Task<ChatNotification?> GetByUsernameAndRoomId(string username, Guid chatRoomId)
        {
            return await _context.ChatNotifications
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.ChatRoomId == chatRoomId && x.Username == username && !x.IsDeleted);
        }

        public async Task<ChatNotification> Create(ChatNotification chatNotification)
        {
            await _context.ChatNotifications.AddAsync(chatNotification);
            await _context.SaveChangesAsync();
            return chatNotification;
        }

        public async Task<ChatNotification> Update(ChatNotification chatNotification)
        {
            _context.ChatNotifications.Update(chatNotification);
            await _context.SaveChangesAsync();
            return chatNotification;
        }
    }
}
