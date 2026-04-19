using AutoMapper;
using POS.Application.DTOs;
using POS.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace POS.Application.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile() 
        {
            //Product
            CreateMap<Product, ProductDto>()
                .ForMember(d => d.Categories,
                o => o.MapFrom(s => s.ProductCategories
                .Select(pc => pc.Category)));


            CreateMap<CreateProductDto, Product>();
            CreateMap<UpdateProductDto, Product>();

        }
    }
}
