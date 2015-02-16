using System.Collections.Generic;
using AspNetIdentityDependencyInjectionSample.DomainClasses;

namespace AspNetIdentityDependencyInjectionSample.ServiceLayer.Contracts
{
    public interface ICategoryService
    {
        void AddNewCategory(Category category);
        IList<Category> GetAllCategories();
    }
}