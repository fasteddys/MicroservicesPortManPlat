using PMaP.Models.DBModels;

namespace PMaP.Models.Authenticate
{
    public class AuthenticateResponse
    {
        //public int RespCode { get; set; }
        //public string Message { get; set; }
        public int Id { get; set; }
        public string Username { get; set; }
        public string Token { get; set; }

        //public AuthenticateResponse() { }

        public AuthenticateResponse(User user, string token)
        {
            Id = user.Id;
            Username = user.Username;
            Token = token;
        }
    }
}
