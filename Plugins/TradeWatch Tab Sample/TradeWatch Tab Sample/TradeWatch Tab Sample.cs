// -------------------------------------------------------------------------------------------------
//
//    This code is a cTrader Algo API example.
//
//    This code is intended to be used as a sample and does not guarantee any particular outcome or
//    profit of any kind. Use it at your own risk.
//    
//    The sample adds a Trade Watch tab and shows some of the active symbol stats with a basic trading panel on it.
//
// -------------------------------------------------------------------------------------------------

using cAlgo.API;

namespace cAlgo.Plugins
{
    // Declare the class as a plugin without requiring special access permissions.
    [Plugin(AccessRights = AccessRights.None)]
    public class TradeWatchTabSample : Plugin
    {
        private SymbolStatsControl _symbolStatsControl;  // Declare a variable for the symbol stats control.
        private TradeControl _tradeControl;  // Declare a variable for the trade control component.

        // This method is executed when the plugin starts.
        protected override void OnStart()
        {            
            var tab = TradeWatch.AddTab("Active Chart Symbol Stats");  // Add a new tab to the Trade Watch panel with a custom name.

            var panel = new StackPanel  // Create a new stack panel to arrange child components vertically.
                {Orientation = Orientation.Vertical, HorizontalAlignment = HorizontalAlignment.Center};

            _symbolStatsControl = new SymbolStatsControl {Margin = 10};  // Initialise the symbol stats control with a margin of 10.
            _tradeControl = new TradeControl {Margin = 10};  // Initialise the trade control with a margin of 10.

            panel.AddChild(_symbolStatsControl);  // Add the symbol stats control to the panel.
            panel.AddChild(_tradeControl);  // Add the trade control to the panel.

            tab.Child = panel;  // Assign the created panel as the child of the newly created tab.

            SetSymbolStats();  // Call the method to set the symbol stats for the controls.

            _tradeControl.Trade += TradeControlOnTrade;  // Subscribe to the trade event on the trade control.
            ChartManager.ActiveFrameChanged += _ => SetSymbolStats();  // Update symbol stats when the active chart changes.
        }

        // Method handler for the trade event.
        private void TradeControlOnTrade(object sender, TradeEventArgs e)
        {
            ExecuteMarketOrder(e.TradeType, e.SymbolName, e.Volume);  // Execute a market order based on the trade event details.
        }

        // Method to set the symbol stats on the controls.
        private void SetSymbolStats()
        {
            if (ChartManager.ActiveFrame is not ChartFrame chartFrame)  // Check if the active frame is a valid ChartFrame object.
                return;  // Return if the active frame is not a valid ChartFrame.

            _tradeControl.Symbol = chartFrame.Symbol;  // Set the symbol for the trade control.
            _symbolStatsControl.Symbol = chartFrame.Symbol;  // Set the symbol for the symbol stats control.
        }
    }
}
