using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using VMO.UI.Models;
using VMO.UI.Services;
using VMO.UI.Services.Interfaces;

namespace VMO.UI.Controllers
{
    public class SalesController : Controller
    {
        private readonly ILogger<SalesController> _logger;
        private readonly ISalesApiService _salesApiService;


        public SalesController(ILogger<SalesController> logger, ISalesApiService salesApiService)
        {
            _salesApiService = salesApiService;
            _logger = logger;
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                var salesDataViewModel = await _salesApiService.GetSalesDataAsync();
                return View(salesDataViewModel);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
