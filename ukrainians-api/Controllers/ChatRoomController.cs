using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Ukrainians.Domain.Core.Models;
using Ukrainians.UtilityServices.Services.ChatRoom;

namespace NomadChat.WebAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ChatRoomController : ControllerBase
    {
        private readonly IChatRoomService<ChatRoomDomain> _chatRoomService;

        public ChatRoomController(IChatRoomService<ChatRoomDomain> chatRoomService)
        {
            _chatRoomService = chatRoomService;
        }

        [Authorize]
        [HttpGet]
        // GET: ChatRoom
        public async Task<IActionResult> GetChatRooms()
        {
            return Ok(await _chatRoomService.GetAllChatRooms());
        }

        [Authorize]
        [HttpGet("UsernamesUserInteractedWith")]
        // GET: ChatRoom/UsernamesUserInteractedWith
        public async Task<IActionResult> GetUsernamesUserInteractedWith(string username)
        {
            return Ok(await _chatRoomService.GetUsernamesUserInteractedWith(username));
        }

        [Authorize]
        [HttpGet("{id}")]
        // GET: ChatRoom/{id}
        public async Task<IActionResult> GetChatRoomById(Guid id)
        {
            return Ok(await _chatRoomService.GetChatRoomById(id));
        }

        [Authorize]
        [HttpPost]
        // POST: ChatRoom
        public async Task<IActionResult> CreateChatRoom(ChatRoomDomain roomDomain)
        {
            return Ok(await _chatRoomService.AddChatRoom(roomDomain));
        }

        [Authorize]
        [HttpPut]
        // PUT: ChatRoom
        public async Task<IActionResult> UpdateChatRoom(ChatRoomDomain roomDomain)
        {
            return Ok(await _chatRoomService.UpdateChatRoom(roomDomain));
        }

        [Authorize]
        [HttpDelete("{id}")]
        // DELETE: ChatRoom/{id}
        public async Task<IActionResult> DeleteChatRoom(Guid id)
        {
            return Ok(await _chatRoomService.DeleteChatRoom(id));
        }
    }
}
