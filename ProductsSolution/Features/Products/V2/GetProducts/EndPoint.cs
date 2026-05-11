using FastEndpoints;
using Microsoft.EntityFrameworkCore;

namespace ProductsSolution.Features.Products.V2.GetProducts
{
    public class Endpoint
        : Endpoint<GetProductsRequest, GetProductsResponse>
    {
        private readonly IProductService ProductService;

        public Endpoint(IProductService service)
        {
            ProductService = service;
        }

        public override void Configure()
        {
            Get("products");
            AllowAnonymous();
            Version(2);
        }

        public override async Task<GetProductsResponse> ExecuteAsync(
            GetProductsRequest getProductRequest,
            CancellationToken ct)
        {
            var query = ProductService.GetFilteredQuery(getProductRequest);

            // Cursor-based pagination (V2 specific)
            if (getProductRequest.Cursor.HasValue)
                query = query.Where(x => x.Id > getProductRequest.Cursor);

            var data = await query
                .OrderBy(x => x.Id)
                .Take(getProductRequest.PageSize)
                .ToListAsync(ct);

            var lastItem = data.LastOrDefault();

            return new GetProductsResponse
            {
                Data = data.Select(x => new ProductDto
                {
                    Id = x.Id,
                    Name = x.Name,
                    Price = x.Price,
                    IsAvailable = x.IsAvailable,
                    Discount = x.Price * 0.1m
                }).ToList(),

                NextCursor = lastItem?.Id,
                LastId = lastItem?.Id,
                HasMore = data.Count == getProductRequest.PageSize
            };
        }
    }
}