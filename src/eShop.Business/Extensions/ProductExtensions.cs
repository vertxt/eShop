using eShop.Data.Entities.Products;

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
    }
}