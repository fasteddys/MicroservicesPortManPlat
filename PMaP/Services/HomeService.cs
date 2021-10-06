using Microsoft.Extensions.Options;
using PMaP.Helpers;
using PMaP.Models.Homes;
using System.Threading.Tasks;

namespace PMaP.Services
{
    public interface IHomeService
    {
        Task<HomeResponse> GetAll();
    }

    public class HomeService : IHomeService
    {
        private readonly AppSettings _appSettings;
        private IHttpService _httpService;

        public HomeService(IOptions<AppSettings> appSettings, IHttpService httpService)
        {
            _appSettings = appSettings.Value;
            _httpService = httpService;
        }

        public async Task<HomeResponse> GetAll()
        {
            return await _httpService.Get<HomeResponse>(_appSettings.HomeUrl + "/home") ?? new HomeResponse();
        }
    }
}
