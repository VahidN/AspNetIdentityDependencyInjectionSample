using System.Collections.Generic;
using AspNetIdentityDependencyInjectionSample.DomainClasses;

namespace AspNetIdentityDependencyInjectionSample.ServiceLayer.Contracts
{
    public interface IProductService
    {
        void AddNewProduct(Product product);
        IList<Product> GetAllProducts();
    }
}