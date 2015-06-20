using MarvelousFileManager.Models;
using System;
using System.Collections.Specialized;
using System.Web.Mvc;

namespace MarvelousFileManager.Helpers
{
    public static class HtmlHelpers
    {
        public static MvcHtmlString BreadcrumbFromPath(this HtmlHelper html, string element, PaneState.PaneType type, string path, NameValueCollection requestParams, bool active = false)
        {
            var liTag = new TagBuilder("li");
            var urlHelper = new UrlHelper(html.ViewContext.RequestContext);
            string url = "";
            switch (type)
            {
                case PaneState.PaneType.Left:
                    url = urlHelper.Action("Index", new { leftPath = path, rightPath = requestParams["rightPath"] });
                    break;
                case PaneState.PaneType.Right:
                    url = urlHelper.Action("Index", new { leftPath = requestParams["leftPath"], rightPath = path });
                    break;
            }
            var aTag = new TagBuilder("a");
            aTag.MergeAttribute("href", url);
            aTag.InnerHtml = element;

            liTag.InnerHtml = aTag.ToString();
            if (active)
            {
                liTag.AddCssClass("active");
            }

            return MvcHtmlString.Create(liTag.ToString());
        }
    }
}
