// -------------------------------------------------------------------------------------------------
//
//    This code is a cTrader Algo API example.
//
//    This code is intended to be used as a sample and does not guarantee any particular outcome or
//    profit of any kind. Use it at your own risk.
//    
//    This sample adds a ASP block that you can use to interact with custom frame API.
//
// -------------------------------------------------------------------------------------------------

using System.Linq;
using cAlgo.API;

namespace cAlgo.Plugins
{
    [Plugin(AccessRights = AccessRights.None)]
    public class CustomFrameSample : Plugin
    {
        protected override void OnStart()
        {
            var aspBlock = Asp.SymbolTab.AddBlock("Custom Frame Sample");

            var panel = new StackPanel();

            var addCustomFrameButton = new Button {Text = "Add Custom Frame", Margin = 5};
            
            addCustomFrameButton.Click += OnAddCustomFrameButtonClick;
            
            panel.AddChild(addCustomFrameButton);

            var removeCustomFrameButton = new Button {Text = "Remove Custom Frame", Margin = 5};

            removeCustomFrameButton.Click += OnRemoveCustomFrameButtonClick;
            
            panel.AddChild(removeCustomFrameButton);

            var detachCustomFrameButton = new Button {Text = "Detach Custom Frame", Margin = 5};

            detachCustomFrameButton.Click += OnDetachCustomFrameButtonClick;
            
            panel.AddChild(detachCustomFrameButton);
         
            var attachCustomFrameButton = new Button {Text = "Attach Custom Frame", Margin = 5};

            attachCustomFrameButton.Click += OnAttachCustomFrameButtonClick;
            
            panel.AddChild(attachCustomFrameButton);
            
            aspBlock.Child = panel;
        }

        private void OnAttachCustomFrameButtonClick(ButtonClickEventArgs obj)
        {
            if (ChartManager.OfType<CustomFrame>().FirstOrDefault(c => !c.IsAttached) is not {} customFrame)
                return;

            customFrame.Attach();
        }
        
        private void OnDetachCustomFrameButtonClick(ButtonClickEventArgs obj)
        {
            if (ChartManager.OfType<CustomFrame>().FirstOrDefault(c => c.IsAttached) is not {} customFrame)
                return;

            customFrame.Detach();
        }

        private void OnRemoveCustomFrameButtonClick(ButtonClickEventArgs obj)
        {
            if (ChartManager.OfType<CustomFrame>().FirstOrDefault() is not {} customFrame)
                return;

            ChartManager.RemoveFrame(customFrame.Id);
        }

        private void OnAddCustomFrameButtonClick(ButtonClickEventArgs obj)
        {
            var customFrame = ChartManager.AddCustomFrame("Custom Frame");

            customFrame.Child = new TextBlock
            {
                Text = $"Custom Frame {customFrame.Id} Child Control",
                FontSize = 32,
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center
            };
        }
    }        
}