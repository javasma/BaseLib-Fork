using Abc.Products;
using Abc.Products.Models;
using BaseLib.Core.Services;
using System.Threading.Tasks;

namespace Abc.Products.CreateProduct
{
    internal class CreateProductService : CoreServiceBase<CreateProductRequest, CreateProductResponse>
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

namespace Abc.Products.CreateProduct
{
    internal class CreateProductRequest : ICoreServiceRequest
    {
        public ProductType Product { get; set; }
    }
}

namespace Abc.Products.CreateProduct
{
    internal class CreateProductResponse : CoreServiceResponseBase<ProductReasonCode>
    {
        public ProductType Product { get; set; }
    }

}

