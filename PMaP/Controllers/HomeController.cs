using Microsoft.AspNetCore.Mvc;
using PMaP.Services;
using System.Threading.Tasks;

namespace PMaP.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HomeController : ControllerBase
    {
        private IHomeService _homeService;

        public HomeController(IHomeService homeService)
        {
            _homeService = homeService;
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var homes = await _homeService.GetAll();
            return Ok(homes);
        }
    }
}
