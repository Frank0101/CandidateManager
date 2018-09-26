using CandidateManager.Web.Attributes;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;

namespace CandidateManager.Web.Extensions
{
    public static class HtmlHelperExtensions
    {
        public static HtmlString RenderCandidateManagerField<TModel, TField>(
            this HtmlHelper<TModel> html, Expression<Func<TModel, TField>> expression,
            IDictionary<string, object> htmlAttributes = null)
        {
            var outerDiv = new TagBuilder("div");
            outerDiv.AddCssClass("form-group");

            var label = html.LabelFor(expression, new { @class = "control-label col-md-2" });

            var innerDiv = new TagBuilder("div");
            innerDiv.AddCssClass("col-md-10");

            htmlAttributes = htmlAttributes ?? new Dictionary<string, object> { { "class", "form-control" } };
            if (HasAttribute(expression, typeof(ReadOnly)))
            {
                htmlAttributes.Add("readonly", "readonly");
            }

            var editor = html.EditorFor(expression, new
            {
                htmlAttributes
            });
            var validation = html.ValidationMessageFor(expression, "", new { @class = "text-danger" });

            innerDiv.InnerHtml = editor.ToString() + validation.ToString();
            outerDiv.InnerHtml = label.ToString() + innerDiv.ToString();
            return new HtmlString(outerDiv.ToString());
        }

        private static bool HasAttribute<TModel, TField>(Expression<Func<TModel, TField>> expression,
            Type attributeType)
        {
            var memberExpression = expression.Body as MemberExpression;
            if (memberExpression != null)
            {
                var propertyInfo = memberExpression.Member as PropertyInfo;
                if (propertyInfo != null)
                {
                    if (Attribute.IsDefined(propertyInfo, attributeType))
                    {
                        return true;
                    }
                }
            }
            return false;
        }
    }
}
