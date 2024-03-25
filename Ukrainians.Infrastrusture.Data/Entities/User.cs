using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace Ukrainians.Infrastrusture.Data.Entities
{
    public class User : IdentityUser<Guid>
    {
        public User()
        {
        }

        public User(string userName, string email, Role role)
        {
            UserName = userName;
            Role = role;
            Email = email;
        }

        public byte[]? ProfilePicture { get; set; }

        [MaxLength(100)]
        public string? Status { get; set; }

        public Guid? RoleId { get; set; }
        public Role Role { get; set; }
    }
}
