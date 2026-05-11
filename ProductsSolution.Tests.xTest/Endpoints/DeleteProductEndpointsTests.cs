using Microsoft.EntityFrameworkCore;
using ProductsSolution.Domain.Entities;
using ProductsSolution.Features.Products.V1.DeleteProduct;
using ProductsSolution.Infrastructure.Data;
using Xunit;
using Microsoft.AspNetCore.Http.HttpResults;
namespace ProductsSolution.Tests.xTest.Endpoints
{
    public class DeleteProductEndpointTests
    {
        private AppDbContext GetDb()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            return new AppDbContext(options);
        }

        [Fact]
        public async Task Should_return_not_found()
        {
            var db = GetDb();
            var endpoint = new DeleteProductEndpoint(db);

            var result = await endpoint.ExecuteAsync(new DeleteProductRequest
            {
                Id = 999
            }, default);

            Assert.IsType<NotFound<Common.ErrorResponse>>(result);
        }

        [Fact]
        public async Task Should_return_bad_request_when_inactive()
        {
            var db = GetDb();

            db.Products.Add(new Product
            {
                Id = 1,
                Name = "Test",
                Price = 100,
                IsAvailable = false
            });

            await db.SaveChangesAsync();

            var endpoint = new DeleteProductEndpoint(db);

            var result = await endpoint.ExecuteAsync(new DeleteProductRequest
            {
                Id = 1
            }, default);

            Assert.IsType<BadRequest<Common.ErrorResponse>>(result);
        }
    }
}
