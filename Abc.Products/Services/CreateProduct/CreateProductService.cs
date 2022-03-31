using Abc.Products.Contracts;
using BaseLib.Core.Services;
using System.Threading.Tasks;

namespace Abc.Products.CreateProduct
{
    internal class CreateProductService : CoreServiceBase<CreateProductRequest, CreateProductResponse>, ICreateProductService
    {
        private readonly INewProductWriter productWriter;

        public CreateProductService(INewProductWriter productWriter)
        {
            this.productWriter = productWriter;
        }

        protected override async Task RunAsync()
        {
            await productWriter.WriteAsync(Request.Product);
            Response.Product = Request.Product;
        }
    }

}

