using System;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ProductsSolution.Common;
using ProductsSolution.Domain.Entities;
using ProductsSolution.Features.Products.V1.DeleteProduct;
using ProductsSolution.Infrastructure.Data;

namespace ProductsSolution.Tests.MSTest.Endpoints
{
    [TestClass]
    public class DeleteProductEndpointTests
    {
        private AppDbContext GetDb()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            return new AppDbContext(options);
        }

        [TestMethod]
        public async Task Should_return_not_found()
        {
            var db = GetDb();
            var endpoint = new DeleteProductEndpoint(db);

            var result = await endpoint.ExecuteAsync(new DeleteProductRequest
            {
                Id = 1
            }, default);

            Assert.IsInstanceOfType(result, typeof(NotFound<ErrorResponse>));
        }

        [TestMethod]
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
                Id = 1 // must match inserted
            }, default);

            Assert.IsInstanceOfType(result, typeof(BadRequest<ErrorResponse>));
        }
    }
}