using System.Collections.Generic;
using E_Shop_Engine.Domain.DomainModel;

namespace E_Shop_Engine.Website.Areas.Admin.Models
{
    public class SubcategoryViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public int CategoryID { get; set; }
        public IEnumerable<Category> Categories { get; set; }

        public static implicit operator SubcategoryViewModel(Subcategory model)
        {
            return new SubcategoryViewModel
            {
                Name = model.Name,
                Description = model.Description,
                CategoryID = model.CategoryID,
                Id = model.ID

            };
        }

        public static implicit operator Subcategory(SubcategoryViewModel model)
        {
            return new Subcategory
            {
                Name = model.Name,
                Description = model.Description,
                CategoryID = model.CategoryID,
                ID = model.Id
            };
        }
    }
}