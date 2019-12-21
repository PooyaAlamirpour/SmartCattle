﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;
using System.Web.Mvc.Html;
using System.Web.Routing;
using BeyondThemes.Bootstrap.Infrastructure;
using BeyondThemes.Bootstrap.Infrastructure.Enums;
using BeyondThemes.Bootstrap.TypeExtensions;

namespace BeyondThemes.Bootstrap.Controls
{
    public class BootstrapActionLinkButton : BootstrapButtonBase<BootstrapActionLinkButton>
    {
        private HtmlHelper html;
        private AjaxHelper ajax;
        private ActionResult _result;
        private Task<ActionResult> _taskResult;
        private string _routeName;
        private string _actionName;
        private string _controllerName;
        private string _protocol;
        private string _hostName;
        private string _fragment;
        private string _title;
        private AjaxOptions _ajaxOptions;
        private RouteValueDictionary _routeValues;
        private ActionTypePassed _actionTypePassed;


        public BootstrapActionLinkButton(HtmlHelper html, string linkText, ActionResult result)
            : base("")
        {
            this.html = html;
            this._model.text = linkText;
            this._result = result;
            this._model.size = ButtonSize.Default;
            this._model.color = BootstrapColors.Default;
            this._actionTypePassed = ActionTypePassed.HtmlActionResult;
        }

        public BootstrapActionLinkButton(HtmlHelper html, string linkText, Task<ActionResult> taskResult)
            : base("")
        {
            this.html = html;
            this._model.text = linkText;
            this._taskResult = taskResult;
            this._model.size = ButtonSize.Default;
            this._model.color = BootstrapColors.Default;
            this._actionTypePassed = ActionTypePassed.HtmlTaskResult;
        }

        public BootstrapActionLinkButton(HtmlHelper html, string linkText, string actionName)
            : base("")
        {
            this.html = html;
            this._model.text = linkText;
            this._actionName = actionName;
            this._model.size = ButtonSize.Default;
            this._model.color = BootstrapColors.Default;
            this._actionTypePassed = ActionTypePassed.HtmlRegular;
        }

        public BootstrapActionLinkButton(HtmlHelper html, string linkText, string actionName, string controllerName)
            : base("")
        {
            this.html = html;
            this._model.text = linkText;
            this._actionName = actionName;
            this._controllerName = controllerName;
            this._model.size = ButtonSize.Default;
            this._model.color = BootstrapColors.Default;
            this._actionTypePassed = ActionTypePassed.HtmlRegular;
        }

        public BootstrapActionLinkButton(AjaxHelper ajax, string linkText, ActionResult result, AjaxOptions ajaxOptions)
            : base("")
        {
            this.ajax = ajax;
            this._model.text = linkText;
            this._result = result;
            this._model.size = ButtonSize.Default;
            this._model.color = BootstrapColors.Default;
            this._ajaxOptions = ajaxOptions;
            this._actionTypePassed = ActionTypePassed.AjaxActionResult;
        }

        public BootstrapActionLinkButton(AjaxHelper ajax, string linkText, Task<ActionResult> taskResult, AjaxOptions ajaxOptions)
            : base("")
        {
            this.ajax = ajax;
            this._model.text = linkText;
            this._taskResult = taskResult;
            this._model.size = ButtonSize.Default;
            this._model.color = BootstrapColors.Default;
            this._ajaxOptions = ajaxOptions;
            this._actionTypePassed = ActionTypePassed.AjaxTaskResult;
        }

        public BootstrapActionLinkButton(AjaxHelper ajax, string linkText, string actionName, AjaxOptions ajaxOptions)
            : base("")
        {
            this.ajax = ajax;
            this._model.text = linkText;
            this._actionName = actionName;
            this._model.size = ButtonSize.Default;
            this._model.color = BootstrapColors.Default;
            this._ajaxOptions = ajaxOptions;
            this._actionTypePassed = ActionTypePassed.AjaxRegular;
        }

        public BootstrapActionLinkButton(AjaxHelper ajax, string linkText, string actionName, string controllerName, AjaxOptions ajaxOptions)
            : base("")
        {
            this.ajax = ajax;
            this._model.text = linkText;
            this._actionName = actionName;
            this._controllerName = controllerName;
            this._model.size = ButtonSize.Default;
            this._model.color = BootstrapColors.Default;
            this._ajaxOptions = ajaxOptions;
            this._actionTypePassed = ActionTypePassed.AjaxRegular;
        }

        public BootstrapActionLinkButton Protocol(string protocol)
        {
            this._protocol = protocol;
            return this;
        }

        public BootstrapActionLinkButton HostName(string hostName)
        {
            this._hostName = hostName;
            return this;
        }

        public BootstrapActionLinkButton Fragment(string fragment)
        {
            this._fragment = fragment;
            return this;
        }

        public BootstrapActionLinkButton RouteName(string routeName)
        {
            this._routeName = routeName;
            return this;
        }

        public BootstrapActionLinkButton RouteValues(object routeValues)
        {
            this._routeValues = routeValues.ToDictionary();
            return this;
        }

        public BootstrapActionLinkButton RouteValues(RouteValueDictionary routeValues)
        {
            this._routeValues = routeValues;
            return this;
        }

        public BootstrapActionLinkButton DropDownToggle()
        {
            this._model.isDropDownToggle = true;
            return this;
        }

        public BootstrapActionLinkButton Title(string title)
        {
            this._title = title;
            return this;
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public override string ToHtmlString()
        {
            var mergedHtmlAttributes = _model.htmlAttributes;
            mergedHtmlAttributes.AddOrMergeCssClass("class", "btn");
            if (!string.IsNullOrEmpty(_model.id)) mergedHtmlAttributes.AddIfNotExist("id", _model.id);

            mergedHtmlAttributes.AddOrMergeCssClass("class", BootstrapHelper.GetClassForButtonSize(_model.size));
            mergedHtmlAttributes.AddOrMergeCssClass("class", BootstrapHelper.GetClassForButtonColor(_model.color));

            if (_model.buttonBlock) mergedHtmlAttributes.AddOrMergeCssClass("class", "btn-block");
            if (_model.isDropDownToggle)
            {
                mergedHtmlAttributes.AddOrMergeCssClass("class", "dropdown-toggle");
                mergedHtmlAttributes.AddIfNotExist("data-toggle", "dropdown");
            }
            if (_model.disabled) mergedHtmlAttributes.AddOrMergeCssClass("class", "disabled");
            if (!string.IsNullOrEmpty(_model.loadingText)) mergedHtmlAttributes.AddOrReplace("data-loading-text", _model.loadingText);
            if (!string.IsNullOrWhiteSpace(_title)) mergedHtmlAttributes.Add("title", _title);

            var input = string.Empty;
            if (_model.iconPrepend != string.Empty || _model.iconAppend != string.Empty || !string.IsNullOrEmpty(_model.iconPrependCustomClass) || !string.IsNullOrEmpty(_model.iconAppendCustomClass))
            {
                var iPrependString = string.Empty;
                var iAppendString = string.Empty;

                if (_model.iconPrepend != string.Empty) iPrependString = new BootstrapIcon(_model.iconPrepend, _model.iconPrependIsWhite).ToHtmlString();
                if (_model.iconAppend != string.Empty) iAppendString = new BootstrapIcon(_model.iconAppend, _model.iconAppendIsWhite).ToHtmlString();
                if (!string.IsNullOrEmpty(_model.iconPrependCustomClass))
                {
                    var i = new TagBuilder("i");
                    i.AddCssClass(_model.iconPrependCustomClass);
                    iPrependString = i.ToString(TagRenderMode.Normal);
                }
                if (!string.IsNullOrEmpty(_model.iconAppendCustomClass))
                {
                    var i = new TagBuilder("i");
                    i.AddCssClass(_model.iconAppendCustomClass);
                    iAppendString = i.ToString(TagRenderMode.Normal);
                }

                var combined = 
                    iPrependString +
                    (!string.IsNullOrEmpty(iPrependString) && (!string.IsNullOrEmpty(_model.text) || !string.IsNullOrEmpty(iAppendString)) ? " " : "") +
                    _model.text +
                    (!string.IsNullOrEmpty(iAppendString) && (!string.IsNullOrEmpty(_model.text) || !string.IsNullOrEmpty(iPrependString)) ? " " : "") +
                    iAppendString;

                var holder = Guid.NewGuid().ToString();

                input = GenerateActionLink(holder, mergedHtmlAttributes);
                input = input.Replace(holder, combined);
            }
            else
            {
                input = GenerateActionLink(_model.text, mergedHtmlAttributes);
            }

            return MvcHtmlString.Create(input).ToString();
        }

        private string GenerateActionLink(string linkText, IDictionary<string, object> htmlAttributes)
        {
            var input = string.Empty;
            switch (_actionTypePassed)
            {
                case ActionTypePassed.HtmlRegular:
                    input = html.ActionLink(linkText, _actionName, _controllerName, _protocol, _hostName, _fragment, _routeValues, htmlAttributes).ToHtmlString();
                    break;
                case ActionTypePassed.HtmlActionResult:
                    input = html.ActionLink(linkText, _result, htmlAttributes, _protocol, _hostName, _fragment).ToHtmlString();
                    break;
                case ActionTypePassed.HtmlTaskResult:
                    input = html.ActionLink(linkText, _taskResult, htmlAttributes, _protocol, _hostName, _fragment).ToHtmlString();
                    break;
                case ActionTypePassed.AjaxRegular:
                    input = ajax.ActionLink(linkText, _actionName, _controllerName, _protocol, _hostName, _fragment, _routeValues, _ajaxOptions, htmlAttributes).ToHtmlString();
                    break;
                case ActionTypePassed.AjaxActionResult:
                    input = ajax.ActionLink(linkText, _result, _ajaxOptions, htmlAttributes).ToHtmlString();
                    break;
                case ActionTypePassed.AjaxTaskResult:
                    input = ajax.ActionLink(linkText, _taskResult, _ajaxOptions, htmlAttributes).ToHtmlString();
                    break;
            }
            return input;
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
