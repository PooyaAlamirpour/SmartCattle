﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Web;

namespace BeyondThemes.Bootstrap.ControlInterfaces
{
    public interface IBootstrapLabel : IHtmlString
    {
        IBootstrapLabel LabelText(string labelText);
        IBootstrapLabel LabelHtml(params IHtmlString[] label);
        IBootstrapLabel ShowRequiredStar(bool showRequiredStar);
        IBootstrapLabel HtmlAttributes(IDictionary<string, object> htmlAttributes);
        IBootstrapLabel HtmlAttributes(object htmlAttributes);

        [EditorBrowsable(EditorBrowsableState.Never)]
        Type GetType();

        [EditorBrowsable(EditorBrowsableState.Never)]
        int GetHashCode();

        [EditorBrowsable(EditorBrowsableState.Never)]
        string ToString();

        [EditorBrowsable(EditorBrowsableState.Never)]
        new string ToHtmlString();

        [EditorBrowsable(EditorBrowsableState.Never)]
        bool Equals(object obj);
    }
}
