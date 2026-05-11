using ProductsSolution.Features.Products.Shared;

namespace ProductsSolution.Features.Products.QueryVersion.GetProducts
{
    public class GetProductsQueryResponse
    {
        public string Version { get; set; } = string.Empty;

        public string Message { get; set; } = string.Empty;

        public List<ProductV1Dto> Data { get; set; } = new();
    }

    public class GetProductsQueryResponseV2
    {
        public string Version { get; set; } = string.Empty;

        public string Message { get; set; } = string.Empty;

        public List<ProductV2Dto> Data { get; set; } = new();
    }
}