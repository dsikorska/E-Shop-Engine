using AutoMapper;
using E_Shop_Engine.Domain.DomainModel;
using E_Shop_Engine.Website.Models;

namespace E_Shop_Engine.Website.App_Start
{
    public static class AutoMapperConfig
    {
        public static void RegisterMappings()
        {
            Mapper.Initialize(cfg =>
            {
                cfg.CreateMap<Product, ProductViewModel>()
                    .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Category.Name))
                    .ForMember(dest => dest.SubcategoryName, opt => opt.MapFrom(src => src.Subcategory.Name));

                cfg.CreateMap<Category, CategoryViewModel>();

                cfg.CreateMap<Subcategory, SubcategoryViewModel>();
            });
        }
    }
}