using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Web;
using BeyondThemes.Bootstrap.Controls;

namespace BeyondThemes.Bootstrap.ControlInterfaces
{
    public interface IBootstrapTextBox<T> : IHtmlString
    {
        T Id(string id);
        T Value(object value);
        T HtmlAttributes(IDictionary<string, object> htmlAttributes);
        T HtmlAttributes(object htmlAttributes);
        T Format(string format);
        T Placeholder();
        T Placeholder(string placeholder);
        T Prepend(string prependString);
        T Prepend(BootstrapButton button);
        T PrependIcon(string icon);
        T PrependIcon(string icon, bool isWhite);
        T Append(string appendString);
        T Append(BootstrapButton button);
        T AppendIcon(string icon);
        T AppendIcon(string icon, bool isWhite);
        T HelpText();
        T HelpText(string text);
        T HelpText(string text, HelpTextStyle style);
        T Size(InputSize size);
        T Tooltip(TooltipConfiguration configuration);
        T Tooltip(Tooltip tooltip);
        T Tooltip(string text);
        T ShowValidationMessage(bool displayValidationMessage);
        T ShowValidationMessage(bool displayValidationMessage, HelpTextStyle validationMessageStyle);
        T Typehead(Typehead typehead);
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
