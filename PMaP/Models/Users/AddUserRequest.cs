using System.ComponentModel.DataAnnotations;

namespace PMaP.Models.Users
{
    public class AddUserRequest
    {
        [Required]
        public string Username { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
