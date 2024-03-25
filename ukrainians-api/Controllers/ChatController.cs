using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Ukrainians.Infrastrusture.Data.Entities;
using Ukrainians.UtilityServices.Models.Common;
using Ukrainians.UtilityServices.Services.Chat;

namespace NomadChat.WebAPI.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ChatController : ControllerBase
    {
        private readonly IChatService _chatService;
        private readonly UserManager<User> _userManager;

        public ChatController(IChatService chatService, UserManager<User> userManager)
        {
            _chatService = chatService;
            _userManager = userManager;
        }

        [HttpPost("register-user")]
        public IActionResult RegisterUser(UserModel userModel)
        {
            if (_chatService.AddUserToList(userModel))
            {
                return NoContent();
            }

            return BadRequest("This name is already taken");
        }

        [Authorize]
        [HttpPost("upload-profile-picture")]
        public async Task<IActionResult> UploadProfilePicture([FromForm] IFormFile image, [FromForm] string username)
        {

            if (image == null || image.Length == 0)
            {
                return BadRequest("Invalid file");
            }

            var user = await _userManager.FindByNameAsync(username);

            using (var ms = new MemoryStream())
            {
                await image.CopyToAsync(ms);
                var fileBytes = ms.ToArray();
                user.ProfilePicture = fileBytes;

                await _userManager.UpdateAsync(user);
            }

            return NoContent();
        }

        [Authorize]
        [HttpPost("delete-profile-picture")]
        public async Task<IActionResult> DeleteProfilePicture([FromForm] string username)
        {

            var user = await _userManager.FindByNameAsync(username);
            if (user == null)
            {
                return BadRequest($"No user with {username} username was found");
            }

            user.ProfilePicture = null;
            await _userManager.UpdateAsync(user);
            return NoContent();
        }
    }
}
