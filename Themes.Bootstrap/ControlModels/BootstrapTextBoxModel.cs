using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Web.Mvc;
using BeyondThemes.Bootstrap.Controls;

namespace BeyondThemes.Bootstrap.ControlModels
{
    public class BootstrapTextBoxModel
    {
        public BootstrapTextBoxModel()
        {
            htmlAttributes = new Dictionary<string, object>();
            size = InputSize.Default;
            appendButtons = new List<BootstrapButton>();
            prependButtons = new List<BootstrapButton>();
            disabled = false;
        }

        public string htmlFieldName;
        public string id;
        public object value;
        public string format;
        public IDictionary<string, object> htmlAttributes;
        public string placeholder;
        public string prependString;
        public string appendString;
        public InputSize size;
        public List<BootstrapButton> prependButtons;
        public List<BootstrapButton> appendButtons;
        public bool disabled;
        public string iconPrepend;
        public string iconAppend;
        public bool iconPrependIsWhite;
        public bool iconAppendIsWhite;
        public string iconPrependCustomClass;
        public string iconAppendCustomClass;
        public BootstrapHelpText helpText;
        public bool displayValidationMessage;
        public HelpTextStyle validationMessageStyle;
        public ModelMetadata metadata;
        public TooltipConfiguration tooltipConfiguration;
        public Tooltip tooltip;
        public Typehead typehead;
        public int LabelWidthLg;
        public int LabelWidthMd;
        public int LabelWidthSm;
        public int LabelWidthXs;
        public int InputWidthLg;
        public int InputWidthMd;
        public int InputWidthSm;
        public int InputWidthXs;
        public int LabelOffsetLg;
        public int LabelOffsetMd;
        public int LabelOffsetSm;
        public int LabelOffsetXs;
        public int InputOffsetLg;
        public int InputOffsetMd;
        public int InputOffsetSm;
        public int InputOffsetXs;
        public string appendIconStyle;
        public string prependIconStyle;
    }
}
