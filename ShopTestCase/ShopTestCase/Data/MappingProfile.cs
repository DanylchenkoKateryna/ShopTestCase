using AutoMapper;
using ShopTestCase.Data.DTO;
using ShopTestCase.Data.Entities;

namespace ShopTestCase.Data
{
    public class MappingProfile : Profile
    {
        public MappingProfile() { 
            CreateMap<ProductForUpdateDTO, Product>();
            CreateMap<ProductForCreateDTO, Product>();
            CreateMap<OrderForCreationDTO, Order>();
            CreateMap<OrderProductDTO, OrderProduct>();
            CreateMap<Product, ProductDTO>();

            CreateMap<Order, OrderForCreationResponse>()
                .AfterMap((src, dest) =>
                {
                    dest.Products=src.OrderProducts.Select(op=> new OrderProductDTO 
                    { 
                        ProductId=op.ProductId,
                        Amount=op.Amount
                    }).ToList();
                });

            CreateMap<Order, OrderDTO>()
            .AfterMap((src, dest) =>
            {
                dest.Products = src.OrderProducts.Select(op => new ProductResponse
                {
                    Id = op.Product.Id,
                    Name = op.Product.Name
                }).ToList();
            });
        }
    }
}
