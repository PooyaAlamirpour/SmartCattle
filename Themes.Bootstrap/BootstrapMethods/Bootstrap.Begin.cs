using BeyondThemes.Bootstrap.Controls;

namespace BeyondThemes.Bootstrap.BootstrapMethods
{
    public partial class Bootstrap<TModel>
    {
        public WidgetBuilder<TModel> Begin(Widget widget)
        {
            return new WidgetBuilder<TModel>(Html, widget);
        }
        public ModalBuilder<TModel> Begin(Modal modal)
        {
            return new ModalBuilder<TModel>(Html, modal);
        }

        public AccordionBuilder<TModel> Begin(Accordion accordion)
        {
            return new AccordionBuilder<TModel>(Html, accordion);
        }

        public ProgressBuilder<TModel> Begin(Progress progress)
        {
            return new ProgressBuilder<TModel>(Html, progress);
        }

        public CarouselBuilder<TModel> Begin(Carousel carousel)
        {
            return new CarouselBuilder<TModel>(Html, carousel);
        }

        public NavBuilder<TModel> Begin(Nav nav)
        {
            return new NavBuilder<TModel>(Html, nav);
        }

        public TabsBuilder<TModel> Begin(Tabs nav)
        {
            return new TabsBuilder<TModel>(Html, nav);
        }

        public FormBuilder<TModel> Begin(Form form)
        {
            return new FormBuilder<TModel>(Html, form);
        }

        public ButtonGroupBuilder<TModel> Begin(ButtonGroup buttonGroup)
        {
            return new ButtonGroupBuilder<TModel>(Html, buttonGroup);
        }

        public DropDownBuilder<TModel> Begin(DropDown dropDown)
        {
            return new DropDownBuilder<TModel>(Html, dropDown, "div", new { @class = "dropdown" });
        }

        public ToolBarBuilder<TModel> Begin(ToolBar toolBar)
        {
            return new ToolBarBuilder<TModel>(Html, toolBar);
        }

        public FormActionsBuilder<TModel> Begin(FormActions formActions)
        {
            return new FormActionsBuilder<TModel>(Html, formActions);
        }

        public AlertBuilder<TModel> Begin(Alert alert)
        {
            return new AlertBuilder<TModel>(Html, alert);
        }

        public TableBuilder<TModel> Begin(Table table)
        {
            return new TableBuilder<TModel>(Html, table);
        }

    }
}