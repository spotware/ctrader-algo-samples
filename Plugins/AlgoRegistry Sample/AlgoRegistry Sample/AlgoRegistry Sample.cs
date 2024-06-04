// -------------------------------------------------------------------------------------------------
//
//    This code is a cTrader Algo API example.
//
//    This code is intended to be used as a sample and does not guarantee any particular outcome or
//    profit of any kind. Use it at your own risk.
//    
//    This sample adds a trade watch tab, and uses AlgoRegistry API to show stats about installed algo types.
//
// -------------------------------------------------------------------------------------------------

using cAlgo.API;

namespace cAlgo.Plugins
{
    [Plugin(AccessRights = AccessRights.None)]
    public class AlgoRegistrySample : Plugin
    {
        protected override void OnStart()
        {
            var tradeWatchTab = TradeWatch.AddTab("Algo Registry");

            var panel = new StackPanel
            {
                Orientation = Orientation.Horizontal,
                HorizontalAlignment = HorizontalAlignment.Center,
            };
            
            panel.AddChild(new AlgoStatsControl(AlgoRegistry) {Margin = 10, VerticalAlignment = VerticalAlignment.Top});
            panel.AddChild(new AlgoTypeInfoControl(AlgoRegistry) {Margin = 10, VerticalAlignment = VerticalAlignment.Top});

            tradeWatchTab.Child = panel;
        }
    }        
}