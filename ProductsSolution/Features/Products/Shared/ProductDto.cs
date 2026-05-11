namespace ProductsSolution.Features.Products.Shared
{
    public class ProductV1Dto
    {
        public int Id { get; set; }
        public string Name { get; set; } = "";
        public bool IsAvailable { get; set; }
    }

    public class ProductV2Dto
    {
        public int Id { get; set; }
        public string Name { get; set; } = "";
        public decimal Price { get; set; }
        public bool IsAvailable { get; set; }
    }
}
