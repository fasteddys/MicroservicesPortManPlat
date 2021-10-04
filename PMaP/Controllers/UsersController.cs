using Microsoft.AspNetCore.Mvc;
using PMaP.Models.Authenticate;
using PMaP.Models.Users;
using PMaP.Services;
using System;

namespace PMaP.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("authenticate")]
        public IActionResult Authenticate(AuthenticateRequest model)
        {
            var response = _userService.Authenticate(model);

            if (response == null)
                return BadRequest(new { Message = "Username or password is incorrect" });

            return Ok(response);
        }

        [HttpPost("add")]
        public IActionResult Add(AddUserRequest model)
        {
            try
            {
                return Ok(new { message = _userService.Add(model) });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [Authorize]
        [HttpGet]
        public IActionResult GetAll()
        {
            var users = _userService.GetAll();
            return Ok(users);
        }
    }
}
