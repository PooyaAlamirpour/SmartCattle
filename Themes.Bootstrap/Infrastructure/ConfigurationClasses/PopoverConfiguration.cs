using System;
using System.Collections.Generic;
using System.ComponentModel;
using BeyondThemes.Bootstrap.TypeExtensions;

namespace BeyondThemes.Bootstrap
{
    public class PopoverConfiguration : TooltipConfiguration
    {
        public string Content;
        public string _titleClass;

        public PopoverConfiguration(string title, string content, string placement = "right", string titleclass = "")
            : base(title)
        {
            this.Content = content;
            this._titleClass = titleclass;
            this.Placement = placement;
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public override IDictionary<string, object> ToDictionary()
        {
            Dictionary<string, object> result = new Dictionary<string, object>();

            result.AddIfNotExist("data-toggle", "popover");
            if (Animation.HasValue) result.AddIfNotExist("data-animation", Animation.Value ? "true" : "false");
            if (Html.HasValue) result.AddIfNotExist("data-html", Html.Value ? "true" : "false");
            if (!string.IsNullOrEmpty(Placement)) result.AddIfNotExist("data-placement", Placement);
            if (!string.IsNullOrEmpty(_titleClass)) result.AddIfNotExist("data-titleclass", _titleClass);
            if (!string.IsNullOrEmpty(Selector)) result.AddIfNotExist("data-selector", Selector);
            if (!string.IsNullOrEmpty(Title)) result.AddIfNotExist("data-title", Title);
            if (!string.IsNullOrEmpty(Content)) result.AddIfNotExist("data-content", Content);
            if (!string.IsNullOrEmpty(Trigger)) result.AddIfNotExist("data-trigger", Trigger);
            result.AddIfNotExist("data-container", "body");
            result.AddIfNotExist("data-placement", "right");

            return result;
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
