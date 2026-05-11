using FastEndpoints;
using Asp.Versioning;
using Microsoft.EntityFrameworkCore;
using ProductsSolution.Infrastructure.Data;
using ProductsSolution.Features.Products.Shared;

namespace ProductsSolution.Features.Products.QueryVersion.GetProducts
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
            Get("/api/products/query"); // SAME endpoint
            AllowAnonymous();
        }

        public override async Task<object> ExecuteAsync( CancellationToken ct)
        {
            var version = HttpContext.GetRequestedApiVersion()?.MajorVersion ?? 1;

            var products = await DbContext.Products
                .AsNoTracking()
                .ToListAsync(ct);

            // V1
            if (version == 1)
            {
                return new GetProductsQueryResponse
                {
                    Version = "V1 (Query Version)",
                    Message = "Basic data (no price)",
                    Data = products.Select(s => new ProductV1Dto
                    {
                        Id = s.Id,
                        Name = s.Name,
                        IsAvailable = s.IsAvailable
                    }).ToList()

                };

            }
            //V2
            return new GetProductsQueryResponseV2
            {
                Version = "V2 (Query Version)",
                Message = "Full Data",
                Data = products.Select(s => new ProductV2Dto
                {
                    Id = s.Id,
                    Name = s.Name,
                    IsAvailable = s.IsAvailable,
                    Price = s.Price
                }).ToList()

            };
        }
    }
}