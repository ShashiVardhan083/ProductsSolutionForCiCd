namespace ProductsSolution.Common.Models
{
    public class ProductFilterRequest
    {
        public string? Search { get; set; }
        public bool? IsAvailable { get; set; }
        public decimal? MinPrice { get; set; }
        public decimal? MaxPrice { get; set; }
    }
}