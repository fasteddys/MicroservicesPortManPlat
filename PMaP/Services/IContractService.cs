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
}
