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
            return query.Where(item => item.Name.Trim().ToLower().Contains(normalizedString));
        }

        public static IQueryable<Product> Sort(this IQueryable<Product> query, string sortBy)
        {
            return sortBy switch
            {
                "price" => query.OrderBy(p => p.BasePrice),
                "price-desc" => query.OrderByDescending(p => p.BasePrice),
                "name" => query.OrderBy(p => p.Name),
                "name-desc" => query.OrderByDescending(p => p.Name),
                "id" => query.OrderBy(p => p.Id),
                "id-desc" => query.OrderByDescending(p => p.Id),
                "created-date" => query.OrderBy(p => p.CreatedDate),
                "created-date-desc" => query.OrderByDescending(p => p.CreatedDate),
                _ => query
            };
        }

        public static IQueryable<Product> Filter(this IQueryable<Product> query, ProductParameters? filterParams)
        {
            if (filterParams is null)
            {
                return query;
            }

            // By Price Range
            if (filterParams.MaxPrice.HasValue)
            {
                query = query.Where(p => p.BasePrice <= filterParams.MaxPrice.Value);
            }
            if (filterParams.MinPrice.HasValue)
            {
                query = query.Where(p => p.BasePrice >= filterParams.MinPrice.Value);
            }

            // By Categories
            if (filterParams.CategoryIds is not null && filterParams.CategoryIds.Any())
            {
                query = query.Where(p => filterParams.CategoryIds.Contains(p.CategoryId));
            }

            // By Stock Range (All, OutOfStock, Low, Medium, High)
            query = filterParams.StockRange switch
            {
                StockRange.OutOfStock => query.Where(p => p.QuantityInStock <= 0),
                StockRange.Low => query.Where(p => 0 < p.QuantityInStock && p.QuantityInStock <= 10),
                StockRange.Medium => query.Where(p => 10 < p.QuantityInStock && p.QuantityInStock <= 50),
                StockRange.High => query.Where(p => 50 < p.QuantityInStock),
                _ => query
            };

            // By Active Status
            if (filterParams.IsActive.HasValue)
            {
                query = query.Where(p => p.IsActive == filterParams.IsActive.Value);
            }

            // Whether or not has variants
            if (filterParams.HasVariants.HasValue)
            {
                query = query.Where(p => p.HasVariants == filterParams.HasVariants.Value);
            }

            if (filterParams.CreatedBefore.HasValue)
            {
                query = query.Where(p => DateTime.Compare(p.CreatedDate, filterParams.CreatedBefore.Value) < 0);
            }

            if (filterParams.CreatedAfter.HasValue)
            {
                query = query.Where(p => DateTime.Compare(p.CreatedDate, filterParams.CreatedAfter.Value) > 0);
            }

            return query;
        }
    }
}
