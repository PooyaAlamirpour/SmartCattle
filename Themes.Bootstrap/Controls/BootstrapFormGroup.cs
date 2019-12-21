using System.Web;
using System.Web.Mvc;

namespace BeyondThemes.Bootstrap.Controls
{
    public class BootstrapFormGroup : IHtmlString
    {
        private string _input;
        private string _label;
        private FormGroupType _type;
        private bool _fieldIsValid;

        public BootstrapFormGroup(string input, string label, FormGroupType type, bool fieldIsValid = true)
        {
            this._input = input;
            this._label = label;
            this._type = type;
            this._fieldIsValid = fieldIsValid;
        }

        public string ToHtmlString()
        {
            var formGroup = new TagBuilder("div");
            formGroup.AddCssClass("form-group");
            if (!_fieldIsValid) formGroup.AddCssClass("error");

            //var controls = new TagBuilder("div");
            //controls.AddCssClass("controls");

            switch (_type)
            {
                case FormGroupType.textboxLike:
                    //controls.InnerHtml = _input;
                    formGroup.InnerHtml = _label + _input;
                    break;
                case FormGroupType.checkboxLike:
                    //controls.InnerHtml = _label;
                    formGroup.InnerHtml = _label;
                    break;
                default:
                    break;
            }

            return formGroup.ToString();
        }
    }

    public enum FormGroupType
    {
        textboxLike,
        checkboxLike
    }
}
