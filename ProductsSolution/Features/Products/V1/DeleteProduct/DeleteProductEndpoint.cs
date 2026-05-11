using FastEndpoints;
using ProductsSolution.Common;
using ProductsSolution.Common.PreProcessors;
using ProductsSolution.Infrastructure.Data;

namespace ProductsSolution.Features.Products.V1.DeleteProduct
{
    public class DeleteProductEndpoint
        : Endpoint<DeleteProductRequest, object>
    {
        private readonly AppDbContext DbContext;

        public DeleteProductEndpoint(AppDbContext db)
        {
            DbContext = db;
        }

        public override void Configure()
        {
            Delete("products/{id}");
            AllowAnonymous();
            Version(1);
            PreProcessor<RequestLoggingPreProcessor<DeleteProductRequest>>();
            PostProcessor<ExecutionTimePostProcessor<DeleteProductRequest, object>>();
        }

        public override async Task<object> ExecuteAsync(DeleteProductRequest request, CancellationToken ct)
        {
            var product = await DbContext.Products.FindAsync(new object[] { request.Id }, ct);

            if (product == null)
            {
                return TypedResults.NotFound(new Common.ErrorResponse
                {
                    Message = "Product not found"
                });
            }

            if (!product.IsAvailable)
            {
                return TypedResults.BadRequest(new Common.ErrorResponse
                {
                    Message = "Business rule failed",
                    Errors = new List<ErrorItem>
                    {
                        new ErrorItem
                        {
                            Field = "Product",
                            Message = "Inactive products cannot be deleted."
                        }
                    }
                });
            }

            DbContext.Products.Remove(product);
            await DbContext.SaveChangesAsync(ct);

            return TypedResults.NoContent();
        }
    }
}