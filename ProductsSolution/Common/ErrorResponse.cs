namespace ProductsSolution.Common
{
    public class ErrorResponse
    {
        public string Message { get; set; } = default!;
        public List<ErrorItem> Errors { get; set; } = new();
    }

    public class ErrorItem
    {
        public string Field { get; set; } = default!;
        public string Message { get; set; } = default!;
    }
}
