using Data;
using System.Collections.Generic;

namespace Core.Interfaces
{
    public interface IUserRepository
    {
        List<UserDetail> GetUsers();
        bool Delete(string id);
        UserDetail GetUser(string userId);
    }
}
