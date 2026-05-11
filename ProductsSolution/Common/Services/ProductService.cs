using ProductsSolution.Domain.Entities;
using ProductsSolution.Infrastructure.Data;
using ProductsSolution.Common.Models;
namespace ProductsSolution.Common.Services
{
    public class ProductService : IProductService
    {
        private readonly AppDbContext DbContext;

        public ProductService(AppDbContext db)
        {
            DbContext = db;
        }

        public IQueryable<Product> GetFilteredQuery(ProductFilterRequest productFilterRequest)
        {
            var query = DbContext.Products.AsQueryable();

            if (!string.IsNullOrWhiteSpace(productFilterRequest.Search))
                query = query.Where(x => x.Name.Contains(productFilterRequest.Search));

            if (productFilterRequest.IsAvailable.HasValue)
                query = query.Where(x => x.IsAvailable == productFilterRequest.IsAvailable);

            if (productFilterRequest.MinPrice.HasValue)
                query = query.Where(x => x.Price >= productFilterRequest.MinPrice);

            if (productFilterRequest.MaxPrice.HasValue)
                query = query.Where(x => x.Price <= productFilterRequest.MaxPrice);

            return query;
        }
    }
}
