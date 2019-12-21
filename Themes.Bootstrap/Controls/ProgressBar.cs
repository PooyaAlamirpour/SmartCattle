using System;
using System.IO;
using System.Web.Mvc;

namespace BeyondThemes.Bootstrap.Controls
{
    public class ProgressBar : IDisposable
    {
        private readonly TextWriter textWriter;

        internal ProgressBar(TextWriter writer, string text, string progressId, int value,
            BootstrapColors color = BootstrapColors.Default, int minValue = 0, int maxValue = 100,
            bool isVertical = false)
        {
            textWriter = writer;


            var builder = new TagBuilder("div");

            switch (color)
            {
                case BootstrapColors.Danger:
                    builder.AddCssClass("progress-bar progress-bar-danger");
                    break;
                case BootstrapColors.Default:
                    builder.AddCssClass("progress-bar");
                    break;
                case BootstrapColors.Info:
                    builder.AddCssClass("progress-bar progress-bar-info");
                    break;
                case BootstrapColors.Primary:
                    builder.AddCssClass("progress-bar progress-bar-primary");
                    break;
                case BootstrapColors.Success:
                    builder.AddCssClass("progress-bar progress-bar-success");
                    break;
                case BootstrapColors.Warning:
                    builder.AddCssClass("progress-bar progress-bar-warning");
                    break;
                case BootstrapColors.Sky:
                    builder.AddCssClass("progress-bar progress-bar-sky");
                    break;
                case BootstrapColors.Blueberry:
                    builder.AddCssClass("progress-bar progress-bar-blueberry");
                    break;
                case BootstrapColors.Yellow:
                    builder.AddCssClass("progress-bar progress-bar-yellow");
                    break;
                case BootstrapColors.Darkorange:
                    builder.AddCssClass("progress-bar progress-bar-darkorange");
                    break;
                case BootstrapColors.Magenta:
                    builder.AddCssClass("progress-bar progress-bar-magenta");
                    break;
                case BootstrapColors.Purple:
                    builder.AddCssClass("progress-bar progress-bar-purple");
                    break;
                case BootstrapColors.Maroon:
                    builder.AddCssClass("progress-bar progress-bar-maroon");
                    break;
                case BootstrapColors.Darkpink:
                    builder.AddCssClass("progress-bar progress-bar-darkpink");
                    break;
                case BootstrapColors.Pink:
                    builder.AddCssClass("progress-bar progress-bar-pink");
                    break;
                case BootstrapColors.Azure:
                    builder.AddCssClass("progress-bar progress-bar-azure");
                    break;
                case BootstrapColors.Orange:
                    builder.AddCssClass("progress-bar progress-bar-orange");
                    break;
                case BootstrapColors.Palegreen:
                    builder.AddCssClass("progress-bar progress-bar-palegreen");
                    break;
                case BootstrapColors.Inverse:
                    builder.AddCssClass("progress-bar progress-bar-inverse");
                    break;
                case BootstrapColors.Blue:
                    builder.AddCssClass("progress-bar progress-bar-blue");
                    break;
                default:
                    builder.AddCssClass("progress-bar");
                    break;
            }
            builder.MergeAttribute("role", "progressbar");
            builder.MergeAttribute("aria-valuenow", value.ToString());
            if (!isVertical)
                builder.MergeAttribute("style", @"width:" + value + "%");
            else
                builder.MergeAttribute("style", @"height:" + value + "%");


            builder.MergeAttribute("aria-valuemin", minValue.ToString());
            builder.MergeAttribute("aria-valuemax", maxValue.ToString());

            if (!String.IsNullOrEmpty(text))
            {
                var builder2 = new TagBuilder("span");
                builder2.InnerHtml = text;
                builder.InnerHtml = builder2.ToString();
            }

            builder.MergeAttribute("id", progressId);
            textWriter.Write(builder.ToString());
        }

        public void Dispose()
        {
        }
    }
}