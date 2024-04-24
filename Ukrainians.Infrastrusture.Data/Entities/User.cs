using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace Ukrainians.Infrastrusture.Data.Entities
{
    public class User : IdentityUser<Guid>
    {
        public User()
        {
        }

        public User(string userName, string nameToDisplay, string email, Role role, bool isEmailConfirmed)
        {
            UserName = userName;
            NameToDisplay = nameToDisplay;
            Role = role;
            Email = email;
            EmailConfirmed = isEmailConfirmed;
        }

        public string NameToDisplay { get; set; }

        public byte[]? ProfilePicture { get; set; }

        [MaxLength(100)]
        public string? Status { get; set; }

        public Guid? RoleId { get; set; }
        public Role Role { get; set; }
        public ICollection<ChatRoom> ChatRooms { get; set; }
    }
}
