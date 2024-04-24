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
        [HttpGet("UsernamesUserInteractedWith")]
        // GET: ChatRoom/UsernamesUserInteractedWith
        public async Task<IActionResult> GetUsernamesUserInteractedWith(string username)
        {
            return Ok(await _chatRoomService.GetUsernamesUserInteractedWith(username));
        }
    }
}
