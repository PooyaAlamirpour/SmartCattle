﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Web;

namespace BeyondThemes.Bootstrap.ControlInterfaces
{
    public interface IBootstrapDisplayText : IHtmlString
    {
        IBootstrapDisplayText HtmlAttributes(IDictionary<string, object> htmlAttributes);
        IBootstrapDisplayText HtmlAttributes(object htmlAttributes);
        IBootstrapLabel Label();

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
