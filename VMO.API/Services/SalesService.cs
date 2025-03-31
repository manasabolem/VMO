using CsvHelper;
using CsvHelper.Configuration;
using System.Globalization;
using System.Text;
using VMO.Models;
using VMO.API.Services.Interfaces;
using VMO.API.Models;

namespace VMO.API.Services
{
    public class SalesService : ISalesService
    {
        private readonly IWebHostEnvironment _env;

        public SalesService(IWebHostEnvironment env)
        {
            _env = env;
        }

        public List<SalesData> GetSalesData()
        {
            var filePath = Path.Combine(_env.WebRootPath, "Data.csv");
            if (!File.Exists(filePath))
                throw new FileNotFoundException($"CSV file not found at {filePath}");

            var config = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                Delimiter = ",",
                HasHeaderRecord = true,
                PrepareHeaderForMatch = args => args.Header.Trim(),
                HeaderValidated = null,
                IgnoreBlankLines = true
            };

            using (var reader = new StreamReader(filePath, Encoding.GetEncoding("Windows-1252")))
            using (var csv = new CsvReader(reader, config))
            {
                csv.Context.RegisterClassMap<SalesDataMap>();
                var records = csv.GetRecords<SalesData>().ToList();
                return records;
            }
        }
    }
}
