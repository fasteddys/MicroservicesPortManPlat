using Microsoft.Extensions.Options;
using PMaP.Helpers;
using PMaP.Models.Contracts;
using System.Threading.Tasks;

namespace PMaP.Services
{
    public interface IContractService
    {
        Task<ContractResponse> GetAll();
        Task<ContractResponse> ContractByPortfolio(int id);
        Task<ContractResponse> ContractsAssessment(int id);
        Task<ContractResponse> ContractByIdPortfolioId(int id, int portfolioId);
    }

    public class ContractService : IContractService
    {
        private readonly AppSettings _appSettings;
        private IHttpService _httpService;

        public ContractService(IOptions<AppSettings> appSettings, IHttpService httpService)
        {
            _appSettings = appSettings.Value;
            _httpService = httpService;
        }

        public async Task<ContractResponse> GetAll()
        {
            return await _httpService.Get<ContractResponse>(_appSettings.ContractUrl + "/contracts") ?? new ContractResponse();
        }

        public async Task<ContractResponse> ContractByPortfolio(int id)
        {
            return await _httpService.Get<ContractResponse>(_appSettings.ContractUrl + "/contracts/portfolio/" + id) ?? new ContractResponse();
        }

        public async Task<ContractResponse> ContractsAssessment(int id)
        {
            return await _httpService.Get<ContractResponse>(_appSettings.ContractUrl + "/contracts/portfolio/" + id + "/assessment") ?? new ContractResponse();
        }

        public async Task<ContractResponse> ContractByIdPortfolioId(int id, int portfolioId)
        {
            return await _httpService.Get<ContractResponse>(_appSettings.ContractUrl + "/contracts/" + id + "/portfolio/" + portfolioId) ?? new ContractResponse();
        }
    }
}
