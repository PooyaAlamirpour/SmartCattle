﻿using System;
using System.ComponentModel;
using System.Web.Mvc;
using BeyondThemes.Bootstrap.ControlInterfaces;
using BeyondThemes.Bootstrap.ControlModels;
using BeyondThemes.Bootstrap.Infrastructure;
using BeyondThemes.Bootstrap.Infrastructure.Enums;
using BeyondThemes.Bootstrap.Renderers;
using BeyondThemes.Bootstrap.TypeExtensions;

namespace BeyondThemes.Bootstrap.Controls
{
    public class BootstrapRadioButtonTrueFalse : IBootstrapRadioButtonTrueFalse
    {
        protected HtmlHelper html;
        protected BootstrapRadioButtonTrueFalseModel _model = new BootstrapRadioButtonTrueFalseModel();

        public BootstrapRadioButtonTrueFalse(HtmlHelper html, string htmlFieldName, ModelMetadata metadata)
        {
            this.html = html;
            this._model.htmlFieldName = htmlFieldName;
            this._model.metadata = metadata;
        }

        public IBootstrapRadioButtonTrueFalse InputsValues(object trueValue, object falseValue)
        {
            this._model.inputTrueValue = trueValue;
            this._model.inputFalseValue = falseValue;
            return this;
        }

        public IBootstrapRadioButtonTrueFalse HtmlAttributesInputTrue(object htmlAttributes)
        {
            this._model.htmlAttributesInputTrue = htmlAttributes.ToDictionary();
            return this;
        }

        public IBootstrapRadioButtonTrueFalse HtmlAttributesInputFalse(object htmlAttributes)
        {
            this._model.htmlAttributesInputFalse = htmlAttributes.ToDictionary();
            return this;
        }

        public IBootstrapRadioButtonTrueFalse HtmlAttributesLabelTrue(object htmlAttributes)
        {
            this._model.htmlAttributesLabelTrue = htmlAttributes.ToDictionary();
            return this;
        }

        public IBootstrapRadioButtonTrueFalse HtmlAttributesLabelFalse(object htmlAttributes)
        {
            this._model.htmlAttributesLabelFalse = htmlAttributes.ToDictionary();
            return this;
        }

        public IBootstrapRadioButtonTrueFalse InputLabelsText(string trueText, string falseText)
        {
            this._model.labelTrueText = trueText;
            this._model.labelFalseText = falseText;
            return this;
        }

        [Obsolete("This overload is deprecated and will be removed in the future versions. Use .Tooltip(Tooltip tooltip) instead.")]
        public IBootstrapRadioButtonTrueFalse Tooltip(TooltipConfiguration configuration)
        {
            this._model.tooltipConfiguration = configuration;
            return this;
        }

        public IBootstrapRadioButtonTrueFalse Tooltip(Tooltip tooltip)
        {
            this._model.tooltip = tooltip;
            return this;
        }

        public IBootstrapRadioButtonTrueFalse Tooltip(string title)
        {
            this._model.tooltip = new Tooltip(title);
            return this;
        }

        public IBootstrapRadioButtonTrueFalse HelpText()
        {
            this._model.helpText = new BootstrapHelpText(BootstrapHelper.GetHelpTextFromMetadata(_model.metadata), HelpTextStyle.Inline);
            return this;
        }

        public IBootstrapRadioButtonTrueFalse HelpText(string text)
        {
            this._model.helpText = new BootstrapHelpText(text, HelpTextStyle.Inline);
            return this;
        }

        public IBootstrapRadioButtonTrueFalse HelpText(string text, HelpTextStyle style)
        {
            this._model.helpText = new BootstrapHelpText(text, style);
            return this;
        }

        public IBootstrapRadioButtonTrueFalse ShowValidationMessage(bool displayValidationMessage)
        {
            _model.displayValidationMessage = displayValidationMessage;
            if (displayValidationMessage) _model.validationMessageStyle = HelpTextStyle.Inline;
            return this;
        }

        public IBootstrapRadioButtonTrueFalse ShowValidationMessage(bool displayValidationMessage, HelpTextStyle validationMessageStyle)
        {
            _model.displayValidationMessage = displayValidationMessage;
            _model.validationMessageStyle = validationMessageStyle;
            return this;
        }

        public virtual IBootstrapLabel Label()
        {
            IBootstrapLabel l = new BootstrapInputLabeled(html, _model, BootstrapInputType.RadioTrueFalse);
            return l;
        }

        public virtual string ToHtmlString()
        {
            return Renderer.RenderRadioButtonTrueFalse(html, _model);
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public override string ToString() { return null; }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public override bool Equals(object obj) { return base.Equals(obj); }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public override int GetHashCode() { return base.GetHashCode(); }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public new Type GetType() { return base.GetType(); }
    }
}
