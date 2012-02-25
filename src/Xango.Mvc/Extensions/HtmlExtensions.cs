using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Web.Mvc;
using System.Web.Mvc.Html;

namespace Xango.Mvc.Extensions
{
    public static class HtmlExtensions
    {
        public static MvcHtmlString MessageDiv(this HtmlHelper htmlHelper)
        {
            if (htmlHelper.ViewContext.TempData.ContainsKey("Success"))
                return new MvcHtmlString(GenerateDiv((string)htmlHelper.ViewContext.TempData["Success"], "success"));

            return htmlHelper.ViewContext.TempData.ContainsKey("Error")
                ? new MvcHtmlString(GenerateDiv((string)htmlHelper.ViewContext.TempData["Error"], "error")) 
                : new MvcHtmlString("");
        }

        public static string Cycle(this HtmlHelper html, params string[] options)
        {
            var last = ""+html.ViewContext.TempData["_lastCycle"];

            last = options.FirstOrDefault(c => c != last);

            if (string.IsNullOrWhiteSpace(last))
                last = options.First();

            html.ViewContext.TempData["_lastCycle"] = last;
            return last;
        }

        public static MvcHtmlString EditLinkButtonForGrid(this HtmlHelper html,
            string editUrl, 
            UrlHelper urlHelper, 
            string editImage)
        {
            return html.LinkButton(new Dictionary<string, string>
                                       {
                                           {"href", editUrl}
                                       },
                                       new Dictionary<string, string>
                                       {
                                           {"src", urlHelper.Image(editImage)}
                                       });
        }

        public static MvcHtmlString DeleteEditLinkButtonForGrid(this HtmlHelper html,
           string deleteUrl,
           string returnUrl,
           UrlHelper urlHelper,
           string message,
           string deleteImage)
        {
            return html.LinkButton(new Dictionary<string, string>
                                       {
                                           {"class", "delete"},
                                           {"href", deleteUrl},
                                           {"data-return", returnUrl},
                                           {"data-message", message}
                                       },
                                       new Dictionary<string, string>
                                       {
                                           {"src", urlHelper.Image(deleteImage)}
                                       });
        }

        public static MvcHtmlString LinkButton(this HtmlHelper html,
            IDictionary<string, string> linkHtmlAttributes = null,
            IDictionary<string, string> imageHtmlAttributes = null)
        {
            var link = new TagBuilder("a");
            if(null != linkHtmlAttributes)
                link.MergeAttributes(linkHtmlAttributes);

            var image = new TagBuilder("img");
            if (null != imageHtmlAttributes)
                image.MergeAttributes(imageHtmlAttributes);

            link.InnerHtml = image.ToString(TagRenderMode.SelfClosing);

            return new MvcHtmlString(link.ToString(TagRenderMode.Normal));
        }

        public static MvcHtmlString DropDownListForEnum<TModel, TProperty>(this HtmlHelper<TModel> html,
            string name,
            TProperty obj,
            string optionLabel = null,
            IDictionary<string,object> htmlAttributes = null)
        {
            if (!typeof(TProperty).IsEnum)
                throw new ArgumentException("TProperty must be an enumerated type");

            var values = Enum
                .GetValues(typeof(TProperty))
                .Cast<Enum>()
                .Select(c => new SelectListItem
                {
                    Text = c.GetDescription(),
                    Value = c.ToString(),
                    Selected = c.ToString() == obj.ToString()
                });

            return html.DropDownList(name,
                values,
                optionLabel,
                htmlAttributes);
        }

        public static MvcHtmlString DropDownListForEnum<TModel, TProperty>(this HtmlHelper<TModel> html,
            Expression<Func<TModel, TProperty>> expression,
            string optionLabel = null,
            IDictionary<string,object> htmlAttributes = null) where TProperty: struct, IConvertible
        {
            if (!typeof(TProperty).IsEnum)
                throw new ArgumentException("TProperty must be an enumerated type");

            var values = Enum
                .GetValues(typeof (TProperty))
                .Cast<Enum>()
                .Select(c => new SelectListItem
                                 {
                                     Text = c.GetDescription(), 
                                     Value = c.ToString()
                                 });

            return html.DropDownListFor(expression,
                values,
                optionLabel,
                htmlAttributes);
        }

        public static MvcHtmlString CheckboxListForEnum<TModel, TProperty>(this HtmlHelper<TModel> html,
           Expression<Func<TModel, TProperty>> expression, 
            IDictionary<string, object> htmlAttributes = null) where TProperty : struct, IConvertible
        {
            if (!typeof (TProperty).IsEnum)
                throw new ArgumentException("TProperty must be an enumerated type");

            TProperty value = expression.Compile()((TModel) html.ViewContext.ViewData.Model);


            var enumValue = (Enum)Enum.Parse(typeof(TProperty), value.ToString());
            

            var itens = Enum
                .GetValues(typeof (TProperty))
                .Cast<Enum>()
                .Select(c => new SelectListItem
                                 {
                                     Text = c.GetDescription(),
                                     Value = c.ToString(),
                                     Selected = null != enumValue && enumValue.HasFlag(c)
                                 });

            var name = ExpressionHelper.GetExpressionText(expression);

            var sb = new StringBuilder();
            var ul = new TagBuilder("ul");

            ul.MergeAttributes(htmlAttributes);

            foreach (var item in itens)
            {
                var id = string.Format("{0}_{1}", name, item.Value);

                var li = new TagBuilder("li");

                var checkBox = new TagBuilder("input");
                checkBox.Attributes.Add("id", id);
                checkBox.Attributes.Add("value", item.Value);
                checkBox.Attributes.Add("name", name);
                checkBox.Attributes.Add("type", "checkbox");
                if(item.Selected)
                    checkBox.Attributes.Add("checked", "checked");

                var label = new TagBuilder("label");
                label.Attributes.Add("for", id);

                label.SetInnerText(item.Text);

                li.InnerHtml = checkBox.ToString(TagRenderMode.SelfClosing) + "\r\n" +
                               label.ToString(TagRenderMode.Normal);

                sb.AppendLine(li.ToString(TagRenderMode.Normal));
            }

            ul.InnerHtml = sb.ToString();

            return new MvcHtmlString(ul.ToString(TagRenderMode.Normal));
        }

        private static string GenerateDiv(string message, string @class)
        {
            var builder = new TagBuilder("div");
            builder.AddCssClass(@class);
            builder.SetInnerText(message);
            return builder.ToString(TagRenderMode.Normal);
        }
    }
}