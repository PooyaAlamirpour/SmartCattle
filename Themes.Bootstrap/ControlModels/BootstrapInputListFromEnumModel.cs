using System.Web.Mvc;
using BeyondThemes.Bootstrap.Infrastructure.Enums;

namespace BeyondThemes.Bootstrap.ControlModels
{
    public class BootstrapInputListFromEnumModel
    {
        public string htmlFieldName;
        public ModelMetadata metadata;
        public BootstrapInputType inputType;
        public int? numberOfColumns;
        public int columnPixelWidth;
        public bool displayInColumnsCondition;
        public bool displayInlineBlock;
        public int marginRightPx;

        public bool displayValidationMessage;
        public HelpTextStyle validationMessageStyle;
    }
}
