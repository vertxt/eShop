using eShop.Shared.Common.Pagination;
using Microsoft.EntityFrameworkCore;

namespace eShop.Business.Extensions
{
    public static class QueryableExtensions
    {
        public static async Task<PagedList<T>> ToPagedList<T>(this IQueryable<T> query, int pageNumber, int pageSize)
        {
            int itemsCount = await query.CountAsync();
            List<T> items = await query.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();

            return new PagedList<T>(items, itemsCount, pageNumber, pageSize);
        }
    }
}
