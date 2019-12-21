using System;
using System.Collections.Generic;
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
    public class BootstrapTextBox : BootstrapTextBoxBase<BootstrapTextBox>
    {
        public BootstrapTextBox(HtmlHelper html, string htmlFieldName, ModelMetadata metadata)
            : base(html, htmlFieldName, metadata)
        {

        }
    }

    public class BootstrapTextBoxBase<T> : IBootstrapTextBox<T>
        where T : BootstrapTextBoxBase<T>
    {
        protected HtmlHelper html;
        protected BootstrapTextBoxModel _model = new BootstrapTextBoxModel();

        public BootstrapTextBoxBase(HtmlHelper html, string htmlFieldName, ModelMetadata metadata)
        {
            this.html = html;
            this._model.htmlFieldName = htmlFieldName;
            this._model.metadata = metadata;
            //this._model.value = metadata.Model;
        }

        public T Id(string id)
        {
            this._model.id = id;
            return (T)this;
        }

        public T Value(object value)
        {
            this._model.value = value;
            return (T)this;
        }

        public T HtmlAttributes(IDictionary<string, object> htmlAttributes)
        {
            this._model.htmlAttributes = htmlAttributes;
            return (T)this;
        }

        public T HtmlAttributes(object htmlAttributes)
        {
            this._model.htmlAttributes = htmlAttributes.ToDictionary();
            return (T)this;
        }

        public T Format(string format)
        {
            this._model.format = format;
            return (T)this;
        }

        public T Placeholder()
        {
            this._model.placeholder = BootstrapHelper.GetPlaceholderFromMetadata(_model.metadata);
            return (T)this;
        }

        public T Placeholder(string placeholder)
        {
            this._model.placeholder = placeholder;
            return (T)this;
        }

        public T Prepend(string prependString)
        {
            this._model.prependString = prependString;
            return (T)this;
        }

        public T Prepend(BootstrapButton button)
        {
            this._model.prependButtons.Add(button);
            return (T)this;
        }

        public T PrependIcon(string icon)
        {
            this._model.iconPrepend = icon;
            return (T)this;
        }

        public T PrependIcon(string icon, bool isWhite)
        {
            this._model.iconPrepend = icon;
            this._model.iconPrependIsWhite = isWhite;
            return (T)this;
        }

        public T Append(string appendString)
        {
            this._model.appendString = appendString;
            return (T)this;
        }

        public T Append(BootstrapButton button)
        {
            this._model.appendButtons.Add(button);
            return (T)this;
        }

        public T AppendIcon(string icon)
        {
            this._model.iconAppend = icon;
            return (T)this;
        }

        public T AppendIconStyle(string style)
        {
            this._model.appendIconStyle = style;
            return (T)this;
        }

        public T PrependIconStyle(string style)
        {
            this._model.prependIconStyle = style;
            return (T)this;
        }

        public T AppendIcon(string icon, bool isWhite)
        {
            this._model.iconAppend = icon;
            this._model.iconAppendIsWhite = isWhite;
            return (T)this;
        }

        public T HelpText()
        {
            this._model.helpText = new BootstrapHelpText(BootstrapHelper.GetHelpTextFromMetadata(_model.metadata), HelpTextStyle.Inline);
            return (T)this;
        }

        public T HelpText(string text)
        {
            this._model.helpText = new BootstrapHelpText(text, HelpTextStyle.Inline);
            return (T)this;
        }

        public T HelpText(string text, HelpTextStyle style)
        {
            this._model.helpText = new BootstrapHelpText(text, style);
            return (T)this;
        }

        public T Size(InputSize size)
        {
            this._model.size = size;
            return (T)this;
        }

        public T Disable()
        {
            this._model.disabled = true;
            return (T)this;
        }

        public T Tooltip(Tooltip tooltip)
        {
            this._model.tooltip = tooltip;
            return (T)this;
        }

        [Obsolete("This overload is deprecated and will be removed in the future versions. Use .Tooltip(Tooltip tooltip) instead.")]
        public T Tooltip(TooltipConfiguration configuration)
        {
            this._model.tooltipConfiguration = configuration;
            return (T)this;
        }

        public T Tooltip(string title)
        {
            this._model.tooltip = new Tooltip(title);
            return (T)this;
        }

        public T ShowValidationMessage(bool displayValidationMessage)
        {
            this._model.displayValidationMessage = displayValidationMessage;
            if (displayValidationMessage) this._model.validationMessageStyle = HelpTextStyle.Inline;
            return (T)this;
        }

        public T ShowValidationMessage(bool displayValidationMessage, HelpTextStyle validationMessageStyle)
        {
            this._model.displayValidationMessage = displayValidationMessage;
            this._model.validationMessageStyle = validationMessageStyle;
            return (T)this;
        }

        public T Typehead(Typehead typehead)
        {
            _model.typehead = typehead;
            return (T)this;
        }

        public virtual IBootstrapLabel Label()
        {
            IBootstrapLabel l = new BootstrapInputLabeled(html, _model, BootstrapInputType.TextBox);
            return l;
        }

        public T LabelWidthLg(int width)
        {
            this._model.LabelWidthLg = width;
            return (T)this;
        }
        public T LabelWidthMd(int width)
        {
            this._model.LabelWidthMd = width;
            return (T)this;
        }
        public T LabelWidthSm(int width)
        {
            this._model.LabelWidthSm = width;
            return (T)this;
        }
        public T LabelWidthXs(int width)
        {
            this._model.LabelWidthXs = width;
            return (T)this;
        }

        public T InputWidthLg(int width)
        {
            this._model.InputWidthLg = width;
            return (T)this;
        }
        public T InputWidthMd(int width)
        {
            this._model.InputWidthMd = width;
            return (T)this;
        }
        public T InputWidthSm(int width)
        {
            this._model.InputWidthSm = width;
            return (T)this;
        }
        public T InputWidthXs(int width)
        {
            this._model.InputWidthXs = width;
            return (T)this;
        }

        public T LabelOffsetLg(int offset)
        {
            this._model.LabelOffsetLg = offset;
            return (T)this;
        }
        public T LabelOffsetMd(int offset)
        {
            this._model.LabelOffsetMd = offset;
            return (T)this;
        }
        public T LabelOffsetSm(int offset)
        {
            this._model.LabelOffsetSm = offset;
            return (T)this;
        }
        public T LabelOffsetXs(int offset)
        {
            this._model.LabelOffsetXs = offset;
            return (T)this;
        }

        public T InputOffsetLg(int offset)
        {
            this._model.InputOffsetLg = offset;
            return (T)this;
        }
        public T InputOffsetMd(int offset)
        {
            this._model.InputOffsetMd = offset;
            return (T)this;
        }
        public T InputOffsetSm(int offset)
        {
            this._model.InputOffsetSm = offset;
            return (T)this;
        }
        public T InputOffsetXs(int offset)
        {
            this._model.InputOffsetXs = offset;
            return (T)this;
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public virtual string ToHtmlString()
        {
            return Renderer.RenderTextBox(html, _model, false);
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
