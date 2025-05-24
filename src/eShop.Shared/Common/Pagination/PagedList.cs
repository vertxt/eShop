using System;
using System.Collections.Generic;

namespace eShop.Shared.Common.Pagination
{
    public class PagedList<T>
    {
        public IEnumerable<T> Items { get; set; } = new List<T>();
        public PaginationMetadata Metadata { get; set; }

        public PagedList() { }

        public PagedList(IList<T> items, int count, int pageNumber, int pageSize)
        {
            Items = items;
            Metadata = new PaginationMetadata
            {
                CurrentPage = pageNumber,
                PageSize = pageSize,
                TotalCount = count,
                TotalPages = (int)Math.Ceiling(count / (double)pageSize),
            };
        }
    }
}
