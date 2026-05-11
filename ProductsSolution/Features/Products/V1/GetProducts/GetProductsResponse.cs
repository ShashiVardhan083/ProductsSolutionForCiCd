namespace ProductsSolution.Features.Products.V1.GetProducts
{
    public class GetProductsResponse
    {
        public int Total { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int TotalPages { get; set; }
        public List<ProductDto> Data { get; set; } = new();
    }
    public class ProductDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = "";
        public decimal Price { get; set; }
        public bool IsAvailable { get; set; }
    }
}