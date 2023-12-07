using Microsoft.AspNetCore.Http.HttpResults;

namespace ProductProject.Models
{
    public class Product
    {
        public string ID { get; set; }

        // Product SKU, unique value created by warehouse
        public string SKU { get; set; }

        public string? name { get; set; }

        // Product number
        public string? EAN { get; set; }

        public string? producer_name { get; set; }

        public string? category { get; set; }

        // Indicates whether the product is a wire (if value is 1) overall not required for the task, but might be needed later
        public bool? is_wire { get; set; }

        // Indicates whether the product is available for order (if value is 1)
        public bool? available { get; set; }

        // Indicates whether the product is shipped by supplier or warehouse. If value is 0, it’s shipped by warehouse, if 1, it’s shipped by supplier.
        public bool? is_vendor { get; set; }

        // URL address to product’s image
        public string? default_image { get; set; }
    }
}
