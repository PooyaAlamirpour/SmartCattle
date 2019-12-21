using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Web.Mvc;
using BeyondThemes.Bootstrap.ControlInterfaces;
using BeyondThemes.Bootstrap.Infrastructure.Enums;
using BeyondThemes.Bootstrap.Renderers;

namespace BeyondThemes.Bootstrap.Controls
{
    public class BootstrapFormGroupInputList<TModel, TSource, SValue, SText> : BootstrapInputList<TModel, TSource, SValue, SText>
    {
        public BootstrapFormGroupInputList(
            HtmlHelper html,
            string htmlFieldName,
            ModelMetadata metadata,
            Expression<Func<TModel, IEnumerable<TSource>>> sourceDataExpression,
            Expression<Func<TSource, SValue>> valueExpression,
            Expression<Func<TSource, SText>> textExpression,
            BootstrapInputType inputType)
            : base(html, htmlFieldName, metadata, sourceDataExpression, valueExpression, textExpression, inputType)
        {
            this._model.displayValidationMessage = true;
        }

        public override IBootstrapLabel Label()
        {
            IBootstrapLabel l = new BootstrapFormGroupInputListLabeled<TModel, TSource, SValue, SText>(html, _model, BootstrapInputType.CheckBoxList);
            return l;
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public override string ToHtmlString()
        {
            return RendererList<TModel, TSource, SValue, SText>.RenderFormGroupInputList(html, _model);
        }
    }
}
