using System.Threading.Tasks;
using Abc.Products.Models;
using MySql.Data.MySqlClient;

namespace Abc.Products.CreateProduct.MySql
{
    public class NewProductWriter : INewProductWriter
    {
        const string sql = "INSERT INTO products(SKU, NAME, DESCRIPTION) VALUES( @SKU, @NAME, @DESCRIPTION);";
        private readonly MySqlConnection connection;

        public NewProductWriter(MySqlConnection connection)
        {
            this.connection = connection;
        }
        
        public Task<int> WriteAsync(ProductType product)
        {
            using( var command = new MySqlCommand(sql, connection))
            {
                command.Parameters.AddWithValue("@SKU", product.SKU);
                command.Parameters.AddWithValue("@NAME", product.Name);
                command.Parameters.AddWithValue("@DESCRIPTION", product.Description);
                
                return command.ExecuteNonQueryAsync();
            }
        }
    }
}