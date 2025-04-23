using AutoMapper;
using eShop.Shared.Common.Pagination;

namespace eShop.Business.Extensions
{
    public static class PagedListExtensions
    {
        public static PagedList<TDestination> MapItems<TSource, TDestination>(
            this PagedList<TSource> pagedList,
            IMapper mapper
        )
        {
            return new PagedList<TDestination>
            {
                Items = mapper.Map<IEnumerable<TDestination>>(pagedList.Items),
                Metadata = pagedList.Metadata,
            };
        }
    }
}