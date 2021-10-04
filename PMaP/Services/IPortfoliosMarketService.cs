using PMaP.Models.PortfoliosMarkets;
using System.Threading.Tasks;

namespace PMaP.Services
{
    public interface IPortfoliosMarketService
    {
        Task<PortfoliosMarketResponse> GetAll(string queryStrings);
    }
}
