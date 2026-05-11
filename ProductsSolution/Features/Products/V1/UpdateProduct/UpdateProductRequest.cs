namespace ProductsSolution.Features.Products.V1.UpdateProduct
{
    public class UpdateProductRequest
    {
        public string Name { get; set; } = "";
        public decimal Price { get; set; }
        public bool IsAvailable { get; set; }
    }
}
