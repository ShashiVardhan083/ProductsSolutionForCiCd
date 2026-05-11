using ProductsSolution.Common.Models;
using ProductsSolution.Domain.Entities;

public interface IProductService
{
    IQueryable<Product> GetFilteredQuery(ProductFilterRequest req);
}