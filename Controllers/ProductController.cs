using Microsoft.AspNetCore.Mvc;
using ProductProject.Models;
using ProductProject.Repo;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using ProductProject.Repo.DatabaseUtility;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace ProductProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductRepo repo;
        public ProductController(IProductRepo repo)
        {
            this.repo = repo;
        }
        [HttpGet]
        [ProducesResponseType(typeof(List<Product>), 200)]
        [ProducesResponseType(204)]
        public async Task<IActionResult> GetAll()
        {
            var _list = await this.repo.GetAll();
            if (_list != null)
            {
                return Ok(_list);
            }
            else
            {
                
                return NoContent();
                
            }
        }
        [HttpPost]
        [ProducesResponseType(200)]
        public async Task<IActionResult> UpdateDatabase()
        {
            await ProductsCsvReader.DownloadRequiredFiles();
            ProductsCsvReader.InsertDataFromCsv(this.repo);

            return Ok("Database updated successfully.");
        }
        [HttpGet("{sku}")]
        [ProducesResponseType(typeof(ProductInfo), 200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetProductBySku(string sku)
        {
                var product = await this.repo.GetBySKU(sku);
                if (product == null)
                {
                    return NotFound(); 
                }

                return Ok(product); 
        }
    }
}

