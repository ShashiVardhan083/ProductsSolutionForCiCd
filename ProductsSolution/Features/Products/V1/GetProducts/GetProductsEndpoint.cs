using FastEndpoints;
using Microsoft.EntityFrameworkCore;
using ProductsSolution.Common.PreProcessors;
namespace ProductsSolution.Features.Products.V1.GetProducts
{
    public class GetProductsEndpoint
        : Endpoint<GetProductsRequest, GetProductsResponse>
    {
        private readonly IProductService ProductService;
        public GetProductsEndpoint(IProductService service)
        {
            ProductService = service;
        }
        

        public override void Configure()
        {
            Get("products");
            AllowAnonymous();
            Version(1);

            PreProcessor<RequestLoggingPreProcessor<GetProductsRequest>>();
            PostProcessor<ExecutionTimePostProcessor<GetProductsRequest, GetProductsResponse>>();
        }

        public override async Task<GetProductsResponse> ExecuteAsync(
            GetProductsRequest getProductsRequest,
            CancellationToken ct)
        {
            

            var query = ProductService.GetFilteredQuery(getProductsRequest);

            var total = await query.CountAsync(ct);

            var data = await query
                .OrderBy(x => x.Id)
                .Skip((getProductsRequest.PageNumber - 1) * getProductsRequest.PageSize)
                .Take(getProductsRequest.PageSize)
                .Select(x => new ProductDto
                {
                    Id = x.Id,
                    Name = x.Name,
                    Price = x.Price,
                    IsAvailable = x.IsAvailable
                })
                .ToListAsync(ct);

            return new GetProductsResponse
            {
                Total = total,
                PageNumber = getProductsRequest.PageNumber,
                PageSize = getProductsRequest.PageSize,
                TotalPages = (int)Math.Ceiling((double)total / getProductsRequest.PageSize),
                Data = data
            };
        }
    }
}