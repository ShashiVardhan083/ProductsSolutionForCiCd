namespace ProductsSolution.Features.Products.V2.GetProducts
{
    public class ProductDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = "";
        public decimal Price { get; set; }
        public bool IsAvailable { get; set; }

        // New in V2
        public decimal Discount { get; set; }
    }

    public class GetProductsResponse
    {
        public List<ProductDto> Data { get; set; } = new();

        public int? NextCursor { get; set; }
        public int? LastId { get; set; }
        public bool HasMore { get; set; }
    }
}