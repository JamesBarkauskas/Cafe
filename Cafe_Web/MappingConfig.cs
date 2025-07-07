using AutoMapper;
using Cafe_Web.Models;
using Cafe_Web.Models.Dto;

namespace Cafe_Web
{
    public class MappingConfig : Profile
    {
        public MappingConfig()
        {
            CreateMap<Product, ProductDTO>().ReverseMap();
            CreateMap<Product, ProductCreateDTO>().ReverseMap();
            CreateMap<Product, ProductUpdateDTO>().ReverseMap();
            CreateMap<Product, ProductUpdateDTO>().ReverseMap();
            CreateMap<ProductDTO, ProductUpdateDTO>().ReverseMap();
        }
    }
}
