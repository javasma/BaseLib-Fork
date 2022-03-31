using Abc.Products.Models;
using BaseLib.Core.Services;

namespace Abc.Products.Contracts
{
    public class CreateProductRequest : ICoreServiceRequest
    {
        public ProductType Product { get; set; }
    }

}

