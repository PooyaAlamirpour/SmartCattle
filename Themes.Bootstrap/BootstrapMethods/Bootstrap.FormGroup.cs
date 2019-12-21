using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Web.Mvc;
using BeyondThemes.Bootstrap.Controls;
using BeyondThemes.Bootstrap.Infrastructure.Enums;
using BeyondThemes.Bootstrap.TypeExtensions;

namespace BeyondThemes.Bootstrap.BootstrapMethods
{
    public class BootstrapFormGroup<TModel>
    {
        private HtmlHelper<TModel> html;


        public BootstrapFormGroup(HtmlHelper<TModel> html)
        {
            this.html = html;
        }

        public BootstrapFormGroupDisplayText DisplayText(string htmlFieldName)
        {
            return new BootstrapFormGroupDisplayText(html, htmlFieldName, ModelMetadata.FromStringExpression(htmlFieldName, html.ViewData));
        }

        public BootstrapFormGroupDisplayText DisplayTextFor<TValue>(Expression<Func<TModel, TValue>> expression)
        {
            return new BootstrapFormGroupDisplayText(html, ExpressionHelper.GetExpressionText(expression), ModelMetadata.FromLambdaExpression(expression, html.ViewData));
        }

        public BootstrapFormGroupTextBox TextBox(string htmlFieldName)
        {
            return new BootstrapFormGroupTextBox(html, htmlFieldName, ModelMetadata.FromStringExpression(htmlFieldName, html.ViewData));
        }

        public BootstrapFormGroupTextBox TextBoxFor<TValue>(Expression<Func<TModel, TValue>> expression)
        {
            return new BootstrapFormGroupTextBox(html, ExpressionHelper.GetExpressionText(expression), ModelMetadata.FromLambdaExpression(expression, html.ViewData));
        }

        public BootstrapFormGroupPassword Password(string htmlFieldName)
        {
            return new BootstrapFormGroupPassword(html, htmlFieldName, ModelMetadata.FromStringExpression(htmlFieldName, html.ViewData));
        }

        public BootstrapFormGroupPassword PasswordFor<TValue>(Expression<Func<TModel, TValue>> expression)
        {
            return new BootstrapFormGroupPassword(html, ExpressionHelper.GetExpressionText(expression), ModelMetadata.FromLambdaExpression(expression, html.ViewData));
        }

        public BootstrapFormGroupFile File(string htmlFieldName)
        {
            return new BootstrapFormGroupFile(html, htmlFieldName, ModelMetadata.FromStringExpression(htmlFieldName, html.ViewData));
        }

        public BootstrapFormGroupFile FileFor<TValue>(Expression<Func<TModel, TValue>> expression)
        {
            return new BootstrapFormGroupFile(html, ExpressionHelper.GetExpressionText(expression), ModelMetadata.FromLambdaExpression(expression, html.ViewData));
        }

        public BootstrapFormGroupCheckBox CheckBox(string htmlFieldName)
        {
            return new BootstrapFormGroupCheckBox(html, htmlFieldName, ModelMetadata.FromStringExpression(htmlFieldName, html.ViewData));
        }

        public BootstrapFormGroupCheckBox CheckBoxFor<TValue>(Expression<Func<TModel, TValue>> expression)
        {
            return new BootstrapFormGroupCheckBox(html, ExpressionHelper.GetExpressionText(expression), ModelMetadata.FromLambdaExpression(expression, html.ViewData));
        }

        public BootstrapFormGroupRadioButton RadioButton(string htmlFieldName, object value)
        {
            return new BootstrapFormGroupRadioButton(html, htmlFieldName, value, ModelMetadata.FromStringExpression(htmlFieldName, html.ViewData));
        }

        public BootstrapFormGroupRadioButton RadioButtonFor<TValue>(Expression<Func<TModel, TValue>> expression, object value)
        {
            return new BootstrapFormGroupRadioButton(html, ExpressionHelper.GetExpressionText(expression), value, ModelMetadata.FromLambdaExpression(expression, html.ViewData));
        }

        public BootstrapFormGroupDropDownList DropDownList(string htmlFieldName, IEnumerable<SelectListItem> selectList)
        {
            return new BootstrapFormGroupDropDownList(html, htmlFieldName, selectList, ModelMetadata.FromStringExpression(htmlFieldName, html.ViewData));
        }

        public BootstrapFormGroupDropDownList DropDownListFor<TValue>(Expression<Func<TModel, TValue>> expression, IEnumerable<SelectListItem> selectList)
        {
            return new BootstrapFormGroupDropDownList(html, ExpressionHelper.GetExpressionText(expression), selectList, ModelMetadata.FromLambdaExpression(expression, html.ViewData));
        }

        public BootstrapFormGroupListBox ListBox(string htmlFieldName, IEnumerable<SelectListItem> selectList)
        {
            return new BootstrapFormGroupListBox(html, htmlFieldName, selectList, ModelMetadata.FromStringExpression(htmlFieldName, html.ViewData));
        }

        public BootstrapFormGroupListBox ListBoxFor<TValue>(Expression<Func<TModel, TValue>> expression, IEnumerable<SelectListItem> selectList)
        {
            return new BootstrapFormGroupListBox(html, ExpressionHelper.GetExpressionText(expression), selectList, ModelMetadata.FromLambdaExpression(expression, html.ViewData));
        }

        public BootstrapFormGroupTextArea TextArea(string htmlFieldName)
        {
            return new BootstrapFormGroupTextArea(html, htmlFieldName, ModelMetadata.FromStringExpression(htmlFieldName, html.ViewData));
        }

        public BootstrapFormGroupTextArea TextAreaFor<TValue>(Expression<Func<TModel, TValue>> expression)
        {
            return new BootstrapFormGroupTextArea(html, ExpressionHelper.GetExpressionText(expression), ModelMetadata.FromLambdaExpression(expression, html.ViewData));
        }

        public BootstrapFormGroupInputList<TModel, TSource, SValue, SText> CheckBoxList<TSource, SValue, SText>(
            string htmlFieldName,
            Expression<Func<TModel, IEnumerable<TSource>>> sourceDataExpression,
            Expression<Func<TSource, SValue>> valueExpression,
            Expression<Func<TSource, SText>> textExpression)
        {
            return new BootstrapFormGroupInputList<TModel, TSource, SValue, SText>(
                html,
                htmlFieldName,
                ModelMetadata.FromStringExpression(htmlFieldName, html.ViewData),
                sourceDataExpression,
                valueExpression,
                textExpression,
                BootstrapInputType.CheckBoxList);
        }

        public BootstrapFormGroupInputList<TModel, TSource, SValue, SText> CheckBoxListFor<TValue, TSource, SValue, SText>(
            Expression<Func<TModel, TValue>> expression,
            Expression<Func<TModel, IEnumerable<TSource>>> sourceDataExpression,
            Expression<Func<TSource, SValue>> valueExpression,
            Expression<Func<TSource, SText>> textExpression)
        {
            return new BootstrapFormGroupInputList<TModel, TSource, SValue, SText>(
                html,
                ExpressionHelper.GetExpressionText(expression),
                ModelMetadata.FromLambdaExpression(expression, html.ViewData),
                sourceDataExpression,
                valueExpression,
                textExpression,
                BootstrapInputType.CheckBoxList);
        }

        public BootstrapFormGroupInputList<TModel, TSource, SValue, SText> RadioButtonList<TSource, SValue, SText>(
            string htmlFieldName,
            Expression<Func<TModel, IEnumerable<TSource>>> sourceDataExpression,
            Expression<Func<TSource, SValue>> valueExpression,
            Expression<Func<TSource, SText>> textExpression)
        {
            return new BootstrapFormGroupInputList<TModel, TSource, SValue, SText>(
                html,
                htmlFieldName,
                ModelMetadata.FromStringExpression(htmlFieldName, html.ViewData),
                sourceDataExpression,
                valueExpression,
                textExpression,
                BootstrapInputType.RadioList);
        }

        public BootstrapFormGroupInputList<TModel, TSource, SValue, SText> RadioButtonListFor<TValue, TSource, SValue, SText>(
            Expression<Func<TModel, TValue>> expression,
            Expression<Func<TModel, IEnumerable<TSource>>> sourceDataExpression,
            Expression<Func<TSource, SValue>> valueExpression,
            Expression<Func<TSource, SText>> textExpression)
        {
            return new BootstrapFormGroupInputList<TModel, TSource, SValue, SText>(
                html,
                ExpressionHelper.GetExpressionText(expression),
                ModelMetadata.FromLambdaExpression(expression, html.ViewData),
                sourceDataExpression,
                valueExpression,
                textExpression,
                BootstrapInputType.RadioList);
        }

        public BootstrapFormGroupInputListFromEnum RadioButtonsFromEnum(string htmlFieldName)
        {
            return new BootstrapFormGroupInputListFromEnum(
                html,
                htmlFieldName,
                ModelMetadata.FromStringExpression(htmlFieldName, html.ViewData),
                BootstrapInputType.RadioList);
        }

        public BootstrapFormGroupInputListFromEnum RadioButtonsFromEnumFor<TValue>(Expression<Func<TModel, TValue>> expression)
        {

            return new BootstrapFormGroupInputListFromEnum(
                html,
                ExpressionHelper.GetExpressionText(expression),
                ModelMetadata.FromLambdaExpression(expression, html.ViewData),
                BootstrapInputType.RadioList);
        }

        public BootstrapFormGroupInputListFromEnum CheckBoxesFromEnum(string htmlFieldName)
        {
            return new BootstrapFormGroupInputListFromEnum(
                html,
                htmlFieldName,
                ModelMetadata.FromStringExpression(htmlFieldName, html.ViewData),
                BootstrapInputType.CheckBoxList);
        }

        public BootstrapFormGroupInputListFromEnum CheckBoxesFromEnumFor<TValue>(Expression<Func<TModel, TValue>> expression)
        {
            return new BootstrapFormGroupInputListFromEnum(
                html,
                ExpressionHelper.GetExpressionText(expression),
                ModelMetadata.FromLambdaExpression(expression, html.ViewData),
                BootstrapInputType.CheckBoxList);
        }

        public BootstrapFormGroupRadioButtonTrueFalse RadioButtonTrueFalse(string htmlFieldName)
        {
            return new BootstrapFormGroupRadioButtonTrueFalse(
                html,
                htmlFieldName,
                ModelMetadata.FromStringExpression(htmlFieldName, html.ViewData)
                );
        }

        public BootstrapFormGroupRadioButtonTrueFalse RadioButtonTrueFalseFor<TValue>(
            Expression<Func<TModel, TValue>> expression)
        {
            return new BootstrapFormGroupRadioButtonTrueFalse(
                html,
                ExpressionHelper.GetExpressionText(expression),
                ModelMetadata.FromLambdaExpression(expression, html.ViewData)
                );
        }

        public BootstrapFormGroupCustom<TModel> CustomControls(string controls)
        {
            return new BootstrapFormGroupCustom<TModel>(html, controls);
        }

        public BootstrapFormGroupCustom<TModel> CustomControls(params IHtmlString[] controls)
        {
            string controlsString = string.Empty;
            controls.ToList().ForEach(x => controlsString += x.ToHtmlString());
            return new BootstrapFormGroupCustom<TModel>(html, controlsString);
        }
        
        [EditorBrowsable(EditorBrowsableState.Never)]
        public override string ToString() { return base.ToString(); }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public override bool Equals(object obj) { return base.Equals(obj); }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public override int GetHashCode() { return base.GetHashCode(); }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public new Type GetType() { return base.GetType(); }
    }
}
