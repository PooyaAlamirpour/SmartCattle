using System.Linq;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using BeyondThemes.Bootstrap.ControlModels;
using BeyondThemes.Bootstrap.TypeExtensions;

namespace BeyondThemes.Bootstrap.Renderers
{
    internal static partial class Renderer
    {
        public static string RenderCheckBoxCustom(HtmlHelper html, BootstrapCheckBoxModel model)
        {
            var fullHtmlFieldName = html.ViewContext.ViewData.TemplateInfo.GetFullHtmlFieldName(model.htmlFieldName);

            model.htmlAttributes.MergeHtmlAttributes(html.GetUnobtrusiveValidationAttributes(model.htmlFieldName, model.metadata));
            if (model.tooltipConfiguration != null) model.htmlAttributes.MergeHtmlAttributes(model.tooltipConfiguration.ToDictionary());
            if (model.tooltip != null) model.htmlAttributes.MergeHtmlAttributes(model.tooltip.ToDictionary());

            ModelState modelState;
            if (html.ViewData.ModelState.TryGetValue(fullHtmlFieldName, out modelState))
            {
                if (modelState.Errors.Count > 0) model.htmlAttributes.AddOrMergeCssClass("class", "input-validation-error");
                if (modelState.Value != null && ((string[])modelState.Value.RawValue).Contains("True")) model.isChecked = true;
            }

            var checkBoxDiv = new TagBuilder("div");
            checkBoxDiv.MergeAttribute("class", "checkbox");

            var label = new TagBuilder("label");

            var input = html.CheckBox(model.htmlFieldName, model.isChecked, model.htmlAttributes).ToString();

            var span = new TagBuilder("span");
            span.MergeAttribute("class", "text");
            span.InnerHtml = !string.IsNullOrEmpty(model.metadata.DisplayName) ? model.metadata.DisplayName : model.text;

            label.InnerHtml = input + span.ToString();
            checkBoxDiv.InnerHtml = label.ToString();

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

            checkBoxDiv.AddOrMergeCssClass(widthlg + widthMd + widthSm + widthXs);
            checkBoxDiv.AddOrMergeCssClass(offsetlg + offsetMd + offsetSm + offsetXs);


            return checkBoxDiv.ToString();
        }
    }
}
