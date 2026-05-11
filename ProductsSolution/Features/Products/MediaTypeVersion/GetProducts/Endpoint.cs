using FastEndpoints;
using Asp.Versioning;
using Microsoft.EntityFrameworkCore;
using ProductsSolution.Infrastructure.Data;
using ProductsSolution.Features.Products.Shared;

namespace ProductsSolution.Features.Products.MediaTypeVersion.GetProducts
{
    public class Endpoint
        : EndpointWithoutRequest<Object>
    {
        private readonly AppDbContext DbContext;

        public Endpoint(AppDbContext db)
        {
            DbContext = db;
        }

        public override void Configure()
        {
            //HTTP Method + Route
            Get("/api/products/media");

            //Authorization
            AllowAnonymous(); 

            //Description (technical)
            Description(b =>
            {
                b.Produces<List<Object>>(200);
                b.Produces(400);
                b.Produces(404);
            });



        }

        public override async Task<Object> ExecuteAsync(CancellationToken ct)
        {
            var version = HttpContext.GetRequestedApiVersion()?.MajorVersion ?? 1;
            Console.WriteLine($"MediaType Version: {version}");
            var products = await DbContext.Products
                .AsNoTracking()
                .ToListAsync(ct);

            // V1
            if (version == 1)
            {
                return new GetProductsMediaResponse
                {
                    Version = "V1 (Media Type)",
                    Message = "Basic data (no price)",
                    Data = products.Select(p => new ProductV1Dto
                    {
                        Id = p.Id,
                        Name = p.Name,
                        IsAvailable = p.IsAvailable
                    }).ToList()
                };
            }

            // V2
            return new GetProductsMediaResponseV2
            {
                Version = "V2 (Media Type)",
                Message = "Full data",
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