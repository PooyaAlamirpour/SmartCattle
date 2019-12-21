using System.Collections.Generic;

namespace BeyondThemes.Bootstrap.ControlModels
{
    public class BootstrapButtonModel
    {
        public BootstrapButtonModel()
        {
            htmlAttributes = new Dictionary<string, object>();
        }

        public string type;
        public string text;
        public string id;
        public string value;
        public string name;
        public IDictionary<string, object> htmlAttributes;
        public bool disabled;
        public bool buttonBlock;
        public ButtonSize size;
        public BootstrapColors color;
        public string iconPrepend;
        public string iconAppend;
        public bool iconPrependIsWhite;
        public bool iconAppendIsWhite;
        public string iconPrependCustomClass;
        public string iconAppendCustomClass;
        public bool isDropDownToggle;
        public string loadingText;
        public bool isShiny;
        public bool circular;
        public bool iconOnly;
        public bool labeled;
        public Tooltip tooltip;
        public Popover popover;
    }
}
