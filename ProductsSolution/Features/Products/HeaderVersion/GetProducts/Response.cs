using ProductsSolution.Features.Products.Shared;
namespace ProductsSolution.Features.Products.HeaderVersion.GetProducts
{
    public class GetProductsHeaderResponse
    {
        public string Version { get; set; } = string.Empty;

        public List<ProductV1Dto> Data { get; set; } = new();

        public string Message { get; set; } = string.Empty;
    }
    public class GetProductsHeaderResponseV2
    {
        public string Version { get; set; } = string.Empty;
        public List<ProductV2Dto> Data { get; set; } = new();
        public string Message { get; set; } = string.Empty;
    }
}