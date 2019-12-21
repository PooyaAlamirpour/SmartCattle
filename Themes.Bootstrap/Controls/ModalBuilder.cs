using System.Web.Mvc;
using BeyondThemes.Bootstrap.Infrastructure;

namespace BeyondThemes.Bootstrap.Controls
{
    public class ModalBuilder<TModel> : BuilderBase<TModel, Modal>
    {
        internal ModalBuilder(HtmlHelper<TModel> htmlHelper, Modal modal)
            : base(htmlHelper, modal)
        {
            switch (this.element._size)
            {
                case BootstrapSizes.Large:
                    base.textWriter.Write(@"<div class=""modal-dialog modal-lg"">");
                    break;
                case BootstrapSizes.Small:
                    base.textWriter.Write(@"<div class=""modal-dialog modal-sm"">");
                    break;
                default:
                    base.textWriter.Write(@"<div class=""modal-dialog"">");
                    break;
            }
            base.textWriter.Write(@"<div class=""modal-content"">");
        }

        public ModalSectionPanel BeginHeader()
        {
            return new ModalSectionPanel(ModalSection.Header, base.textWriter, base.element._closeable);
        }
        public ModalSectionPanel BeginTitle()
        {
            return new ModalSectionPanel(ModalSection.Title, base.textWriter, false);
        }
        public ModalSectionPanel BeginBody()
        {
            return new ModalSectionPanel(ModalSection.Body, base.textWriter, false);
        }

        public ModalSectionPanel BeginFooter()
        {
            return new ModalSectionPanel(ModalSection.Footer, base.textWriter, false);
        }
        public override void Dispose()
        {
            base.textWriter.Write("</div>");
            base.textWriter.Write("</div>");
            base.Dispose();
        }
    }
}
