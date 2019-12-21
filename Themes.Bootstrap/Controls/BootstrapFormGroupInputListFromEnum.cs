using System.ComponentModel;
using System.Web.Mvc;
using BeyondThemes.Bootstrap.ControlInterfaces;
using BeyondThemes.Bootstrap.Infrastructure.Enums;
using BeyondThemes.Bootstrap.Renderers;

namespace BeyondThemes.Bootstrap.Controls
{
    public class BootstrapFormGroupInputListFromEnum : BootstrapInputListFromEnum
    {
        public BootstrapFormGroupInputListFromEnum(HtmlHelper html, string htmlFieldName, ModelMetadata metadata, BootstrapInputType inputType)
            : base(html, htmlFieldName, metadata, inputType)
        {
            this._model.displayValidationMessage = true;
        }

        public override IBootstrapLabel Label()
        {
            IBootstrapLabel l = new BootstrapFormGroupInputListFromEnumLabeled(html, _model);
            return l;
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public override string ToHtmlString()
        {
            return Renderer.RenderFormGroupInputListFromEnum(html, _model);
        }
    }
}
