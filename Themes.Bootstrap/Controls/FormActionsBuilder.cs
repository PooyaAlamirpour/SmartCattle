using System.Web.Mvc;
using BeyondThemes.Bootstrap.Infrastructure;

namespace BeyondThemes.Bootstrap.Controls
{
    public class FormActionsBuilder<TModel> : BuilderBase<TModel, FormActions>
    {
        internal FormActionsBuilder(HtmlHelper<TModel> htmlHelper, FormActions formActions)
            : base(htmlHelper, formActions)
        {
        }
    }
}
