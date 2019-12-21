using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Web.Mvc;
using BeyondThemes.Bootstrap.ControlInterfaces;
using BeyondThemes.Bootstrap.Infrastructure.Enums;
using BeyondThemes.Bootstrap.Renderers;

namespace BeyondThemes.Bootstrap.Controls
{
    public class BootstrapFormGroupDropDownList : BootstrapDropDownList
    {
        public BootstrapFormGroupDropDownList(HtmlHelper html, string htmlFieldName, IEnumerable<SelectListItem> selectList, ModelMetadata metadata)
            : base(html, htmlFieldName, selectList, metadata)
        {
            this._model.displayValidationMessage = true;
        }

        public override IBootstrapLabel Label()
        {
            IBootstrapLabel l = new BootstrapFormGroupLabeled(html, _model, BootstrapInputType.DropDownList);
            return l;
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public override string ToHtmlString()
        {
            return Renderer.RenderFormGroupSelectElement(html, _model, null, BootstrapInputType.DropDownList);
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public override string ToString() { return ToHtmlString(); }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public override bool Equals(object obj) { return base.Equals(obj); }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public override int GetHashCode() { return base.GetHashCode(); }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public new Type GetType() { return base.GetType(); }
    }
}
