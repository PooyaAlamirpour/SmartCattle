using System;
using System.ComponentModel;
using System.Web.Mvc;
using BeyondThemes.Bootstrap.Infrastructure.Enums;
using BeyondThemes.Bootstrap.Renderers;
using BeyondThemes.Bootstrap.TypeExtensions;

namespace BeyondThemes.Bootstrap.Controls
{
    public class BootstrapFormGroupLabeled : BootstrapInputLabeled
    {
        public BootstrapFormGroupLabeled(HtmlHelper html, object inputModel, BootstrapInputType inputType)
            : base(html, inputModel, inputType)
        {
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public override string ToHtmlString()
        {
            var input = string.Empty;

            if (_inputModel.LabelWidthLg != 0)
            {
                var width = (object)_inputModel.LabelWidthLg.ToString();
                _labelModel.htmlAttributes.AddOrMergeCssClass("class", "col-lg-" + width);
            }

            if (_inputModel.LabelWidthMd != 0)
            {
                var width = (object)_inputModel.LabelWidthMd.ToString();
                _labelModel.htmlAttributes.AddOrMergeCssClass("class", "col-md-" + width);
            }

            if (_inputModel.LabelWidthSm != 0)
            {
                var width = (object)_inputModel.LabelWidthSm.ToString();
                _labelModel.htmlAttributes.AddOrMergeCssClass("class", "col-sm-" + width);
            }

            if (_inputModel.LabelWidthXs != 0)
            {
                var width = (object)_inputModel.LabelWidthXs.ToString();
                _labelModel.htmlAttributes.AddOrMergeCssClass("class", "col-xs-" + width);
            }

            switch (_inputType)
            {
                case BootstrapInputType._NotSet:
                    return null;
                case BootstrapInputType.TextBox:
                    _labelModel.htmlAttributes.AddOrMergeCssClass("class", "control-label");
                    input = Renderer.RenderFormGroupTextBox(html, _inputModel, _labelModel);
                    break;
                case BootstrapInputType.Password:
                    _labelModel.htmlAttributes.AddOrMergeCssClass("class", "control-label");
                    input = Renderer.RenderFormGroupPassword(html, _inputModel, _labelModel);
                    break;
                case BootstrapInputType.File:
                    _labelModel.htmlAttributes.AddOrMergeCssClass("class", "control-label");
                    input = Renderer.RenderFormGroupFile(html, _inputModel, _labelModel);
                    break;
                case BootstrapInputType.CheckBox:
                    _labelModel.htmlAttributes.AddOrMergeCssClass("class", "checkbox");
                    _labelModel.innerInputType = BootstrapInputType.CheckBox;
                    _labelModel.innerInputModel = _inputModel;
                    input = Renderer.RenderFormGroupCheckBox(html, _inputModel, _labelModel);
                    break;
                case BootstrapInputType.Radio:
                    _labelModel.htmlAttributes.AddOrMergeCssClass("class", "radio");
                    _labelModel.innerInputType = BootstrapInputType.Radio;
                    _labelModel.innerInputModel = _inputModel;
                    input = Renderer.RenderFormGroupRadioButton(html, _inputModel, _labelModel);
                    break;
                case BootstrapInputType.RadioTrueFalse:
                    _labelModel.htmlAttributes.AddOrMergeCssClass("class", "control-label");
                    input = Renderer.RenderFormGroupRadioButtonTrueFalse(html, _inputModel, _labelModel);
                    break;
                case BootstrapInputType.DropDownList:
                    _labelModel.htmlAttributes.AddOrMergeCssClass("class", "control-label");
                    input = Renderer.RenderFormGroupSelectElement(html, _inputModel, _labelModel, BootstrapInputType.DropDownList);
                    break;
                case BootstrapInputType.ListBox:
                    _labelModel.htmlAttributes.AddOrMergeCssClass("class", "control-label");
                    input = Renderer.RenderFormGroupSelectElement(html, _inputModel, _labelModel, BootstrapInputType.ListBox);
                    break;
                case BootstrapInputType.TextArea:
                    _labelModel.htmlAttributes.AddOrMergeCssClass("class", "control-label");
                    input = Renderer.RenderFormGroupTextArea(html, _inputModel, _labelModel);
                    break;
                case BootstrapInputType.Custom:
                    _labelModel.htmlAttributes.AddOrMergeCssClass("class", "control-label");
                    input = Renderer.RenderFormGroupCustom(html, _inputModel.input, _labelModel);
                    break;
                case BootstrapInputType.Display:
                    _labelModel.htmlAttributes.AddOrMergeCssClass("class", "control-label");
                    input = Renderer.RenderFormGroupDisplayText(html, _inputModel, _labelModel);
                    break;
            }

            
            return input;
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public override string ToString() { return ToHtmlString(); }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public override bool Equals(object obj) { return base.Equals(obj); }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public override int GetHashCode() { return base.GetHashCode(); }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public new Type GetType() { return base.GetType(); }
    }
}
