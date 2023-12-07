using CsvHelper;
using Microsoft.Data.SqlClient;
using ProductProject.Models;
using System;
using System.Globalization;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using System.Globalization;
using CsvHelper.Configuration;
using CsvHelper.TypeConversion;
using System.Diagnostics;


namespace ProductProject.Repo.DatabaseUtility
{
    enum Data { product, inventory, price }
    enum DataDetails { location, url }
    
    public class ProductsCsvReader
    {
        //we will use this array with enums to describe the contained fields
        private static string[,] files = new string[,]{ 
            { "Product.csv", "https://rekturacjazadanie.blob.core.windows.net/zadanie/Products.csv" },
            { "Inventory.csv","https://rekturacjazadanie.blob.core.windows.net/zadanie/Inventory.csv" },
            { "Price.csv","https://rekturacjazadanie.blob.core.windows.net/zadanie/Prices.csv" } };
        
        public static async Task DownloadRequiredFiles()
        {

            for(int i=0;i<files.GetLength(0); i++)
            {
                await DownloadFileAsync(files[i, (int)DataDetails.url], files[i, (int)DataDetails.location]);
            }
            Console.WriteLine("File downloaded successfully.");
        }

        static async Task DownloadFileAsync(string fileUrl, string localFilePath)
        {
            using (HttpClient httpClient = new HttpClient())
            {
                //big file - 15 minutes timeout?
                httpClient.Timeout = TimeSpan.FromMinutes(15);

                HttpResponseMessage response = await httpClient.GetAsync(fileUrl);

                response.EnsureSuccessStatusCode();

                using (Stream contentStream = await response.Content.ReadAsStreamAsync())
                {

                    using (FileStream fileStream = File.Create(localFilePath))
                    {
                        await contentStream.CopyToAsync(fileStream);
                        fileStream.Close(); 
                    }
                }
            }
        }
        public static void InsertDataFromCsv(IProductRepo repo)
        {
            //according to task we need to process inventory first to get info about shipping for products
            IEnumerable<Inventory> items=ProcessInventoryData(repo);
            ProcessProductData(repo,items);
            ProcessPriceData(repo);
            DeleteFiles();
        }
        private static void DeleteFiles()
        {
            for (int i = 0; i < files.GetLength(0); i++)
            {
                File.Delete(files[i, (int)DataDetails.location]);
            }
        }
        private static IEnumerable<Inventory> ProcessInventoryData(IProductRepo repo)
        {
            var csvConfig = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                MissingFieldFound = null
            };
            csvConfig.ReadingExceptionOccurred = re =>
            {
            
                Console.WriteLine($"Bad Row in Inventor; CSV ERROR: {re.Exception}");
                return false;
            };

            using (var reader = new StreamReader(files[(int)Data.inventory, (int)DataDetails.location]))
            using (var csv = new CsvReader(reader, csvConfig))
            {
                //we are only interested in 24h shipping
                var records = csv.GetRecords<Inventory>().Where(p => p.shipping.Equals("24h")).ToList();
                records.RemoveAll(item => item.product_id == null || item.sku == null);
                repo.InsertInventory(records);

                return records.ToList();

            }
            

        }
        private static void ProcessProductData(IProductRepo repo, IEnumerable<Inventory> items)
        {
            //different configs for different csv due to different data conventions
            var csvConfig = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                Delimiter = ";",
                MissingFieldFound = null
            };
            csvConfig.ReadingExceptionOccurred = re =>
            {

                Console.WriteLine($"Bad Row in Product; CSV ERROR: {re.Exception}");
                return false;
            };
            using (var reader = new StreamReader(files[(int)Data.product, (int)DataDetails.location]))
            using (var csv = new CsvReader(reader, csvConfig))
            {
                //we are not interested in records that are wires or have null required values
                var records = csv.GetRecords<Product>().Where(p => !(p.is_wire == null || (bool)p.is_wire));
                List<Product> filteredProducts = records
                    .Where(product => items.Any(item => item.sku == product.SKU))
                    .ToList();
                filteredProducts.RemoveAll(product => product.ID == null || product.SKU == null);

                repo.InsertProducts(filteredProducts);
            }
        }

        private static void ProcessPriceData(IProductRepo repo)
        {
            var csvConfig = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                HasHeaderRecord = false,
                MissingFieldFound = null,
                
            BadDataFound = context =>
                {
                    Console.WriteLine($"Bad data found at row{context.RawRecord}");

                }
            };
          
            using (var reader = new StreamReader(files[(int)Data.price, (int)DataDetails.location]))
            using (var csv = new CsvReader(reader, csvConfig))
            {
                var records = csv.GetRecords<Prices>().ToList();
                
                
                records.RemoveAll(record => record.InternalID == null || record.SKU == null);
                
                repo.InsertPrices(records);
            }
        }
    }

}   
