using System.Web.Mvc;
using BeyondThemes.Bootstrap.TypeExtensions;

namespace BeyondThemes.Bootstrap.Infrastructure
{
    public static class BootstrapHelper
    {
        public static string GetClassForButtonSize(ButtonSize buttonSize)
        {
            if (buttonSize == ButtonSize.Large) return "btn-lg";
            if (buttonSize == ButtonSize.Small) return "btn-sm";
            if (buttonSize == ButtonSize.Mini) return "btn-xs";
            return string.Empty;
        }

        public static string GetClassForButtonColor(BootstrapColors buttonColor)
        {
            if (buttonColor == BootstrapColors.Primary) return "btn-primary";
            if (buttonColor == BootstrapColors.Info) return "btn-info";
            if (buttonColor == BootstrapColors.Success) return "btn-success";
            if (buttonColor == BootstrapColors.Warning) return "btn-warning";
            if (buttonColor == BootstrapColors.Danger) return "btn-danger";
            if (buttonColor == BootstrapColors.Inverse) return "btn-inverse";
            if (buttonColor == BootstrapColors.Sky) return "btn-sky";
            if (buttonColor == BootstrapColors.Blueberry) return "btn-blueberry";
            if (buttonColor == BootstrapColors.Yellow) return "btn-yellow";
            if (buttonColor == BootstrapColors.Darkorange) return "btn-darkorange";
            if (buttonColor == BootstrapColors.Magenta) return "btn-magenta";
            if (buttonColor == BootstrapColors.Purple) return "btn-purple";
            if (buttonColor == BootstrapColors.Maroon) return "btn-maroon";
            if (buttonColor == BootstrapColors.Darkpink) return "btn-darkpink";
            if (buttonColor == BootstrapColors.Azure) return "btn-azure";
            if (buttonColor == BootstrapColors.Orange) return "btn-orange";
            if (buttonColor == BootstrapColors.Palegreen) return "btn-palegreen";
            if (buttonColor == BootstrapColors.Blue) return "btn-blue";
            return string.Empty;
        }

        public static string GetClassForInputSize(InputSize textBoxSize)
        {
            if (textBoxSize == InputSize.BlockLevel) return "input-block-level";
            if (textBoxSize == InputSize.XSmall) return "input-xs";
            if (textBoxSize == InputSize.Small) return "input-sm";
            if (textBoxSize == InputSize.Default) return "";
            if (textBoxSize == InputSize.Large) return "input-lg";
            if (textBoxSize == InputSize.XLarge) return "input-xl";
            return string.Empty;
        }

        public static string GetPlaceholderFromMetadata(ModelMetadata metadata)
        {
            if (metadata == null) return string.Empty;
            if (!string.IsNullOrEmpty(metadata.Watermark)) return metadata.Watermark;
            if (!string.IsNullOrEmpty(metadata.DisplayName)) return metadata.DisplayName;
            if (!string.IsNullOrEmpty(metadata.PropertyName)) return metadata.PropertyName.SplitByUpperCase();
            return string.Empty;
        }

        public static string GetHelpTextFromMetadata(ModelMetadata metadata)
        {
            if (metadata == null) return string.Empty;
            if (!string.IsNullOrEmpty(metadata.Description)) return metadata.Description;
            return string.Empty;
        }
    }
}
