﻿using System.Web.Mvc;
using System.Web.Mvc.Html;
using BeyondThemes.Bootstrap.ControlModels;
using BeyondThemes.Bootstrap.Controls;
using BeyondThemes.Bootstrap.TypeExtensions;

namespace BeyondThemes.Bootstrap.Renderers
{
    internal static partial class Renderer
    {
        public static string RenderCheckBox(HtmlHelper html, BootstrapCheckBoxModel model)
        {
            model.htmlAttributes.MergeHtmlAttributes(html.GetUnobtrusiveValidationAttributes(model.htmlFieldName, model.metadata));
            if (model.tooltipConfiguration != null) model.htmlAttributes.MergeHtmlAttributes(model.tooltipConfiguration.ToDictionary());
            if (model.tooltip != null) model.htmlAttributes.MergeHtmlAttributes(model.tooltip.ToDictionary());
            var mergedHtmlAttrs = string.IsNullOrEmpty(model.id) ? model.htmlAttributes : model.htmlAttributes.AddOrReplace("id", model.id);

            string validationMessage = "";
            if (model.displayValidationMessage)
            {
                string validation = html.ValidationMessage(model.htmlFieldName).ToHtmlString();
                validationMessage = new BootstrapHelpText(validation, model.validationMessageStyle).ToHtmlString();
            }
            return html.CheckBox(model.htmlFieldName, model.isChecked, mergedHtmlAttrs.FormatHtmlAttributes()).ToHtmlString() + validationMessage;
        }
    }
}
