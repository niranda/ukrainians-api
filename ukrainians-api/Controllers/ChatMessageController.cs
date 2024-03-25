using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Ukrainians.Domain.Core.Models;
using Ukrainians.UtilityServices.Services.ChatMessage;

namespace NomadChat.WebAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ChatMessageController : ControllerBase
    {
        private readonly IChatMessageService<ChatMessageDomain> _messageService;

        public ChatMessageController(IChatMessageService<ChatMessageDomain> messageService)
        {
            _messageService = messageService;
        }

        [Authorize]
        [HttpGet]
        // GET: ChatMessage
        public async Task<IActionResult> GetChatMessages()
        {
            return Ok(await _messageService.GetAllChatMessages());
        }

        [Authorize]
        [HttpGet("{id}")]
        // GET: ChatMessage/{id}
        public async Task<IActionResult> GetChatMessageById(Guid id)
        {
            return Ok(await _messageService.GetChatMessageById(id));
        }

        [Authorize]
        [HttpPost]
        // POST: ChatMessage
        public async Task<IActionResult> CreateChatMessage(ChatMessageDomain messageDomain)
        {
            return Ok(await _messageService.AddChatMessage(messageDomain));
        }

        [Authorize]
        [HttpPut]
        // PUT: ChatMessage
        public async Task<IActionResult> UpdateChatMessage(ChatMessageDomain messageDomain)
        {
            return Ok(await _messageService.UpdateChatMessage(messageDomain));
        }

        [Authorize]
        [HttpDelete("{id}")]
        // DELETE: ChatMessage/{id}
        public async Task<IActionResult> DeleteChatMessage(Guid id)
        {
            return Ok(await _messageService.DeleteChatMessage(id));
        }
    }
}
