using VMO.Models;
using VMO.UI.Models;

namespace VMO.UI.Services.Interfaces
{
    public interface ISalesApiService
    {
        Task<List<SalesDataViewModel>> GetSalesDataAsync();
    }
}
