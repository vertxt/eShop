using System.Reflection;
using Microsoft.AspNetCore.WebUtilities;

namespace eShop.Web.Extensions
{
    public static class QueryStringHelper
    {
        public static string ToQueryString(this object obj)
        {
            var properties = obj.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public);
            var dict = properties.Where(p => p.GetValue(obj) != null)
                .ToDictionary(
                    p => p.Name,
                    p => p.GetValue(obj)?.ToString()
                );
            
            return QueryHelpers.AddQueryString("", dict).TrimStart('?');
        }
    }
}