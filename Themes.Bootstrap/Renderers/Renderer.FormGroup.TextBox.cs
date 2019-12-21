using System.Web.Mvc;
using BeyondThemes.Bootstrap.ControlModels;
using BeyondThemes.Bootstrap.Controls;
using BeyondThemes.Bootstrap.TypeExtensions;

namespace BeyondThemes.Bootstrap.Renderers
{
    internal static partial class Renderer
    {
        public static string RenderFormGroupTextBox(HtmlHelper html, BootstrapTextBoxModel inputModel, BootstrapLabelModel labelModel)
        {
            var input = Renderer.RenderTextBox(html, inputModel, false);
            var widthlg = "";
            if (inputModel.LabelWidthLg != 0)
            {
                var width = inputModel.LabelWidthLg.ToString();
                widthlg =  " col-lg-" + width;
            }

            var widthMd = "";
            if (inputModel.LabelWidthMd != 0)
            {
                var width = inputModel.LabelWidthMd.ToString();
                widthMd = " col-md-" + width;
            }

            var widthSm = "";
            if (inputModel.LabelWidthSm != 0)
            {
                var width = inputModel.LabelWidthSm.ToString();
                widthSm = " col-sm-" + width;
            }

            var widthXs = "";
            if (inputModel.LabelWidthXs != 0)
            {
                var width = inputModel.LabelWidthXs.ToString();
                widthXs = " col-xs-" + width;
            }

            var offsetlg = "";
            if (inputModel.LabelOffsetLg != 0)
            {
                var offset = inputModel.LabelOffsetLg.ToString();
                offsetlg = " col-lg-offset-" + offset;
            }

            var offsetMd = "";
            if (inputModel.LabelOffsetMd != 0)
            {
                var offset = inputModel.LabelOffsetMd.ToString();
                offsetMd = " col-md-offset-" + offset;
            }

            var offsetSm = "";
            if (inputModel.LabelOffsetSm != 0)
            {
                var offset = inputModel.LabelOffsetSm.ToString();
                offsetSm = " col-sm-offset-" + offset;
            }

            var offsetXs = "";
            if (inputModel.LabelOffsetXs != 0)
            {
                var offset = inputModel.LabelOffsetXs.ToString();
                offsetXs = " col-xs-offset-" + offset;
            }

            string label = Renderer.RenderLabel(html, labelModel ?? new BootstrapLabelModel
            {
                htmlFieldName = inputModel.htmlFieldName,
                metadata = inputModel.metadata,
                htmlAttributes = new { @class = "control-label" + widthlg + widthMd + widthSm + widthXs + offsetlg + offsetMd + offsetSm + offsetXs }.ToDictionary()
            });

            

            bool fieldIsValid = true;
            if(inputModel != null) fieldIsValid = html.ViewData.ModelState.IsValidField(inputModel.htmlFieldName);
            return new BootstrapFormGroup(input, label, FormGroupType.textboxLike, fieldIsValid).ToHtmlString();
        }
    }
}
