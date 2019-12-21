using System;
using System.Linq;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using BeyondThemes.Bootstrap.ControlModels;
using BeyondThemes.Bootstrap.Controls;
using BeyondThemes.Bootstrap.Infrastructure;
using BeyondThemes.Bootstrap.TypeExtensions;

namespace BeyondThemes.Bootstrap.Renderers
{
    internal static partial class Renderer
    {
        public static string RenderTextBox(HtmlHelper html, BootstrapTextBoxModel model, bool isPassword)
        {
            var combinedHtml = "{0}{1}{2}";

            model.htmlAttributes.MergeHtmlAttributes(html.GetUnobtrusiveValidationAttributes(model.htmlFieldName, model.metadata));

            model.htmlAttributes.AddOrMergeCssClass("class", "form-control");
            if (!string.IsNullOrEmpty(model.id)) model.htmlAttributes.Add("id", model.id);
            if (model.tooltipConfiguration != null) model.htmlAttributes.MergeHtmlAttributes(model.tooltipConfiguration.ToDictionary());
            if (model.tooltip != null) model.htmlAttributes.MergeHtmlAttributes(model.tooltip.ToDictionary());
            if (model.typehead != null) model.htmlAttributes.MergeHtmlAttributes(model.typehead.ToDictionary(html));
            // assign placeholder class
            if (!string.IsNullOrEmpty(model.placeholder)) model.htmlAttributes.Add("placeholder", model.placeholder);
            // assign size class
            model.htmlAttributes.AddOrMergeCssClass("class", BootstrapHelper.GetClassForInputSize(model.size));

            if (model.disabled) model.htmlAttributes.Add("disabled", "disabled");


            var widthlg = "";
            if (model.InputWidthLg != 0)
            {
                var width = model.InputWidthLg.ToString();
                widthlg = " col-lg-" + width;
            }

            var widthMd = "";
            if (model.InputWidthMd != 0)
            {
                var width = model.InputWidthMd.ToString();
                widthMd = " col-md-" + width;
            }

            var widthSm = "";
            if (model.InputWidthSm != 0)
            {
                var width = model.InputWidthSm.ToString();
                widthSm = " col-sm-" + width;
            }

            var widthXs = "";
            if (model.InputWidthXs != 0)
            {
                var width = model.InputWidthXs.ToString();
                widthXs = " col-xs-" + width;
            }

            var offsetlg = "";
            if (model.InputOffsetLg != 0)
            {
                var offset = model.InputOffsetLg.ToString();
                offsetlg = " col-lg-offset-" + offset;
            }

            var offsetMd = "";
            if (model.InputOffsetMd != 0)
            {
                var offset = model.InputOffsetMd.ToString();
                offsetMd = " col-md-offset-" + offset;
            }

            var offsetSm = "";
            if (model.InputOffsetSm != 0)
            {
                var offset = model.InputOffsetSm.ToString();
                offsetSm = " col-sm-offset-" + offset;
            }

            var offsetXs = "";
            if (model.InputOffsetXs != 0)
            {
                var offset = model.InputOffsetXs.ToString();
                offsetXs = " col-xs-offset-" + offset;
            }

            var widthoffset = widthlg + widthMd + widthSm + widthXs + offsetlg + offsetMd + offsetSm + offsetXs;
            // account for appendString, prependString, and AppendButtons
            if (!string.IsNullOrEmpty(model.prependString) ||
                !string.IsNullOrEmpty(model.appendString) ||
                model.prependButtons.Any() ||
                model.appendButtons.Any() ||
                !string.IsNullOrEmpty(model.iconPrepend) ||
                !string.IsNullOrEmpty(model.iconAppend) ||
                !string.IsNullOrEmpty(model.iconPrependCustomClass) ||
                !string.IsNullOrEmpty(model.iconAppendCustomClass) ||
                !string.IsNullOrEmpty(widthoffset))
            {
                var appendPrependContainer = new TagBuilder("div");
                var addOnPrependString = "";
                var addOnAppendString = "";
                var addOnPrependButtons = "";
                var addOnAppendButtons = "";
                var addOnPrependIcon = "";
                var addOnAppendIcon = "";

                var addOn = new TagBuilder("span");
                addOn.AddCssClass("input-group-addon");
                if (!string.IsNullOrEmpty(model.prependString))
                {
                    appendPrependContainer.AddOrMergeCssClass("input-group");
                    addOn.InnerHtml = model.prependString;
                    addOnPrependString = addOn.ToString();
                }
                if (!string.IsNullOrEmpty(model.appendString))
                {
                    appendPrependContainer.AddOrMergeCssClass("input-group");
                    addOn.InnerHtml = model.appendString;
                    addOnAppendString = addOn.ToString();
                }
                if (model.prependButtons.Count > 0)
                {
                    appendPrependContainer.AddOrMergeCssClass("input-group");
                    var span = new TagBuilder("span");
                    span.AddOrMergeCssClass("input-group-btn");
                    model.prependButtons.ForEach(x => addOnPrependButtons += x.ToHtmlString());
                    span.InnerHtml = addOnPrependButtons;
                    addOnPrependButtons = span.ToString();
                }
                if (model.appendButtons.Count > 0)
                {
                    appendPrependContainer.AddOrMergeCssClass("input-group");
                    var span = new TagBuilder("span");
                    span.AddOrMergeCssClass("input-group-btn");
                    model.appendButtons.ForEach(x => addOnAppendButtons += x.ToHtmlString());
                    span.InnerHtml = addOnAppendButtons;
                    addOnAppendButtons = span.ToString();
                }
                if (!string.IsNullOrEmpty(model.iconPrepend))
                {
                    appendPrependContainer.AddOrMergeCssClass("input-icon icon-left");
                    addOnPrependIcon = new BootstrapIcon(model.iconPrepend, model.iconPrependIsWhite).HtmlAttributes(new { @class = model.prependIconStyle }).ToHtmlString();
                }
                if (!string.IsNullOrEmpty(model.iconAppend))
                {
                    appendPrependContainer.AddOrMergeCssClass("input-icon icon-right");
                    addOnAppendIcon = new BootstrapIcon(model.iconAppend, model.iconAppendIsWhite).HtmlAttributes(new { @class = model.appendIconStyle }).ToHtmlString();
                }
                if (!string.IsNullOrEmpty(model.iconPrependCustomClass))
                {
                    appendPrependContainer.AddOrMergeCssClass("input-prepend");
                    var i = new TagBuilder("i");
                    i.AddCssClass(model.iconPrependCustomClass);
                    addOn.InnerHtml = i.ToString(TagRenderMode.Normal);
                    addOnPrependIcon = addOn.ToString();
                }
                if (!string.IsNullOrEmpty(model.iconAppendCustomClass))
                {
                    appendPrependContainer.AddOrMergeCssClass("input-append");
                    var i = new TagBuilder("i");
                    i.AddCssClass(model.iconAppendCustomClass);
                    addOn.InnerHtml = i.ToString(TagRenderMode.Normal);
                    addOnAppendIcon = addOn.ToString();
                }

                appendPrependContainer.AddOrMergeCssClass(widthlg + widthMd + widthSm + widthXs);
                appendPrependContainer.AddOrMergeCssClass(offsetlg + offsetMd + offsetSm + offsetXs);

                switch (model.size)
                {
                    case InputSize.XSmall:
                        appendPrependContainer.AddOrMergeCssClass("input-group-xs");
                        break;
                    case InputSize.Small:
                        appendPrependContainer.AddOrMergeCssClass("input-group-sm");
                        break;
                    case InputSize.Large:
                        appendPrependContainer.AddOrMergeCssClass("input-group-lg");
                        break;
                    case InputSize.XLarge:
                        appendPrependContainer.AddOrMergeCssClass("input-group-xl");
                        break;
                }


                appendPrependContainer.InnerHtml = addOnPrependButtons + addOnPrependString + "{0}" + addOnPrependIcon + addOnAppendString + addOnAppendIcon + addOnAppendButtons + "{1}";
                combinedHtml = appendPrependContainer.ToString(TagRenderMode.Normal) + "{2}";
            }

            // build html for input
            var input = isPassword
                    ? html.Password(model.htmlFieldName, null, model.htmlAttributes.FormatHtmlAttributes()).ToHtmlString()
                    : html.TextBox(model.htmlFieldName, model.value, model.htmlAttributes.FormatHtmlAttributes()).ToHtmlString();


            var helpText = model.helpText != null ? model.helpText.ToHtmlString() : string.Empty;
            var validationMessage = "";
            if (model.displayValidationMessage)
            {
                var validation = html.ValidationMessage(model.htmlFieldName).ToHtmlString();
                validationMessage = new BootstrapHelpText(validation, model.validationMessageStyle).ToHtmlString();
            }

            return MvcHtmlString.Create(string.Format(combinedHtml, input, helpText, validationMessage)).ToString();
        }
    }
}
