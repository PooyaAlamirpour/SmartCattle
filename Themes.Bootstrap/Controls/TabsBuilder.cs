using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Web.Mvc;
using BeyondThemes.Bootstrap.Infrastructure;

namespace BeyondThemes.Bootstrap.Controls
{
    public class TabsBuilder<TModel> : BuilderBase<TModel, Tabs>
    {
        private int _tabIndex;
        private int _panelIndex;
        private readonly Queue<string> _tabIds;
        private bool _isFirstTab = true;
        private int _activeTabIndex;
        private bool _isHeaderClosed;

        internal TabsBuilder(HtmlHelper<TModel> htmlHelper, Tabs tabs)
            : base(htmlHelper, tabs)
        {
            _tabIndex = 1;
            _activeTabIndex = element._activeTabIndex;
            _tabIds = new Queue<string>();
            var justified = element.IsJustified ? "nav-justified" : "";
            var flat = element.IsFlat ? " tabs-flat" : "";
            switch (element.Type)
            {
                case NavType.Pills:
                    textWriter.Write(@"<ul class=""nav nav-pills " + justified + flat + @""">");
                    break;
                case NavType.List:
                    textWriter.Write(@"<ul class=""nav nav-list " + justified + flat + @""">");
                    break;
                default:
                    textWriter.Write(@"<ul class=""nav nav-tabs " + justified + flat + @""">");
                    break;
            }
        }

        public void Tab(string label, BootstrapColors borderColor = BootstrapColors.Default)
        {
            if (string.IsNullOrWhiteSpace(label))
                throw new ArgumentNullException("label");

            var tabId = base.element._id + "-" + _tabIndex;
            _tabIds.Enqueue(tabId);

            if (_isFirstTab)
            {
                if (_activeTabIndex == 0) _activeTabIndex = 1;
                WriteTab(label, borderColor, tabId, _tabIndex == _activeTabIndex);
                _isFirstTab = false;
            }
            else
            {
                WriteTab(label, borderColor, tabId, _tabIndex == _activeTabIndex);
            }

            _tabIndex++;
        }

        public TabsPanel BeginPanel()
        {
            _panelIndex++;
            if (!_isHeaderClosed)
            {
                textWriter.Write("</ul>");
                _isHeaderClosed = true;
            }

            var tabId = _tabIds.Dequeue();
            if (_panelIndex == 1)
            {
                var radiusBordered = element.IsRadiusBordered ? "radius-bordered" : "";
                var flat = element.IsFlat ? " tabs-flat" : "";

                textWriter.Write(@"<div class=""tab-content " + radiusBordered + flat + @""">");
                _isFirstTab = false;
                return new TabsPanel(textWriter, "div", tabId, _panelIndex == _activeTabIndex);
            }

            return new TabsPanel(base.textWriter, "div", tabId, _panelIndex == _activeTabIndex);
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public override void Dispose()
        {
            if (_tabIndex == 1) throw new ArgumentNullException("Tab", "You should specify at least one tab");
            if (_tabIds.Count > 0) throw new ArgumentNullException("BeginPanel", "The number of panels should be the same as the number of tabs.");

            // Close Tab Content Div:
            textWriter.Write("</div>");
            base.Dispose();
        }

        private void WriteTab(string label, BootstrapColors borderColor, string href, bool isActive)
        {
            string tabColor = "";
            switch (borderColor)
            {
                case BootstrapColors.Danger:
                    tabColor = "tab-danger";
                    break;
                case BootstrapColors.Default:
                    tabColor = "";
                    break;
                case BootstrapColors.Info:
                    tabColor = "tab-info";
                    break;
                case BootstrapColors.Primary:
                    tabColor = "tab-primary";
                    break;
                case BootstrapColors.Success:
                    tabColor = "tab-success";
                    break;
                case BootstrapColors.Warning:
                    tabColor = "tab-warning";
                    break;
                case BootstrapColors.Sky:
                    tabColor = "tab-sky";
                    break;
                case BootstrapColors.Blueberry:
                    tabColor = "tab-blueberry";
                    break;
                case BootstrapColors.Yellow:
                    tabColor = "tab-yellow";
                    break;
                case BootstrapColors.Darkorange:
                    tabColor = "tab-darkorange";
                    break;
                case BootstrapColors.Magenta:
                    tabColor = "tab-magenta";
                    break;
                case BootstrapColors.Purple:
                    tabColor = "tab-purple";
                    break;
                case BootstrapColors.Maroon:
                    tabColor = "tab-maroon";
                    break;
                case BootstrapColors.Darkpink:
                    tabColor = "tab-darkpink";
                    break;
                case BootstrapColors.Pink:
                    tabColor = "tab-pink";
                    break;
                case BootstrapColors.Azure:
                    tabColor = "tab-azure";
                    break;
                case BootstrapColors.Orange:
                    tabColor = "tab-orange";
                    break;
                case BootstrapColors.Inverse:
                    tabColor = "tab-inverse";
                    break;
                case BootstrapColors.Palegreen:
                    tabColor = "tab-palegreen";
                    break;
                case BootstrapColors.Blue:
                    tabColor = "tab-blue";
                    break;
                default:
                    tabColor = "";
                    break;
            }


            textWriter.Write(isActive
                ? string.Format(@"<li class=""active " + tabColor + @"""><a data-toggle=""tab"" href=""#{1}"">{0}</a></li>", label, href)
                : string.Format(@"<li class=""" + tabColor + @"""><a data-toggle=""tab"" href=""#{1}"">{0}</a></li>", label, href)
            );
        }
    }
}
