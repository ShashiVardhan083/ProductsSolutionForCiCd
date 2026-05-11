namespace ProductsSolution.Features.Products.V1.PatchProduct
{
    public class PatchProductRequest
    {
        public string? Name { get; set; }
        public decimal? Price { get; set; }
        public bool? IsAvailable { get; set; }
    }
}
