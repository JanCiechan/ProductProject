using CsvHelper.Configuration.Attributes;
using Microsoft.AspNetCore.Http.HttpResults;

namespace ProductProject.Models
{
    public class Inventory
    {
        public string product_id { get; set; }

        // Product SKU, unique value created by warehouse
        public string sku { get; set; }

        // Type of unit the product is sold as
        public string? unit { get; set; }

        // Stock quantity
        [TypeConverter(typeof(CustomIntConverter))]
        public int? qty { get; set; }

        public string? manufacturer_name { get; set; }

        // Shipping time
        public string? shipping { get; set; }

        // Nett cost for shipping product
        [TypeConverter(typeof(CustomDecimalConverter))]
        public decimal? shipping_cost { get; set; }
    }
}
