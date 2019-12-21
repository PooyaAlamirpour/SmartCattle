﻿using System;
using System.ComponentModel;
using System.Web.Mvc;
using BeyondThemes.Bootstrap.TypeExtensions;

namespace BeyondThemes.Bootstrap.Controls
{
    public class BootstrapBeginDiv : IDisposable
    {
        private readonly HtmlHelper html;
        private bool _isDisposed;
        

        public BootstrapBeginDiv(HtmlHelper html, string cssClass, object htmlAttributes)
        {
            this.html = html;
            TagBuilder div = new TagBuilder("div");
            div.AddCssClass(cssClass);
            div.MergeAttributes(htmlAttributes.ToDictionary().FormatHtmlAttributes(), true);
            html.ViewContext.Writer.Write(div.ToString(TagRenderMode.StartTag));
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public void Dispose()
        {
            if (!_isDisposed)
            {
                _isDisposed = true;
                html.ViewContext.Writer.Write("</div>");
            }
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public override string ToString() { return base.ToString(); }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public override bool Equals(object obj) { return base.Equals(obj); }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public override int GetHashCode() { return base.GetHashCode(); }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public new Type GetType() { return base.GetType(); }
    }
}
