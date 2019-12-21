using System.ComponentModel;
using System.Web.Mvc;
using BeyondThemes.Bootstrap.Infrastructure;

namespace BeyondThemes.Bootstrap.Controls
{
    public class ProgressBuilder<TModel> : BuilderBase<TModel, Progress>
    {
        private bool _isVertical = false;

        internal ProgressBuilder(HtmlHelper<TModel> htmlHelper, Progress progress)
            : base(htmlHelper, progress)
        {
            _isVertical = progress._vertical;
        }

        public ProgressBar BeginProgressBar(string text, string progressId, int value, BootstrapColors color = BootstrapColors.Default, int minValue = 0, int maxValue = 100)
        {
            return new ProgressBar(base.textWriter, text, progressId, value, color, minValue, maxValue, _isVertical);
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public override void Dispose()
        {
            base.Dispose();
        }
    }
}
