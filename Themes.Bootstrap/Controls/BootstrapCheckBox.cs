using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Web.Mvc;
using BeyondThemes.Bootstrap.ControlInterfaces;
using BeyondThemes.Bootstrap.ControlModels;
using BeyondThemes.Bootstrap.Infrastructure.Enums;
using BeyondThemes.Bootstrap.Renderers;
using BeyondThemes.Bootstrap.TypeExtensions;

namespace BeyondThemes.Bootstrap.Controls
{
    public class BootstrapCheckBox : IBootstrapCheckBox
    {
        protected HtmlHelper html;
        protected BootstrapCheckBoxModel _model = new BootstrapCheckBoxModel();

        public BootstrapCheckBox(HtmlHelper html, string htmlFieldName, ModelMetadata metadata)
        {
            this.html = html;
            this._model.htmlFieldName = htmlFieldName;
            this._model.metadata = metadata;
            this._model.isChecked = (metadata.Model != null) && bool.Parse(metadata.Model.ToString());
        }

        public BootstrapCheckBox(HtmlHelper html, string htmlFieldName, ModelMetadata metadata, object value, bool isSingleInput)
        {
            this.html = html;
            this._model.htmlFieldName = htmlFieldName;
            this._model.metadata = metadata;
            this._model.isChecked = (metadata.Model != null) && bool.Parse(metadata.Model.ToString());
            this._model.value = value;
            this._model.isSingleInput = isSingleInput;
        }

        public IBootstrapCheckBox Id(string id)
        {
            this._model.id = id;
            return this;
        }

        public IBootstrapCheckBox IsChecked(bool isChecked)
        {
            this._model.isChecked = isChecked;
            return this;
        }

        public IBootstrapCheckBox Text(string text)
        {
            this._model.text = text;
            return this;
        }

        public IBootstrapCheckBox HtmlAttributes(IDictionary<string, object> htmlAttributes)
        {
            this._model.htmlAttributes = htmlAttributes;
            return this;
        }

        public IBootstrapCheckBox HtmlAttributes(object htmlAttributes)
        {
            this._model.htmlAttributes = htmlAttributes.ToDictionary();
            return this;
        }

        [Obsolete("This overload is deprecated and will be removed in the future versions. Use .Tooltip(Tooltip tooltip) instead.")]
        public IBootstrapCheckBox Tooltip(TooltipConfiguration configuration)
        {
            this._model.tooltipConfiguration = configuration;
            return this;
        }

        public IBootstrapCheckBox Tooltip(Tooltip tooltip)
        {
            this._model.tooltip = tooltip;
            return this;
        }

        public IBootstrapCheckBox Tooltip(string title)
        {
            this._model.tooltip = new Tooltip(title);
            return this;
        }

        public IBootstrapCheckBox ShowValidationMessage(bool displayValidationMessage)
        {
            _model.displayValidationMessage = displayValidationMessage;
            if (displayValidationMessage) _model.validationMessageStyle = HelpTextStyle.Inline;
            return this;
        }

        public IBootstrapCheckBox ShowValidationMessage(bool displayValidationMessage, HelpTextStyle validationMessageStyle)
        {
            _model.displayValidationMessage = displayValidationMessage;
            _model.validationMessageStyle = validationMessageStyle;
            return this;
        }

        public virtual IBootstrapLabel Label()
        {
            IBootstrapLabel l = new BootstrapInputLabeled(html, _model, BootstrapInputType.CheckBox);
            return l;
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public virtual string ToHtmlString()
        {
            return Renderer.RenderCheckBoxCustom(html, _model);
            //return (_model.isSingleInput)
            //    ? Renderer.RenderCheckBoxCustom(html, _model)
            //    : Renderer.RenderCheckBox(html, _model);
        }

        public BootstrapFormGroupCheckBox LabelWidthLg(int width)
        {
            this._model.LabelWidthLg = width;
            return (BootstrapFormGroupCheckBox)this;
        }
        public BootstrapFormGroupCheckBox LabelWidthMd(int width)
        {
            this._model.LabelWidthMd = width;
            return (BootstrapFormGroupCheckBox)this;
        }
        public BootstrapFormGroupCheckBox LabelWidthSm(int width)
        {
            this._model.LabelWidthSm = width;
            return (BootstrapFormGroupCheckBox)this;
        }
        public BootstrapFormGroupCheckBox LabelWidthXs(int width)
        {
            this._model.LabelWidthXs = width;
            return (BootstrapFormGroupCheckBox)this;
        }

        public BootstrapFormGroupCheckBox InputWidthLg(int width)
        {
            this._model.InputWidthLg = width;
            return (BootstrapFormGroupCheckBox)this;
        }
        public BootstrapFormGroupCheckBox InputWidthMd(int width)
        {
            this._model.InputWidthMd = width;
            return (BootstrapFormGroupCheckBox)this;
        }
        public BootstrapFormGroupCheckBox InputWidthSm(int width)
        {
            this._model.InputWidthSm = width;
            return (BootstrapFormGroupCheckBox)this;
        }
        public BootstrapFormGroupCheckBox InputWidthXs(int width)
        {
            this._model.InputWidthXs = width;
            return (BootstrapFormGroupCheckBox)this;
        }

        public BootstrapFormGroupCheckBox LabelOffsetLg(int offset)
        {
            this._model.LabelOffsetLg = offset;
            return (BootstrapFormGroupCheckBox)this;
        }
        public BootstrapFormGroupCheckBox LabelOffsetMd(int offset)
        {
            this._model.LabelOffsetMd = offset;
            return (BootstrapFormGroupCheckBox)this;
        }
        public BootstrapFormGroupCheckBox LabelOffsetSm(int offset)
        {
            this._model.LabelOffsetSm = offset;
            return (BootstrapFormGroupCheckBox)this;
        }
        public BootstrapFormGroupCheckBox LabelOffsetXs(int offset)
        {
            this._model.LabelOffsetXs = offset;
            return (BootstrapFormGroupCheckBox)this;
        }

        public BootstrapFormGroupCheckBox InputOffsetLg(int offset)
        {
            this._model.InputOffsetLg = offset;
            return (BootstrapFormGroupCheckBox)this;
        }
        public BootstrapFormGroupCheckBox InputOffsetMd(int offset)
        {
            this._model.InputOffsetMd = offset;
            return (BootstrapFormGroupCheckBox)this;
        }
        public BootstrapFormGroupCheckBox InputOffsetSm(int offset)
        {
            this._model.InputOffsetSm = offset;
            return (BootstrapFormGroupCheckBox)this;
        }
        public BootstrapFormGroupCheckBox InputOffsetXs(int offset)
        {
            this._model.InputOffsetXs = offset;
            return (BootstrapFormGroupCheckBox)this;
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
