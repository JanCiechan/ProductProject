namespace ProductProject.Models
{   
    //model used for data collected from database
    public class ProductInfo
    {
        public string NazwaProduktu { get; set; }
        public string EAN { get; set; }
        public string NazwaProducenta { get; set; }
        public string Kategoria { get; set; }
        public string URLdoZdjeciaProduktu { get; set; }
        public int StanMagazynowy { get; set; }
        public string JednostkaLogistycznaProduktu { get; set; }
        public decimal CenaNettoZakupuProduktu { get; set; }
        public decimal KosztDostawy { get; set; }
    }
}
