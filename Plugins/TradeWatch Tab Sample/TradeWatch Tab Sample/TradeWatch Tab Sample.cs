// -------------------------------------------------------------------------------------------------
//
//    This code is a cTrader Algo API example.
//
//    This code is intended to be used as a sample and does not guarantee any particular outcome or
//    profit of any kind. Use it at your own risk.
//    
//    This sample adds a trade watch tab, and shows some of the active symbol stats with a basic trading panel on it.
//
// -------------------------------------------------------------------------------------------------

using cAlgo.API;

namespace cAlgo.Plugins
{
    [Plugin(AccessRights = AccessRights.None)]
    public class TradeWatchTabSample : Plugin
    {
        private SymbolStatsControl _symbolStatsControl;
        private TradeControl _tradeControl;

        protected override void OnStart()
        {            
            var tab = TradeWatch.AddTab("Active Chart Symbol Stats");

            var panel = new StackPanel
                {Orientation = Orientation.Vertical, HorizontalAlignment = HorizontalAlignment.Center};

            _symbolStatsControl = new SymbolStatsControl {Margin = 10};
            _tradeControl = new TradeControl {Margin = 10};

            panel.AddChild(_symbolStatsControl);
            panel.AddChild(_tradeControl);

            tab.Child = panel;

            SetSymbolStats();

            _tradeControl.Trade += TradeControlOnTrade;
            ChartManager.ActiveFrameChanged += _ => SetSymbolStats();
        }

        private void TradeControlOnTrade(object sender, TradeEventArgs e)
        {
            ExecuteMarketOrder(e.TradeType, e.SymbolName, e.Volume);
        }

        private void SetSymbolStats()
        {
            if (ChartManager.ActiveFrame is not ChartFrame chartFrame)
                return;

            _tradeControl.Symbol = chartFrame.Symbol;
            _symbolStatsControl.Symbol = chartFrame.Symbol;
        }
    }
}