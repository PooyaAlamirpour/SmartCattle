using System.ComponentModel;
using System.Web.Mvc;
using BeyondThemes.Bootstrap.Infrastructure;

namespace BeyondThemes.Bootstrap.Controls
{
    public class AccordionBuilder<TModel> : BuilderBase<TModel, Accordion>
    {
        private int PanelsCount { get; set; }

        internal AccordionBuilder(HtmlHelper<TModel> htmlHelper, Accordion accordion)
            : base(htmlHelper, accordion)
        {
        }

        public AccordionPanel BeginPanel(string title)
        {
            this.PanelsCount++;
            bool isActive = this.element._activePanelIndex == PanelsCount ? true : false;
            return new AccordionPanel(base.textWriter, title, base.element.Id + "-" + this.PanelsCount.ToString(), base.element.Id, isActive);
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public override void Dispose()
        {
            base.Dispose();
        }
    }
}
