using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ukrainians.Infrastrusture.Data.Entities
{
    public class Role : IdentityRole<Guid>
    {
        public Role(string name)
        {
            Name = name;
        }
    }
}
