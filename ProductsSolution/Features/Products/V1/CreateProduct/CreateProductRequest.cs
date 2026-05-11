namespace ProductsSolution.Features.Products.V1.CreateProduct
{
    public class CreateProductRequest
    {
        public string Name { get; set; } = "";
        public decimal Price { get; set; }
        public bool IsAvailable { get; set; }
    }
}
