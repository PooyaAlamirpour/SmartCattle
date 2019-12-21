using System.Collections.Generic;
using System.Web.Mvc;
using BeyondThemes.Bootstrap.Controls;

namespace BeyondThemes.Bootstrap.ControlModels
{
    public class BootstrapFileModel
    {
        public BootstrapFileModel()
        {
            htmlAttributes = new Dictionary<string, object>();
        }

        public string id;
        public string htmlFieldName;
        public IDictionary<string, object> htmlAttributes;
        public BootstrapHelpText helpText;
        public bool displayValidationMessage;
        public HelpTextStyle validationMessageStyle;
        public ModelMetadata metadata;
        public TooltipConfiguration tooltipConfiguration;
        public Tooltip tooltip;
    }
}
