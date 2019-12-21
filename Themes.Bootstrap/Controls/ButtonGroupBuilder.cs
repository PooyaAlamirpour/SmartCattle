using System.Web.Mvc;
using BeyondThemes.Bootstrap.Infrastructure;

namespace BeyondThemes.Bootstrap.Controls
{
    public class ButtonGroupBuilder<TModel> : BuilderBase<TModel, ButtonGroup>
    {
        internal ButtonGroupBuilder(HtmlHelper<TModel> htmlHelper, ButtonGroup buttonGroup)
            : base(htmlHelper, buttonGroup)
        {
        }

        public BootstrapActionLinkButton ActionLinkButton(string linkText, ActionResult result)
        {
            return new BootstrapActionLinkButton(base.htmlHelper, linkText, result);
        }

        public BootstrapActionLinkButton ActionLinkButton(string linkText, string actionName)
        {
            return new BootstrapActionLinkButton(base.htmlHelper, linkText, actionName);
        }

        public BootstrapActionLinkButton ActionLinkButton(string linkText, string actionName, string controllerName)
        {
            return new BootstrapActionLinkButton(base.htmlHelper, linkText, actionName, controllerName);
        }

        public BootstrapButton Button()
        {
            return new BootstrapButton("button");
        }

        public DropDownBuilder<TModel> BeginDropDown(DropDown dropDown)
        {
            return new DropDownBuilder<TModel>(base.htmlHelper, dropDown);
        }
    }
}
