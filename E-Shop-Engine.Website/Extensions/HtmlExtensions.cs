using System;
using System.Web.Mvc;
using System.Web.Routing;

namespace E_Shop_Engine.Website
{
    public static class HtmlExtensions
    {
        public static MvcHtmlString Image(this HtmlHelper html, byte[] image, string mimeType, string imgClass, object htmlAttributes = null)
        {
            var builder = new TagBuilder("img");
            builder.MergeAttribute("class", imgClass);
            builder.MergeAttributes(new RouteValueDictionary(htmlAttributes));

            var imageString = image != null ? Convert.ToBase64String(image) : "";
            var img = string.Format("data:{0};base64,{1}", mimeType, imageString);
            builder.MergeAttribute("src", img);

            return MvcHtmlString.Create(builder.ToString(TagRenderMode.SelfClosing));
        }
    }
}