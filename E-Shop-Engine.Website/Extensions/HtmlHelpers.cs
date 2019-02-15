using System;
using System.Linq.Expressions;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using E_Shop_Engine.Domain.Abstract;
using E_Shop_Engine.Domain.DomainModel;

namespace E_Shop_Engine.Website.Extensions
{
    public static class HtmlHelpers
    {
        /// <summary>
        /// Get instance of Settings.
        /// </summary>
        /// <param name="html"></param>
        /// <returns>Settings.</returns>
        public static Settings Settings(this HtmlHelper html)
        {
            ISettingsRepository settingsRepository = DependencyResolver.Current.GetService<ISettingsRepository>();
            return settingsRepository.Get();
        }

        public static IHtmlString EndingFormButtons(this HtmlHelper html, string submitButtonText = "Save", string anchorValue = "Go Back", string anchorAction = "GoBack", string anchorController = "Home")
        {
            TagBuilder div = new TagBuilder("div");
            div.AddCssClass("input-group mb-3 my-3 d-flex justify-content-around");

            MvcHtmlString anchor = LinkExtensions.ActionLink(html, anchorValue, anchorAction, anchorController, new { area = "" }, new { @class = "btn btn-secondary btn-lg" });

            TagBuilder submit = new TagBuilder("input");
            submit.AddCssClass("btn btn-success btn-lg");
            submit.MergeAttribute("type", "submit");
            submit.MergeAttribute("value", submitButtonText);

            StringBuilder htmlBuilder = new StringBuilder();
            htmlBuilder.Append(anchor.ToHtmlString());
            htmlBuilder.Append(submit.ToString(TagRenderMode.SelfClosing));

            div.InnerHtml = htmlBuilder.ToString();
            return MvcHtmlString.Create(div.ToString(TagRenderMode.Normal));
        }

        public static IHtmlString TextBoxForExtended<TModel, TValue>(this HtmlHelper<TModel> html, Expression<Func<TModel, TValue>> property)
        {
            TagBuilder outerDiv = new TagBuilder("div");
            outerDiv.AddCssClass("input-group mb-3");

            TagBuilder innerDiv = new TagBuilder("div");
            innerDiv.AddCssClass("input-group-prepend");

            MvcHtmlString label = LabelExtensions.LabelFor(html, property, new { @class = "input-group-text" });

            MvcHtmlString textBox = InputExtensions.TextBoxFor(html, property, new { @class = "form-control" });

            StringBuilder htmlBuilder = new StringBuilder();
            htmlBuilder.Append(innerDiv.ToString(TagRenderMode.StartTag));
            htmlBuilder.Append(label.ToHtmlString());
            htmlBuilder.Append(innerDiv.ToString(TagRenderMode.EndTag));
            htmlBuilder.Append(textBox.ToHtmlString());

            outerDiv.InnerHtml = htmlBuilder.ToString();
            return MvcHtmlString.Create(outerDiv.ToString(TagRenderMode.Normal));
        }

        public static IHtmlString CheckBoxForExtended<TModel>(this HtmlHelper<TModel> html, Expression<Func<TModel, bool>> property)
        {
            TagBuilder outerDiv = new TagBuilder("div");
            outerDiv.AddCssClass("custom-control custom-checkbox form-control-lg");

            MvcHtmlString checkbox = InputExtensions.CheckBoxFor(html, property, new { @class = "custom-control-input" });
            MvcHtmlString label = LabelExtensions.LabelFor(html, property, htmlAttributes: new { @class = "custom-control-label" });

            StringBuilder htmlBuilder = new StringBuilder();
            htmlBuilder.Append(checkbox.ToHtmlString());
            htmlBuilder.Append(label.ToHtmlString());

            outerDiv.InnerHtml = htmlBuilder.ToString();
            return MvcHtmlString.Create(outerDiv.ToString(TagRenderMode.Normal));
        }
    }
}