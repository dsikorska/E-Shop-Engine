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

                cfg.CreateMap<Category, CategoryViewModel>()
                .ForMember(dest => dest.Products, opt => opt.MapFrom(src => src.Products))
                .ForMember(dest => dest.Subcategories, opt => opt.MapFrom(src => src.Subcategories));

                cfg.CreateMap<Subcategory, SubcategoryViewModel>()
                .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Category.Name))
                .ForMember(dest => dest.CategoryId, opt => opt.MapFrom(src => src.Category.ID))
                .ForMember(dest => dest.Products, opt => opt.MapFrom(src => src.Products));
            });
        }
    }
}