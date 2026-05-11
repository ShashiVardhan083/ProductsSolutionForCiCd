using ProductsSolution.Common.Models;

namespace ProductsSolution.Features.Products.V2.GetProducts
{
    public class GetProductsRequest : ProductFilterRequest
    {
        public int PageSize { get; set; } = 5;
        public int? Cursor { get; set; }
    }
}