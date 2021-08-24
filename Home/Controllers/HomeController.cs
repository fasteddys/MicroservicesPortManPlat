using Microsoft.AspNetCore.Mvc;
using System.Linq;
using Home.Models;
using Microsoft.Extensions.Configuration;
using Home.Models.DBModels;

namespace Home.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class HomeController : ControllerBase
    {
        private pmpContext _pmpContext;

        public HomeController(pmpContext pmpContext)
        {
            _pmpContext = pmpContext;
        }

        [HttpGet]
        public Response Get()
        {
            Response response = new Response();

            try
            {
                var homes = _pmpContext.Homes.ToList();
                response.ResponseCode = 200;
                response.Message = "Success";
                response.Documents = homes;
            }
            catch (System.Exception ex)
            {
                response.ResponseCode = 500;
                response.Message = ex.Message;
            }

            return response;
        }
    }
}
