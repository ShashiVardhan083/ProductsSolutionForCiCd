using ProductsSolution.Common.Models;

namespace ProductsSolution.Features.Products.V1.GetProducts
{
    public class GetProductsRequest : ProductFilterRequest
    {
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 3;
    }
}