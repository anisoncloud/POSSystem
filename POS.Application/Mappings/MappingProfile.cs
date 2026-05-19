using AutoMapper;
using POS.Application.DTOs;
using POS.Application.ViewModels;
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


            CreateMap<CreateProductDto, Product>()
             //.ForMember(d => d.ImageUrl, o => o.Ignore())  // handled manually in service
            .ForMember(d => d.SKU, o => o.Ignore())  // generated in service
            .ForMember(d => d.Barcode, o => o.Ignore())  // generated in service
            .ForMember(d => d.BranchId, o => o.Ignore())  // set from claims in service
            .ForMember(d => d.ProductCategories, o => o.Ignore()); // set manually
            
            
            CreateMap<UpdateProductDto, Product>()
             //.ForMember(d => d.ImageUrl, o => o.Ignore())  // handled manually in service
            .ForMember(d => d.ProductCategories, o => o.Ignore()) // synced manually
            .ForMember(d => d.SKU, o => o.Ignore())  // never changes on update
            .ForMember(d => d.Barcode, o => o.Ignore())  // use RegenerateBarcode instead
            .ForMember(d => d.BranchId, o => o.Ignore()); // never changes on update

            // ── Category ──────────────────────────────────────────────────────
            CreateMap<Category, CategoryDto>()
                .ForMember(d => d.ParentCategoryName,
                o => o.Ignore());
            CreateMap<CategoryCreateDto, Category>()
                .ForMember(d=>d.SubCategories, o=>o.Ignore())
                .ForMember(d=>d.ProductCategories, o=>o.Ignore())
                .ForMember(d=>d.ParentCategory, o=>o.Ignore());
            CreateMap<CategoryUpdateDto, Category>()
                .ForMember(d=>d.SubCategories, o=>o.Ignore())
                .ForMember(d=>d.ProductCategories, o=>o.Ignore())
                .ForMember(d=>d.ParentCategory, o=>o.Ignore());

            //CreateMap<Source, Destination>().ForMember( of destination parentcategoryname) .MapFrom(source ParentCategory.Name)
            CreateMap<Category, CategoryDto>()
                .ForMember(d => d.ParentCategoryName, 
                c => c.MapFrom(p => p.ParentCategory.Name)); //** Using in CategoryService

            // ── Order to OrderDto ─────────────────────────────────────────────────────────
            // Order → OrderDto — null-safe mapping CreateMap<Source, Destination>().ForMember(Destination, MapFrom(Source))
            CreateMap<Order, OrderDto>()
                .ForMember(d => d.CashierName,
                    o => o.MapFrom(s =>
                        s.Cashier != null
                            ? s.Cashier.FullName
                            : string.Empty))
                .ForMember(d => d.BranchName,
                    o => o.MapFrom(s =>
                        s.Branch != null
                            ? s.Branch.Name
                            : string.Empty))
                .ForMember(d => d.TableId,
                    o => o.MapFrom(s =>
                        s.Table != null
                            ? s.Table.TableNumber
                            : null))
                .ForMember(d => d.Items,
                    o => o.MapFrom(s => s.Items ?? new List<OrderItem>()))
                .ForMember(d => d.Payment,
                    o => o.MapFrom(s => s.Payments ?? new List<Payment>()));

            // ── Order Detail View ─────────────────────────────────────────────────────────
            CreateMap<Order, OrderDetailViewModel>()
                .ForMember(d => d.CashierName,
                    o => o.MapFrom(s => s.Cashier != null
                        ? s.Cashier.FullName
                        : "N/A"))
                .ForMember(d => d.TableNumber,
                    o => o.MapFrom(s => s.Table != null
                        ? s.Table.TableNumber
                        : null))
                .ForMember(d => d.BranchName,
                    o => o.MapFrom(s => s.Branch.Name))
                .ForMember(d => d.BranchAddress,
                    o => o.MapFrom(s => s.Branch.Address))
                .ForMember(d => d.BranchPhone,
                    o => o.MapFrom(s => s.Branch.Phone))
                .ForMember(d => d.Items,
                    o => o.MapFrom(s => s.Items))
                .ForMember(d => d.Payments,
                    o => o.MapFrom(s => s.Payments));

            // ── OrderItem ─────────────────────────────────────────────────────
            CreateMap<OrderItem, OrderItemDto>();

            // ── Payment ───────────────────────────────────────────────────────
            CreateMap<Payment, PaymentDto>();

            // ── Table ─────────────────────────────────────────────────────────
            CreateMap<Table, TableDto>().ReverseMap();

            //----Branch--------------------------------------------------------
            CreateMap<Branch, BranchDto>()
                .ForMember(b=>b.TotalUsers,
                o=>o.Ignore())
                .ForMember(x=>x.TotalProducts,
                y=>y.Ignore());
            CreateMap<CreateBranchDto, Branch>();
            CreateMap<UpdateBranchDto, Branch>();

        }
    }
}
