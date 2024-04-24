using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ukrainians.UtilityServices.Models.Common
{
    public class UserModel
    {
        public byte[]? ProfilePicture { get; set; }
        public string UserName { get; set; }
        public string? NameToDisplay { get; set; }
        public string? Email { get; set; }
    }
}
