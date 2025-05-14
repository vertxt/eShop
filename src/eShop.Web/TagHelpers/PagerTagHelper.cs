using eShop.Shared.Common.Pagination;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace eShop.Web.TagHelpers
{
    public class PagerTagHelper : TagHelper
    {
        // helper to generate url
        private readonly IUrlHelperFactory _urlHelperFactory;

        public PagerTagHelper(IUrlHelperFactory urlHelperFactory)
        {
            _urlHelperFactory = urlHelperFactory;
        }

        // view context
        [ViewContext]
        [HtmlAttributeNotBound]
        public ViewContext? ViewContext { get; set; }

        // pagination info
        public PaginationMetadata? Metadata { get; set; }

        // routing
        public string PageAction { get; set; } = string.Empty;

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            if (ViewContext is null || Metadata is null) return;
            var urlHelper = _urlHelperFactory.GetUrlHelper(ViewContext);
            var routeValues = new RouteValueDictionary(ViewContext.RouteData.Values);

            foreach (var query in ViewContext.HttpContext.Request.Query)
            {
                routeValues[query.Key] = query.Value;
            }

            var nav = new TagBuilder("nav");
            nav.AddCssClass("d-flex justify-content-between align-items-center p-2 mt-4");
            
            var pageInfoItem = new TagBuilder("small");
            pageInfoItem.InnerHtml.Append($"Page {Metadata.CurrentPage} of {Metadata.TotalPages}");
            pageInfoItem.AddCssClass("text-body-secondary");
            nav.InnerHtml.AppendHtml(pageInfoItem);

            var ul = new TagBuilder("ul");
            ul.AddCssClass("pagination");
            
            // First page item
            var firstLi = CreatePageItem(urlHelper, routeValues, "First", 1, Metadata.HasPreviousPage ? "" : "disabled");
            ul.InnerHtml.AppendHtml(firstLi);

            // Previous page item
            var prevLi = CreatePageItem(urlHelper, routeValues, "&#8249;", Metadata.CurrentPage - 1, Metadata.HasPreviousPage ? "" : "disabled");
            ul.InnerHtml.AppendHtml(prevLi);

            for (int i = 1; i <= Metadata.TotalPages; i++)
            {
                var li = CreatePageItem(urlHelper, routeValues, i.ToString(), i, (i == Metadata.CurrentPage) ? "active" : "");
                ul.InnerHtml.AppendHtml(li);
            }
            
            // Next page item
            var nextLi = CreatePageItem(urlHelper, routeValues, "&#8250;", Metadata.CurrentPage + 1, Metadata.HasNextPage ? "" : "disabled");
            ul.InnerHtml.AppendHtml(nextLi);
            
            // Last page item
            var lastLi = CreatePageItem(urlHelper, routeValues, "Last", Metadata.TotalPages, Metadata.HasNextPage ? "" : "disabled");
            ul.InnerHtml.AppendHtml(lastLi);

            nav.InnerHtml.AppendHtml(ul);
            output.TagName = null;
            output.Content.AppendHtml(nav);
        }

        private TagBuilder CreatePageItem(IUrlHelper urlHelper, RouteValueDictionary routeValues, string content, int pageNumber, string cssClass = "")
        {
            var li = new TagBuilder("li");
            li.AddCssClass("page-item" + " " + cssClass);
            
            var a = new TagBuilder("a");
            a.AddCssClass("page-link");
            a.InnerHtml.AppendHtml(content);
            
            var pageRouteValues = new RouteValueDictionary(routeValues);
            pageRouteValues["PageNumber"] = pageNumber;
            string? href = urlHelper.Action(PageAction, pageRouteValues);
            a.Attributes["href"] = href;
            
            li.InnerHtml.AppendHtml(a);
            
            return li;
        }
    }
}