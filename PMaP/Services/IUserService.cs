using PMaP.Models.Authenticate;
using PMaP.Models.DBModels;
using PMaP.Models.Users;
using System.Collections.Generic;

namespace PMaP.Services
{
    public interface IUserService
    {
        AuthenticateResponse Authenticate(AuthenticateRequest model);
        string Add(AddUserRequest model);
        User GetById(int id);
        IEnumerable<User> GetAll();
    }
}
