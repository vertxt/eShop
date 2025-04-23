using eShop.Data.Entities.ProductAggregate;
using eShop.Shared.Parameters;

namespace eShop.Business.Extensions
{
    public static class ProductExtensions
    {
        public static IQueryable<Product> Search(this IQueryable<Product> query, string searchTerm)
        {
            if (string.IsNullOrEmpty(searchTerm))
            {
                return query;
            }
            string normalizedString = searchTerm.Trim().ToLower();
            return query.Where(item => item.Name.Contains(normalizedString));
        }

        public static IQueryable<Product> Sort(this IQueryable<Product> query, string sortBy)
        {
            return sortBy switch
            {
                "price" => query.OrderBy(p => p.BasePrice),
                "price-desc" => query.OrderByDescending(p => p.BasePrice),
                "name" => query.OrderBy(p => p.Name),
                "name-desc" => query.OrderByDescending(p => p.Name),
                _ => query
            };
        }

        public static IQueryable<Product> Filter(this IQueryable<Product> query, ProductParameters filterParams)
        {
            if (filterParams is null)
            {
                return query;
            }

            if (filterParams.MaxPrice.HasValue)
            {
                query = query.Where(p => p.BasePrice <= filterParams.MaxPrice);
            }

            if (filterParams.MinPrice.HasValue)
            {
                query = query.Where(p => p.BasePrice >= filterParams.MinPrice);
            }

            if (filterParams.CategoryIds is not null && filterParams.CategoryIds.Count > 0)
            {
                query = query.Where(p => filterParams.CategoryIds.Contains(p.CategoryId));
            }
            
            if (filterParams.InStock.HasValue)
            {
                query = query.Where(p => p.QuantityInStock > 0);
            }
            
            return query;
        }
    }
}