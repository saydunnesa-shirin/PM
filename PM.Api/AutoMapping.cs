using AutoMapper;
using PM.Common.Commands;
using PM.Common.Responses;
using PM.Repository.Models;

namespace PM.Api
{
    class AutoMapping : Profile
    {
        public AutoMapping()
        {
            CreateMap<AddProductCommand, Product>();

            CreateMap<Product, ProductResponse>()
                .ForMember(dest => dest.ProductGroupName, opt => opt.MapFrom(src => src.ProductGroup.Name))
                .ForMember(dest => dest.Stores, opt => opt.MapFrom(x => x.Stores));

            CreateMap<Store, StoreResponse>()
                .ForMember(des => des.StoreName, opt => opt.MapFrom(src => src.Name));

            CreateMap<ProductGroup, ProductGroupResponse>()
                .ForMember(des => des.GroupName, opt => opt.MapFrom(src => src.Name));


        }
    }
}
