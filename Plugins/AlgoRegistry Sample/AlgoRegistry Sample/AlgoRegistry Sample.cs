// -------------------------------------------------------------------------------------------------
//
//    This code is a cTrader Algo API example.
//
//    This code is intended to be used as a sample and does not guarantee any particular outcome or
//    profit of any kind. Use it at your own risk.
//    
//    The sample adds a trade watch tab and uses Algo Registry API to show stats about installed algo types.
//
// -------------------------------------------------------------------------------------------------

using cAlgo.API;

namespace cAlgo.Plugins
{
    // Declare the class as a plugin without requiring special access permissions.
    [Plugin(AccessRights = AccessRights.None)]
    public class AlgoRegistrySample : Plugin
    {
        // This method is triggered when the plugin starts.
        protected override void OnStart()
        {
            var tradeWatchTab = TradeWatch.AddTab("Algo Registry");  // Add a new tab to the Trade Watch section, named "Algo Registry".

            var panel = new StackPanel  // Initialise a StackPanel to hold UI elements.
            {
                Orientation = Orientation.Horizontal,  // Set the panel orientation to horizontal.
                HorizontalAlignment = HorizontalAlignment.Center,  // Centre the panel within its parent container horizontally.
            };
            
            panel.AddChild(new AlgoStatsControl(AlgoRegistry) {Margin = 10, VerticalAlignment = VerticalAlignment.Top});  // Add the AlgoStatsControl to the panel with a margin and top vertical alignment.
            panel.AddChild(new AlgoTypeInfoControl(AlgoRegistry) {Margin = 10, VerticalAlignment = VerticalAlignment.Top});  // Add the AlgoTypeInfoControl to the panel with a margin and top vertical alignment.

            tradeWatchTab.Child = panel;  // Set the StackPanel containing the controls as the content of the "Algo Registry" tab in the Trade Watch section.
        }
    }        
}
