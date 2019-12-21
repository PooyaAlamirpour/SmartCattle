using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Web;

namespace BeyondThemes.Bootstrap.ControlInterfaces
{
    public interface IBootstrapButton<T> : IHtmlString
    {
        T Id(string id);
        T Text(string text);
        T Name(string name);
        T Value(string value);
        T HtmlAttributes(IDictionary<string, object> htmlAttributes);
        T HtmlAttributes(object htmlAttributes);
        T Size(ButtonSize size);
        T Color(BootstrapColors color);
        T ButtonBlock();
        T IconPrepend(string icon);
        T IconPrepend(string icon, bool isWhite);
        T IconAppend(string icon);
        T IconAppend(string icon, bool isWhite);
        T Disabled();
        T Tooltip(Tooltip tooltip);
        T Popover(Popover popover);


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
