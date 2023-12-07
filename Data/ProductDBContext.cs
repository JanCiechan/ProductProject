using Microsoft.Data.SqlClient;
using System.Data;

namespace ProductProject.Data
{
    public class ProductDBContext
    {
        private readonly IConfiguration _configuration;
        private readonly string connectionString;

        public ProductDBContext(IConfiguration configuration)
        {
            this._configuration = configuration;
            this.connectionString = this._configuration.GetConnectionString("SqlServer");
        }

        public IDbConnection CreateConnection() => new SqlConnection(connectionString);
    }
}
