﻿using AutoMapper;
using PM.Common.Commands;
using PM.Common.Responses;
using PM.Repository.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PM.Api
{
    class AutoMapping : Profile
    {
        public AutoMapping()
        {
            CreateMap<AddProductCommand, Product>();
            //CreateMap<AddProductCommand, Product>().ForMember(d => d.Stores, opt => opt.Ignore())
                                                   // .ForMember(d => d.ProductGroup, opt => opt.Ignore());

            CreateMap<Product, ProductResponse>()
                .ForMember(dest => dest.ProductGroupName, opt => opt.MapFrom(src => src.ProductGroup.Name))
                .ForMember(dest => dest.Stores, opt => opt.MapFrom(x => x.Stores));

            CreateMap<Store, StoreResponse>()
                .ForMember(des => des.StoreName, opt => opt.MapFrom(src => src.Name));


        }
    }
}