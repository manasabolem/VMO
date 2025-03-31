using Microsoft.AspNetCore.Mvc;
using VMO.API.Services.Interfaces;
using VMO.Models;

namespace VMO.API.Controllers
{
    
    [ApiController]
    [Route("api/[controller]")]
    public class SalesController : ControllerBase
    {
        private readonly ISalesService _salesService;
        public SalesController(ISalesService salesService)
        {
            _salesService = salesService;
        }

        [HttpGet]
        public ActionResult<IEnumerable<SalesData>> GetSalesData()
        {
            var salesData = _salesService.GetSalesData();
            return Ok(salesData);
        }
    }
}
