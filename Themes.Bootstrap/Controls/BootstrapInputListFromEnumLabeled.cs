﻿using System;
using System.ComponentModel;
using System.Web.Mvc;
using BeyondThemes.Bootstrap.ControlModels;
using BeyondThemes.Bootstrap.Infrastructure.Enums;
using BeyondThemes.Bootstrap.Renderers;

namespace BeyondThemes.Bootstrap.Controls
{
    public class BootstrapInputListFromEnumLabeled : BootstrapInputLabeled
    {
        public BootstrapInputListFromEnumLabeled(HtmlHelper html, object inputModel)
            : base(html, inputModel, BootstrapInputType.CheckBox)
        {
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public override string ToHtmlString()
        {
            if (_inputModel == null) return null;
            string result = string.Empty;

            var input = Renderer.RenderInputListFromEnum(html, (BootstrapInputListFromEnumModel)_inputModel);
            var label = Renderer.RenderLabel(html, _labelModel);

            result = label + input;
            return result;
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
