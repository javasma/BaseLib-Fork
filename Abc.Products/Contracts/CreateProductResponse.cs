using Abc.Products.Models;
using BaseLib.Core.Services;

namespace Abc.Products.Contracts
{
    public class CreateProductResponse : CoreServiceResponseBase<ProductReasonCode>
    {
        public ProductType Product { get; set; }
    }

}

