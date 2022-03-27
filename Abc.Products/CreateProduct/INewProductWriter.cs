using Abc.Products.Models;
using System.Threading.Tasks;

namespace Abc.Products.CreateProduct
{
    internal interface INewProductWriter
    {
        Task WriteAsync(ProductType product);
    }
}