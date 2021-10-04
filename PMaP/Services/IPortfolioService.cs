using PMaP.Models.Portfolios;
using System;
using System.Threading.Tasks;

namespace PMaP.Services
{
    public interface IPortfolioService
    {
        Task<PortfolioResponse> GetAll(string queryStrings);
    }
}
