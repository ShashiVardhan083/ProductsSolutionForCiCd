using ProductsSolution.Features.Products.Shared;

namespace ProductsSolution.Features.Products.MediaTypeVersion.GetProducts
{
    public class GetProductsMediaResponse
    {
        public string Version { get; set; } = string.Empty;

        public string Message { get; set; } = string.Empty;

        public List<ProductV1Dto> Data { get; set; } = new();
    }
    public class GetProductsMediaResponseV2
    {
        public string Version { get; set; } = string.Empty;

        public string Message { get; set; } = string.Empty;

        public List<ProductV2Dto> Data { get; set; } = new();
    }
}