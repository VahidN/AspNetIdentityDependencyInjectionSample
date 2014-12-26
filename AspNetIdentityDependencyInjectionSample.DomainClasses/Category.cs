using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AspNetIdentityDependencyInjectionSample.DomainClasses
{
    public class Category
    {
        public int Id { get; set; }

        [Required]
        public virtual string Name { get; set; }

        [Required]
        public virtual string Title { get; set; }

        public virtual ICollection<Product> Products { get; set; }
    }
}
