using FastEndpoints;
using Microsoft.EntityFrameworkCore;
using ProductsSolution.Infrastructure.Data;
using ProductsSolution.Features.Products.Shared;

namespace ProductsSolution.Features.Products.HeaderVersion.GetProducts
{
    public class Endpoint : EndpointWithoutRequest<object>
    {
        private readonly AppDbContext DbContext;

        public Endpoint(AppDbContext db)
        {
            DbContext = db;
        }

        public override void Configure()
        {
            Get("/api/products/header");
            AllowAnonymous();
            Description(b =>
            {
                b.Produces<GetProductsHeaderResponse>(200);
                b.Produces<GetProductsHeaderResponseV2>(200);
            });
        }

        public override async Task<Object> ExecuteAsync(CancellationToken ct)
        {
            var version = HttpContext.GetRequestedApiVersion()?.MajorVersion ?? 1;
            Console.WriteLine($"Header Version: {version}");
            var products = await DbContext.Products
                .ToListAsync(ct);

            // VERSION 1 RESPONSE
            if (version == 1)
            {
                return new GetProductsHeaderResponse
                {
                    Version = "V1 (Header)",
                    Message = "Basic product data",
                    Data = products.Select(p => new ProductV1Dto
                    {
                        Id = p.Id,
                        Name = p.Name,// V1 doesn't expose price
                        IsAvailable = p.IsAvailable
                    }).ToList()
                };
            }

            // VERSION 2 RESPONSE
            return new GetProductsHeaderResponseV2
            {
                Version = "V2 (Header)",
                Message = "Enhanced product data",
                Data = products.Select(p => new ProductV2Dto
                {
                    Id = p.Id,
                    Name = p.Name,
                    Price = p.Price,
                    IsAvailable = p.IsAvailable
                }).ToList()
            };
        }
    }
}