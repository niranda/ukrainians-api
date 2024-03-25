using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ukrainians.Infrastrusture.Data.Entities;

namespace Ukrainians.Infrastructure.Business.Services.Token
{
    public interface ITokenService
    {
        string GenerateJWT(User user);
    }
}
