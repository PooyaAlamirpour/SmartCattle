using System.Collections.Generic;
using System.Web.Mvc;

namespace BeyondThemes.Bootstrap.ControlModels
{
    public class BootstrapCheckBoxModel
    {
        public BootstrapCheckBoxModel()
        {
            htmlAttributes = new Dictionary<string, object>();
        }

        // properties for regular mvc checkbox
        public string id;
        public string htmlFieldName;
        public bool isChecked;
        public bool isDisabled;
        public IDictionary<string, object> htmlAttributes;
        public bool displayValidationMessage;
        public HelpTextStyle validationMessageStyle;
        public ModelMetadata metadata;
        public TooltipConfiguration tooltipConfiguration;
        public Tooltip tooltip;
        public string text;

        //properties for a custom checkbox
        public object value;
        public bool isSingleInput;

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
    }
}
