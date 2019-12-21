using System.Collections.Generic;
using System.Web.Mvc;
using BeyondThemes.Bootstrap.Infrastructure.Enums;

namespace BeyondThemes.Bootstrap.ControlModels
{
    public class BootstrapLabelModel
    {
        public BootstrapLabelModel()
        {
            htmlAttributes = new Dictionary<string, object>();
            innerInputType = BootstrapInputType._NotSet;
        }

        public int? index;
        public string htmlFieldName;
        public string labelText;
        public bool? showRequiredStar;
        public ModelMetadata metadata;
        public IDictionary<string, object> htmlAttributes;
        public MvcHtmlString innerInput;
        public string innerValidationMessage;
        public BootstrapInputType innerInputType;
        public object innerInputModel;
    }
}
