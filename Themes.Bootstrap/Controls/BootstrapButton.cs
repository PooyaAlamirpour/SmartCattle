﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Web.Mvc;
using BeyondThemes.Bootstrap.ControlInterfaces;
using BeyondThemes.Bootstrap.ControlModels;
using BeyondThemes.Bootstrap.Infrastructure;
using BeyondThemes.Bootstrap.TypeExtensions;

namespace BeyondThemes.Bootstrap.Controls
{
    public class BootstrapButton : BootstrapButtonBase<BootstrapButton>
    {
        public BootstrapButton(string type)
            : base(type)
        {
            this._model.text = "Submit";
        }

        public BootstrapButton WithCaret()
        {
            this._withCaret = true;
            return this;
        }

        public BootstrapButton DropDownToggle()
        {
            this._model.isDropDownToggle = true;
            return this;
        }
    }

    public class BootstrapButtonBase<T> : IBootstrapButton<T>
        where T : BootstrapButtonBase<T>
    {
        protected BootstrapButtonModel _model = new BootstrapButtonModel();
        protected bool _withCaret;

        public BootstrapButtonBase(string type)
        {
            this._model.type = type;
            this._model.size = ButtonSize.Default;
            this._model.color = BootstrapColors.Default;
            this._model.iconPrepend = String.Empty;
            this._model.iconAppend = String.Empty;
        }

        public T Text(string text)
        {
            this._model.text = text;
            return (T)this;
        }

        public T Name(string name)
        {
            this._model.name = name;
            return (T)this;
        }

        public T Id(string id)
        {
            this._model.id = id;
            return (T)this;
        }

        public T Value(string value)
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

        public T Size(ButtonSize size)
        {
            this._model.size = size;
            return (T)this;
        }

        public T Color(BootstrapColors color)
        {
            this._model.color = color;
            return (T)this;
        }

        public T ButtonBlock()
        {
            this._model.buttonBlock = true;
            return (T)this;
        }

        public T LoadingText(string loadingText)
        {
            this._model.loadingText = loadingText;
            return (T)this;
        }

        public T IconPrepend(string icon)
        {
            this._model.iconPrepend = icon;
            return (T)this;
        }

        public T IconPrepend(string icon, bool isWhite)
        {
            this._model.iconPrepend = icon;
            this._model.iconPrependIsWhite = isWhite;
            return (T)this;
        }

        public T IconAppend(string icon)
        {
            this._model.iconAppend = icon;
            return (T)this;
        }

        public T IconAppend(string icon, bool isWhite)
        {
            this._model.iconAppend = icon;
            this._model.iconAppendIsWhite = isWhite;
            return (T)this;
        }

        public T Disabled()
        {
            this._model.disabled = true;
            return (T)this;
        }

        public T Tooltip(Tooltip tooltip)
        {
            this._model.tooltip = tooltip;
            return (T) this;
        }

        public T Popover(Popover popover)
        {
            this._model.popover = popover;
            return (T)this; 
        }

        public T Shiny()
        {
            this._model.isShiny = true;
            return (T)this;
        }

        public T Circular()
        {
            this._model.circular = true;
            return (T)this;
        }

        public T IconOnly()
        {
            this._model.iconOnly = true;
            return (T)this;
        }

        public T Labeled()
        {
            this._model.labeled = true;
            return (T)this;
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public virtual string ToHtmlString()
        {
            var input = new TagBuilder("button");
            input.Attributes.Add("type", _model.type);
            if (!string.IsNullOrEmpty(_model.name)) input.Attributes.Add("name", _model.name);
            if (!string.IsNullOrEmpty(_model.id)) input.Attributes.Add("id", _model.id);
            if (!string.IsNullOrEmpty(_model.value)) input.Attributes.Add("value", _model.value);
            if (_model.tooltip != null) _model.htmlAttributes.MergeHtmlAttributes(_model.tooltip.ToDictionary());
            if (_model.popover != null) _model.htmlAttributes.MergeHtmlAttributes(_model.popover.ToDictionary());
            input.MergeAttributes(_model.htmlAttributes.FormatHtmlAttributes());
            input.AddCssClass(BootstrapHelper.GetClassForButtonSize(_model.size));
            input.AddCssClass(BootstrapHelper.GetClassForButtonColor(_model.color));
            if (_model.buttonBlock) input.AddCssClass("btn-block");
            if (_model.isShiny) input.AddCssClass("shiny");
            if (_model.circular) input.AddCssClass("btn-circle");
            if (_model.iconOnly) input.AddCssClass("icon-only");
            if (_model.labeled) input.AddCssClass("btn-labeled");
            if (_model.isDropDownToggle)
            {
                input.AddCssClass("dropdown-toggle");
                input.AddOrMergeAttribute("data-toggle", "dropdown");
            }
            if (_model.disabled)
            {
                input.AddCssClass("disabled");
                input.AddOrMergeAttribute("disabled", "");
            }

            if (!string.IsNullOrEmpty(_model.loadingText)) input.AddOrMergeAttribute("data-loading-text", _model.loadingText);
            input.AddCssClass("btn");

            if (_withCaret)
            {
                if (!string.IsNullOrEmpty(_model.text)) _model.text += " ";
                _model.text += "<span class='caret'></span>";
            }

            if (_model.iconPrepend != String.Empty || _model.iconAppend != String.Empty || !string.IsNullOrEmpty(_model.iconPrependCustomClass) || !string.IsNullOrEmpty(_model.iconAppendCustomClass))
            {
                var iPrependString = string.Empty;
                var iAppendString = string.Empty;
                var iconLabelString = "";
                if (_model.labeled)
                {
                    iconLabelString = " btn-label";
                }
                if (_model.iconPrepend != String.Empty) iPrependString = new BootstrapIcon(_model.iconPrepend+ iconLabelString, _model.iconPrependIsWhite).ToHtmlString();
                if (_model.iconAppend != String.Empty) iAppendString = new BootstrapIcon(_model.iconAppend + " right" + iconLabelString, _model.iconAppendIsWhite).ToHtmlString();
                if (!string.IsNullOrEmpty(_model.iconPrependCustomClass))
                {
                    var i = new TagBuilder("i");
                    i.AddCssClass(_model.iconPrependCustomClass);
                    iPrependString = i.ToString(TagRenderMode.Normal);
                }
                if (!string.IsNullOrEmpty(_model.iconAppendCustomClass))
                {
                    var i = new TagBuilder("i");
                    i.AddCssClass(_model.iconAppendCustomClass);
                    iAppendString = i.ToString(TagRenderMode.Normal);
                }

                _model.text =
                    iPrependString +
                    (!string.IsNullOrEmpty(iPrependString) && (!string.IsNullOrEmpty(_model.text) || !string.IsNullOrEmpty(iAppendString)) ? " " : "") +
                    _model.text +
                    (!string.IsNullOrEmpty(iAppendString) && (!string.IsNullOrEmpty(_model.text) || !string.IsNullOrEmpty(iPrependString)) ? " " : "") +
                    iAppendString;
            }

            input.InnerHtml = _model.text;
            return input.ToString();
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
