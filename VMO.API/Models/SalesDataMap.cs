using CsvHelper.Configuration;
using VMO.Models;

namespace VMO.API.Models
{
    public class SalesDataMap : ClassMap<SalesData>
    {
        public SalesDataMap()
        {
            Map(m => m.Segment).Name("Segment");
            Map(m => m.Country).Name("Country");
            Map(m => m.Product).Name("Product");
            Map(m => m.DiscountBand).Name("Discount Band");
            Map(m => m.UnitsSold).Name("Units Sold");
            Map(m => m.ManufacturingPrice).Name("Manufacturing Price");
            Map(m => m.SalePrice).Name("Sale Price");
            Map(m => m.Date).Name("Date");
        }
    }
}
