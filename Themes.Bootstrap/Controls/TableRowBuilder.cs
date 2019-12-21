using System.Web.Mvc;
using BeyondThemes.Bootstrap.Infrastructure;

namespace BeyondThemes.Bootstrap.Controls
{
    public class TableRowBuilder<TModel> : BuilderBase<TModel, TableRow>
    {
        internal TableRowBuilder(HtmlHelper<TModel> htmlHelper, TableRow tableRow)
            : base(htmlHelper, tableRow)
        {
        }
    }
}
