using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using PMaP.Helpers;
using PMaP.Models.Authenticate;
using PMaP.Models.DBModels;
using PMaP.Models.Users;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;

namespace PMaP.Services
{
    public interface IUserService
    {
        AuthenticateResponse Authenticate(AuthenticateRequest model);
        string Add(AddUserRequest model);
        User GetById(int id);
        IEnumerable<User> GetAll();
    }

    public class UserService : IUserService
    {
        private readonly AppSettings _appSettings;
        private pmpContext _pmpContext;

        public UserService(IOptions<AppSettings> appSettings, pmpContext pmpContext)
        {
            _appSettings = appSettings.Value;
            _pmpContext = pmpContext;
        }

        public AuthenticateResponse Authenticate(AuthenticateRequest model)
        {
            var user = _pmpContext.Users.SingleOrDefault(x => x.Username == model.Username && x.Password == ManagedAes.Encrypt(model.Password));

            // return null if user not found
            if (user == null) return null;

            // authentication successful so generate jwt token
            var token = generateJwtToken(user);

            return new AuthenticateResponse(user, token);
        }

        public string Add(AddUserRequest model)
        {
            try
            {
                _pmpContext.Users.Add(new User
                {
                    Password = ManagedAes.Encrypt(model.Password),
                    Username = model.Username
                });
                _pmpContext.SaveChangesAsync();

                return "User successfully added.";
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public User GetById(int id)
        {
            return _pmpContext.Users.FirstOrDefault(x => x.Id == id);
        }

        public IEnumerable<User> GetAll()
        {
            return _pmpContext.Users;
        }

        private string generateJwtToken(User user)
        {
            // generate token that is valid for 7 days
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[] { new Claim("id", user.Id.ToString()) }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
