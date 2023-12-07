using CsvHelper.Configuration.Attributes;


namespace ProductProject.Models
{
    public class Prices
    {
        [Index(0)]
        public string InternalID { get; set; }

        // Product SKU, unique value created by warehouse
        [Index(1)]
        public string SKU { get; set; }
        [Index(2)]
        [TypeConverter(typeof(CustomDecimalConverter))]
        public decimal? NettPrice { get; set; }
        [Index(3)]
        [TypeConverter(typeof(CustomDecimalConverter))]
        public decimal? NettPriceAfterDiscount { get; set; }
        [Index(4)]
        [TypeConverter(typeof(CustomDecimalConverter))]
        public decimal? VATRate { get; set; }
        [Index(5)]
        [TypeConverter(typeof(CustomDecimalConverter))]
        public decimal? NettPriceAfterDiscountForLogisticUnit { get; set; }
    }
}
