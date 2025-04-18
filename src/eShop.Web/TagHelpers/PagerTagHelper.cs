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
            
            var nav = new TagBuilder("nav");
            var ul = new TagBuilder("ul");
            ul.AddCssClass("pagination");

            for (int i = 1; i <= Metadata.TotalPages; i++)
            {
                var li = new TagBuilder("li");
                li.AddCssClass("page-item");
                
                var a = new TagBuilder("a");
                a.AddCssClass("page-link");

                string? href = urlHelper.Action(PageAction, new { pageNumber = i });
                a.Attributes["href"] = href;

                if (i == Metadata.CurrentPage)
                {
                    a.AddCssClass("active");
                }

                a.InnerHtml.Append(i.ToString());
                li.InnerHtml.AppendHtml(a);
                ul.InnerHtml.AppendHtml(li);
            }
            
            nav.InnerHtml.AppendHtml(ul);
            output.TagName = null;
            output.Content.AppendHtml(nav);
        }
    }
}