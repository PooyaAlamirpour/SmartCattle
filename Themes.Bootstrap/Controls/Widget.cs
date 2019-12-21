using System.Collections.Generic;
using System.ComponentModel;
using BeyondThemes.Bootstrap.Infrastructure;

namespace BeyondThemes.Bootstrap
{
    public class Widget : HtmlElement
    {
        [EditorBrowsable(EditorBrowsableState.Never)]
        public bool _collapsed { get; set; }
        public bool _closeable { get; set; }
        public bool _collapsable { get; set; }
        public bool _maximizable { get; set; }
        public string _closeButtonClass { get; set; }
        public string _collapseButtonClass { get; set; }
        public string _maximizeButtonClass { get; set; }
        public string _headerSize { get; set; }
        public string headerIcon { get; set; }
        public string _headerIconColor { get; set; }
        public string _headerColor { get; set; }
        public string _headerBorderColor { get; set; }
        public string _headerBorderDirection { get; set; }
        public string _bodyBorderColor { get; set; }
        public string _bodyBorderDirection { get; set; }
        public string _bodyColor { get; set; }
        public string caption { get; set; }
        public bool _lined { get; set; }
        public bool _separated { get; set; }
        public bool _compact { get; set; }

        public Widget()
            : base("div")
        {
            EnsureClass("widget");
        }

        public Widget Id(string id)
        {
            MergeHtmlAttribute("id", id);
            return this;
        }

        public Widget Icon(string icon)
        {
            this.headerIcon = icon;
            return this;
        }

        public Widget Caption(string caption)
        {
            this.caption = caption;
            return this;
        }

        public Widget HtmlAttributes(IDictionary<string, object> htmlAttributes)
        {
            SetHtmlAttributes(htmlAttributes);
            return this;
        }

        public Widget HtmlAttributes(object htmlAttributes)
        {
            SetHtmlAttributes(htmlAttributes);
            return this;
        }

        public Widget Collapse()
        {
            EnsureClass("collapsed");
            this._collapsed = true;
            return this;
        }

        public Widget Flat()
        {
            EnsureClass("flat");
            return this;
        }
        public Widget RadiusBordered()
        {
            EnsureClass("radius-bordered");
            return this;
        }

        public Widget Transparent()
        {
            EnsureClass("transparent");
            return this;
        }

        public Widget Lined()
        {
            this._lined = true;
            return this;
        }

        public Widget Separated()
        {
            this._separated = true;
            return this;
        }

        public Widget Compact()
        {
            this._compact = true;
            return this;
        }
        public Widget Closeable()
        {
            this._closeable = true;
            return this;
        }

        public Widget Collapsable()
        {
            this._collapsable = true;
            return this;
        }

        public Widget Maximizable()
        {
            this._maximizable = true;
            return this;
        }

        public Widget Closeable(string closeButtonClass)
        {
            this._closeButtonClass = closeButtonClass;
            this._closeable = true;
            return this;
        }

        public Widget Collapsable(string collapseButtonClass)
        {
            this._collapseButtonClass = collapseButtonClass;
            this._collapsable = true;
            return this;
        }

        public Widget Maximizable(string maximizeButtonClass)
        {
            this._maximizeButtonClass = maximizeButtonClass;
            this._maximizable = true;
            return this;
        }

        public Widget HeaderSize(BootstrapSizes size)
        {
            switch (size)
            {
                case BootstrapSizes.Large:
                    this._headerSize= " header-large";
                    break;
                case BootstrapSizes.Small:
                    this._headerSize = " header-small";
                    break;
                default:
                    break;
            }
            return this;
        }

        public Widget HeaderIconColor(BootstrapColors color)
        {
            switch (color)
            {
                case BootstrapColors.Danger:
                    _headerIconColor = "danger";
                    break;
                case BootstrapColors.Default:
                    break;
                case BootstrapColors.Info:
                    _headerIconColor = "info";
                    break;
                case BootstrapColors.Primary:
                    _headerIconColor = "primary";
                    break;
                case BootstrapColors.Success:
                    _headerIconColor = "success";
                    break;
                case BootstrapColors.Warning:
                    _headerIconColor = "warning";
                    break;
                case BootstrapColors.Sky:
                    _headerIconColor = "sky";
                    break;
                case BootstrapColors.Blueberry:
                    _headerIconColor = "blueberry";
                    break;
                case BootstrapColors.Yellow:
                    _headerIconColor = "yellow";
                    break;
                case BootstrapColors.Darkorange:
                    _headerIconColor = "darkorange";
                    break;
                case BootstrapColors.Magenta:
                    _headerIconColor = "magenta";
                    break;
                case BootstrapColors.Purple:
                    _headerIconColor = "purple";
                    break;
                case BootstrapColors.Maroon:
                    _headerIconColor = "maroon";
                    break;
                case BootstrapColors.Darkpink:
                    _headerIconColor = "darkpink";
                    break;
                case BootstrapColors.Pink:
                    _headerIconColor = "pink";
                    break;
                case BootstrapColors.Azure:
                    _headerIconColor = "azure";
                    break;
                case BootstrapColors.Orange:
                    _headerIconColor = "orange";
                    break;
                case BootstrapColors.Palegreen:
                    _headerIconColor = "palegreen";
                    break;
                case BootstrapColors.Inverse:
                    _headerIconColor = "inverse";
                    break;
                case BootstrapColors.Blue:
                    _headerIconColor = "blue";
                    break;
                default:
                    break;
            }
            return this;
        }

        public Widget HeaderColor(BootstrapColors color)
        {
            switch (color)
            {
                case BootstrapColors.Danger:
                    _headerColor = "bg-danger";
                    break;
                case BootstrapColors.Default:
                    break;
                case BootstrapColors.Info:
                    _headerColor = "bg-info";
                    break;
                case BootstrapColors.Primary:
                    _headerColor = "bg-primary";
                    break;
                case BootstrapColors.Success:
                    _headerColor = "bg-success";
                    break;
                case BootstrapColors.Warning:
                    _headerColor = "bg-warning";
                    break;
                case BootstrapColors.Sky:
                    _headerColor = "bg-sky";
                    break;
                case BootstrapColors.Blueberry:
                    _headerColor = "bg-blueberry";
                    break;
                case BootstrapColors.Yellow:
                    _headerColor = "bg-yellow";
                    break;
                case BootstrapColors.Darkorange:
                    _headerColor = "bg-darkorange";
                    break;
                case BootstrapColors.Magenta:
                    _headerColor = "bg-magenta";
                    break;
                case BootstrapColors.Purple:
                    _headerColor = "bg-purple";
                    break;
                case BootstrapColors.Maroon:
                    _headerColor = "bg-maroon";
                    break;
                case BootstrapColors.Darkpink:
                    _headerColor = "bg-darkpink";
                    break;
                case BootstrapColors.Pink:
                    _headerColor = "bg-pink";
                    break;
                case BootstrapColors.Azure:
                    _headerColor = "bg-azure";
                    break;
                case BootstrapColors.Orange:
                    _headerColor = "bg-orange";
                    break;
                case BootstrapColors.Palegreen:
                    _headerColor = "bg-palegreen";
                    break;
                case BootstrapColors.Inverse:
                    _headerColor = "bg-inverse";
                    break;
                case BootstrapColors.Blue:
                    _headerColor = "bg-blue";
                    break;
                default:
                    break;
            }
            return this;
        }

        public Widget BodyColor(BootstrapColors color)
        {
            switch (color)
            {
                case BootstrapColors.Danger:
                    _bodyColor = "bg-danger";
                    break;
                case BootstrapColors.Default:
                    break;
                case BootstrapColors.Info:
                    _bodyColor = "bg-info";
                    break;
                case BootstrapColors.Primary:
                    _bodyColor = "bg-primary";
                    break;
                case BootstrapColors.Success:
                    _bodyColor = "bg-success";
                    break;
                case BootstrapColors.Warning:
                    _bodyColor = "bg-warning";
                    break;
                case BootstrapColors.Sky:
                    _bodyColor = "bg-sky";
                    break;
                case BootstrapColors.Blueberry:
                    _bodyColor = "bg-blueberry";
                    break;
                case BootstrapColors.Yellow:
                    _bodyColor = "bg-yellow";
                    break;
                case BootstrapColors.Darkorange:
                    _bodyColor = "bg-darkorange";
                    break;
                case BootstrapColors.Magenta:
                    _bodyColor = "bg-magenta";
                    break;
                case BootstrapColors.Purple:
                    _bodyColor = "bg-purple";
                    break;
                case BootstrapColors.Maroon:
                    _bodyColor = "bg-maroon";
                    break;
                case BootstrapColors.Darkpink:
                    _bodyColor = "bg-darkpink";
                    break;
                case BootstrapColors.Pink:
                    _bodyColor = "bg-pink";
                    break;
                case BootstrapColors.Azure:
                    _bodyColor = "bg-azure";
                    break;
                case BootstrapColors.Orange:
                    _bodyColor = "bg-orange";
                    break;
                case BootstrapColors.Palegreen:
                    _bodyColor = "bg-palegreen";
                    break;
                case BootstrapColors.Inverse:
                    _bodyColor = "bg-inverse";
                    break;
                case BootstrapColors.Blue:
                    _bodyColor = "bg-blue";
                    break;
                default:
                    break;
            }
            return this;
        }

        public Widget HeaderBorder(BootstrapColors color, Direction direction)
        {
            switch (color)
            {
                case BootstrapColors.Danger:
                    _headerBorderColor = "bordered-danger";
                    break;
                case BootstrapColors.Default:
                    break;
                case BootstrapColors.Info:
                    _headerBorderColor = "bordered-info";
                    break;
                case BootstrapColors.Primary:
                    _headerBorderColor = "bordered-primary";
                    break;
                case BootstrapColors.Success:
                    _headerBorderColor = "bordered-success";
                    break;
                case BootstrapColors.Warning:
                    _headerBorderColor = "bordered-warning";
                    break;
                case BootstrapColors.Sky:
                    _headerBorderColor = "bordered-sky";
                    break;
                case BootstrapColors.Blueberry:
                    _headerBorderColor = "bordered-blueberry";
                    break;
                case BootstrapColors.Yellow:
                    _headerBorderColor = "bordered-yellow";
                    break;
                case BootstrapColors.Darkorange:
                    _headerBorderColor = "bordered-darkorange";
                    break;
                case BootstrapColors.Magenta:
                    _headerBorderColor = "bordered-magenta";
                    break;
                case BootstrapColors.Purple:
                    _headerBorderColor = "bordered-purple";
                    break;
                case BootstrapColors.Maroon:
                    _headerBorderColor = "bordered-maroon";
                    break;
                case BootstrapColors.Darkpink:
                    _headerBorderColor = "bordered-darkpink";
                    break;
                case BootstrapColors.Pink:
                    _headerBorderColor = "bordered-pink";
                    break;
                case BootstrapColors.Azure:
                    _headerBorderColor = "bordered-azure";
                    break;
                case BootstrapColors.Orange:
                    _headerBorderColor = "bordered-orange";
                    break;
                case BootstrapColors.Palegreen:
                    _headerBorderColor = "bordered-palegreen";
                    break;
                case BootstrapColors.Inverse:
                    _headerBorderColor = "bordered-inverse";
                    break;
                case BootstrapColors.Blue:
                    _headerBorderColor = "bordered-blue";
                    break;
                default:
                    break;
            }

            switch (direction)
            {
                case Direction.Bottom:
                    _headerBorderDirection = "bordered-bottom";
                    break;
                case Direction.Top:
                    _headerBorderDirection = "bordered-top";
                    break;
                case Direction.Right:
                    _headerBorderDirection = "bordered-right";
                    break;
                case Direction.Left:
                    _headerBorderDirection = "bordered-left";
                    break;
                default:
                    break;
            }
            return this;
        }

        public Widget BodyBorder(BootstrapColors color, Direction direction)
        {
            switch (color)
            {
                case BootstrapColors.Danger:
                    _bodyBorderColor = "bordered-danger";
                    break;
                case BootstrapColors.Default:
                    break;
                case BootstrapColors.Info:
                    _bodyBorderColor = "bordered-info";
                    break;
                case BootstrapColors.Primary:
                    _bodyBorderColor = "bordered-primary";
                    break;
                case BootstrapColors.Success:
                    _bodyBorderColor = "bordered-success";
                    break;
                case BootstrapColors.Warning:
                    _bodyBorderColor = "bordered-warning";
                    break;
                case BootstrapColors.Sky:
                    _bodyBorderColor = "bordered-sky";
                    break;
                case BootstrapColors.Blueberry:
                    _bodyBorderColor = "bordered-blueberry";
                    break;
                case BootstrapColors.Yellow:
                    _bodyBorderColor = "bordered-yellow";
                    break;
                case BootstrapColors.Darkorange:
                    _bodyBorderColor = "bordered-darkorange";
                    break;
                case BootstrapColors.Magenta:
                    _bodyBorderColor = "bordered-magenta";
                    break;
                case BootstrapColors.Purple:
                    _bodyBorderColor = "bordered-purple";
                    break;
                case BootstrapColors.Maroon:
                    _bodyBorderColor = "bordered-maroon";
                    break;
                case BootstrapColors.Darkpink:
                    _bodyBorderColor = "bordered-darkpink";
                    break;
                case BootstrapColors.Pink:
                    _bodyBorderColor = "bordered-pink";
                    break;
                case BootstrapColors.Azure:
                    _bodyBorderColor = "bordered-azure";
                    break;
                case BootstrapColors.Orange:
                    _bodyBorderColor = "bordered-orange";
                    break;
                case BootstrapColors.Palegreen:
                    _bodyBorderColor = "bordered-palegreen";
                    break;
                case BootstrapColors.Inverse:
                    _bodyBorderColor = "bordered-inverse";
                    break;
                case BootstrapColors.Blue:
                    _bodyBorderColor = "bordered-blue";
                    break;
                default:
                    break;
            }

            switch (direction)
            {
                case Direction.Bottom:
                    _bodyBorderDirection = "bordered-bottom";
                    break;
                case Direction.Top:
                    _bodyBorderDirection = "bordered-top";
                    break;
                case Direction.Right:
                    _bodyBorderDirection = "bordered-right";
                    break;
                case Direction.Left:
                    _bodyBorderDirection = "bordered-left";
                    break;
                default:
                    break;
            }
            return this;
        }
    }
}
