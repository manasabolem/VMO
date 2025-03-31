using Newtonsoft.Json;
using VMO.Models;
using VMO.UI.Models;
using VMO.UI.Services.Interfaces;

namespace VMO.UI.Services
{
    public class SalesApiService : ISalesApiService
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<SalesApiService> _logger;
        public SalesApiService(HttpClient httpClient, ILogger<SalesApiService> logger)
        {
            _logger = logger;
            _httpClient = httpClient;
        }

        public async Task<List<SalesDataViewModel>> GetSalesDataAsync()
        {
            try
            {
                var baseAddress = _httpClient.BaseAddress + "Sales";
                var response = await _httpClient.GetStringAsync(baseAddress);

                if(string.IsNullOrEmpty(response))
                {
                    throw new Exception("Cannot get Sales Data");
                }

                var salesDataViewModel = JsonConvert.DeserializeObject<List<SalesDataViewModel>>(response);
                if (salesDataViewModel is null || salesDataViewModel.Count <= 0)
                {
                    throw new Exception("Error converting Sales data to View Model");
                }

                return salesDataViewModel;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }

        }
    }

}
