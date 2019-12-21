using System.Web.Mvc;
using System.Web.Mvc.Html;
using BeyondThemes.Bootstrap.ControlModels;
using BeyondThemes.Bootstrap.Controls;
using BeyondThemes.Bootstrap.Infrastructure.Enums;

namespace BeyondThemes.Bootstrap.Renderers
{
    internal static partial class Renderer
    {
        public static string RenderFormGroupRadioButton(HtmlHelper html, BootstrapRadioButtonModel inputModel, BootstrapLabelModel labelModel)
        {
            string validationMessage = "";
            if (inputModel.displayValidationMessage)
            {
                string validation = html.ValidationMessage(inputModel.htmlFieldName).ToHtmlString();
                validationMessage = new BootstrapHelpText(validation, inputModel.validationMessageStyle).ToHtmlString();
            }

            string label = Renderer.RenderLabel(html, labelModel ?? new BootstrapLabelModel
            {
                htmlFieldName = inputModel.htmlFieldName,
                metadata = inputModel.metadata,
                innerInputType = BootstrapInputType.Radio,
                innerInputModel = inputModel,
                innerValidationMessage = validationMessage
            });

            bool fieldIsValid = true;
            if(inputModel != null) fieldIsValid = html.ViewData.ModelState.IsValidField(inputModel.htmlFieldName);
            return new BootstrapFormGroup(null, label, FormGroupType.checkboxLike, fieldIsValid).ToHtmlString();
        }
    }
}
