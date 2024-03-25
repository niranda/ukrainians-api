using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ukrainians.Infrastrusture.Data.Entities;

namespace Ukrainians.Infrastrusture.Data.Stores
{
    public interface IUserRepository
    {
        Task<User?> FindByUsername(string username);
        Task<User?> FindByEmail(string email);
        Task<IEnumerable<User>> GetAll();
        Task<Guid> Create(User user);
        Task<Guid> GetRoleId(string roleId);
    }
}
