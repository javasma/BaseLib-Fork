using Abc.Products.Models;
using System.Threading.Tasks;

namespace Abc.Products.CreateProduct
{
    public interface INewProductWriter
    {
        Task<int> WriteAsync(ProductType product);
    }
}