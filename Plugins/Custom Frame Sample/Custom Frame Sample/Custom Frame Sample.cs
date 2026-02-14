// -------------------------------------------------------------------------------------------------
//
//    This code is a cTrader Algo API example.
//
//    This code is intended to be used as a sample and does not guarantee any particular outcome or
//    profit of any kind. Use it at your own risk.
//    
//    The sample adds Active Symbol Panel (ASP) block that you can use to interact with custom frame API.
//    It provides functionality to add, remove, attach and detach custom frames through buttons in an ASP block.
//
// -------------------------------------------------------------------------------------------------

using System.Linq;
using cAlgo.API;

namespace cAlgo.Plugins
{
    // Declare the class as a plugin without requiring special access permissions.
    [Plugin(AccessRights = AccessRights.None)]
    public class CustomFrameSample : Plugin
    {
        // This method is executed when the plugin starts.
        protected override void OnStart()
        {
            var aspBlock = Asp.SymbolTab.AddBlock("Custom Frame Sample");  // Adding a new block to the ASP with the title "Custom Frame Sample".

            var panel = new StackPanel();  // Creating a new StackPanel to hold the buttons for managing custom frames.

            // Create and set up the "Add Custom Frame" button.
            var addCustomFrameButton = new Button {Text = "Add Custom Frame", Margin = 5};
            addCustomFrameButton.Click += OnAddCustomFrameButtonClick;  // Add click event handler.
            panel.AddChild(addCustomFrameButton);  // Add the button to the panel.

            // Create and set up the "Remove Custom Frame" button.
            var removeCustomFrameButton = new Button {Text = "Remove Custom Frame", Margin = 5};
            removeCustomFrameButton.Click += OnRemoveCustomFrameButtonClick;
            panel.AddChild(removeCustomFrameButton);

            // Create and set up the "Detach Custom Frame" button.
            var detachCustomFrameButton = new Button {Text = "Detach Custom Frame", Margin = 5};
            detachCustomFrameButton.Click += OnDetachCustomFrameButtonClick;           
            panel.AddChild(detachCustomFrameButton);

            // Create and set up the "Attach Custom Frame" button.
            var attachCustomFrameButton = new Button {Text = "Attach Custom Frame", Margin = 5};
            attachCustomFrameButton.Click += OnAttachCustomFrameButtonClick;            
            panel.AddChild(attachCustomFrameButton);
            
            // Assign the panel with all buttons to the ASP block.
            aspBlock.Child = panel;
        }

        // This method handles the click event for attaching a custom frame.
        private void OnAttachCustomFrameButtonClick(ButtonClickEventArgs obj)
        {
            // Search for a detached custom frame and attach it if found.
            if (ChartManager.OfType<CustomFrame>().FirstOrDefault(c => !c.IsAttached) is not {} customFrame)
                return;

            customFrame.Attach();  // Attach the found custom frame.
        }
        
        // This method handles the click event for detaching a custom frame.
        private void OnDetachCustomFrameButtonClick(ButtonClickEventArgs obj)
        {
            // Search for an attached custom frame and detach it if found.
            if (ChartManager.OfType<CustomFrame>().FirstOrDefault(c => c.IsAttached) is not {} customFrame)
                return;

            customFrame.Detach();  // Detach the found custom frame.
        }

        // This method handles the click event for removing a custom frame.
        private void OnRemoveCustomFrameButtonClick(ButtonClickEventArgs obj)
        {
            // Search for a custom frame and remov it if found.
            if (ChartManager.OfType<CustomFrame>().FirstOrDefault() is not {} customFrame)
                return;

            ChartManager.RemoveFrame(customFrame.Id);  // Remove the custom frame from the chart.
        }

        // This method handles the click event for adding a custom frame.
        private void OnAddCustomFrameButtonClick(ButtonClickEventArgs obj)
        {
            var customFrame = ChartManager.AddCustomFrame("Custom Frame");  // Add a new custom frame to the chart.

            // Create a text block and assign it as the child of the custom frame.
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
