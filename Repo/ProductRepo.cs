using Dapper;
using ProductProject.Data;
using ProductProject.Models;
using System.Data;
using System.Data.Common;

namespace ProductProject.Repo
{
    public class ProductRepo : IProductRepo
    {
        private readonly ProductDBContext _dbContext;
        public ProductRepo(ProductDBContext dbContext)
        {
            this._dbContext = dbContext;
        }
        //not required for task
        public async Task<List<Product>> GetAll()
        {
            string query = "SELECT * FROM Product";
            using ( var connection = this._dbContext.CreateConnection() )
            {
                var prodList = await connection.QueryAsync<Product>(query);
                return prodList.ToList();
            }
        }
        public void InsertProducts(IEnumerable<Product> products)
        {

            using (var connection = this._dbContext.CreateConnection())
            {
                foreach (var product in products)
                {

                    connection.Execute(
                        @"INSERT INTO Product (ID, SKU, name , EAN, producer_name , category , is_wire , available , is_vendor , default_image ) 
                          VALUES (@ID, @SKU, @name , @EAN, @producer_name , @category , @is_wire , @available , @is_vendor , @default_image )",
                        product);
                }
            }
        }

        public void InsertInventory(IEnumerable<Inventory> inventory)
        {

            using (var connection = this._dbContext.CreateConnection())
            {
                foreach (var item in inventory)
                {

                    

                    connection.Execute(
                         @"INSERT INTO Inventory (product_id, sku, unit, qty, manufacturer_name, shipping, shipping_cost) 
                          VALUES (@product_id , @sku , @unit , @qty , @manufacturer_name , @shipping , @shipping_cost )",
                         item);
                }
            }
        }

        public void InsertPrices(IEnumerable<Prices> prices)
        {
            using (var connection = this._dbContext.CreateConnection())
            {
                foreach (var price in prices)
                {
                    connection.Execute(
                        @"INSERT INTO Prices (InternalID, SKU, NettPrice, NettPriceAfterDiscount, VATRate, NettPriceAfterDiscountForLogisticUnit) 
                          VALUES (@InternalID, @SKU, @NettPrice, @NettPriceAfterDiscount, @VATRate, @NettPriceAfterDiscountForLogisticUnit)",
                        price);
                }
            }
        }
        public async Task<ProductInfo> GetBySKU(string sku)
        {
            string query = $@"
                SELECT
                    p.name AS 'Nazwa produktu',
                    p.EAN,
                    p.producer_name AS 'Nazwa producenta',
                    p.category AS 'Kategoria',
                    p.default_image AS 'URL do zdjęcia produktu',
                    i.qty AS 'Stan magazynowy',
                    i.unit AS 'Jednostkę logistyczną produktu',
                    pr.NettPrice AS 'Cenę netto zakupu produktu',
                    i.shipping_cost AS 'Koszt dostawy'
                FROM
                    Product p
                JOIN
                    Inventory i ON p.sku = i.sku
                JOIN
                    Prices pr ON p.sku = pr.sku
                WHERE
                    p.SKU = '{sku}';
            ";
            using (var connection = this._dbContext.CreateConnection())
            {
                var product = await connection.QueryFirstOrDefaultAsync<ProductInfo>(query);
                if (product != null) { 
                    return product; 
                }
                return null;
            }
            
        }
    }
}
