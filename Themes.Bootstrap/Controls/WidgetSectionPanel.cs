using System;
using System.ComponentModel;
using System.IO;

namespace BeyondThemes.Bootstrap.Controls
{
    public class WidgetSectionPanel : IDisposable
    {
        private readonly TextWriter textWriter;

        internal WidgetSectionPanel(WidgetSection section, TextWriter writer, Widget element)
        {
            textWriter = writer;

            switch (section)
            {
                case WidgetSection.Header:
                    var lined = element._lined ? " lined" : "";
                    var separated = element._separated ? " separated" : "";
                    textWriter.Write(@"<div class=""widget-header " + element._headerSize + element._headerColor + " " + element._headerBorderColor + " " + element._headerBorderDirection + lined + separated  +@""">");
                    if (!string.IsNullOrEmpty(element.headerIcon))
                        textWriter.Write(
                            new BootstrapIcon(element.headerIcon).HtmlAttributes(new { @class = "widget-icon " + element._headerIconColor }));

                    if (!string.IsNullOrEmpty(element.caption))
                        textWriter.Write(@"<span class=""widget-caption"" >" + element.caption + @"</span>");
                    if (element._maximizable || element._collapsable || element._closeable)
                    {
                        var compact = element._compact ? " compact" : "";

                        textWriter.Write(@"<div class=""widget-buttons"+compact+@""">");
                        if (element._maximizable)
                            textWriter.Write(
                                @"<a href=""#"" data-toggle=""maximize""><i class=""fa fa-expand " + element._maximizeButtonClass + @"""></i></a>");
                        if (element._collapsable)
                            if (!element._collapsed)
                                textWriter.Write(
                                    @"<a href=""#"" data-toggle=""collapse""><i class=""fa fa-minus " + element._collapseButtonClass + @"""></i></a>");
                            else
                                textWriter.Write(
                                    @"<a href=""#"" data-toggle=""collapse""><i class=""fa fa-plus " + element._collapseButtonClass + @"""></i></a>");
                        if (element._closeable)
                            textWriter.Write(
                                @"<a href=""#"" data-toggle=""dispose""><i class=""fa fa-times " + element._closeButtonClass + @"""></i></a>");
                        textWriter.Write(@"</div>");
                    }
                    break;
                case WidgetSection.Body:
                    textWriter.Write(@"<div class=""widget-body " + element._bodyColor + " " + element._bodyBorderColor + " " + element._bodyBorderDirection + @""">");
                    break;
            }
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public void Dispose()
        {
            textWriter.Write("</div>");
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public override string ToString()
        {
            return base.ToString();
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public new Type GetType()
        {
            return base.GetType();
        }
    }

    internal enum WidgetSection
    {
        Header,
        Body
    }
}