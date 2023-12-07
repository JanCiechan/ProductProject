using ProductProject.Models;

namespace ProductProject.Repo
{
    public interface IProductRepo
    {
        public Task<List<Product>> GetAll();
        void InsertInventory(IEnumerable<Inventory> records);
        void InsertPrices(IEnumerable<Prices> records);
        void InsertProducts(IEnumerable<Product> products);
        public Task<ProductInfo> GetBySKU(string sku);
    }
}
