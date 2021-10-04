using PMaP.Models.Homes;
using System.Threading.Tasks;

namespace PMaP.Services
{
    public interface IHomeService
    {
        Task<HomeResponse> GetAll();
    }
}
