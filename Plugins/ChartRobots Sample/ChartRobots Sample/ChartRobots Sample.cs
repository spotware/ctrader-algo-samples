// -------------------------------------------------------------------------------------------------
//
//    This code is a cTrader Algo API example.
//
//    This code is intended to be used as a sample and does not guarantee any particular outcome or
//    profit of any kind. Use it at your own risk.
//    
//    This sample adds an active symbol panel tab, and uses ChartRobots API to show stats about
//    active chart cBots and lets you add and remove cBots to active chart.
//
// -------------------------------------------------------------------------------------------------

using cAlgo.API;

namespace cAlgo.Plugins
{
    [Plugin(AccessRights = AccessRights.None)]
    public class ChartRobotsSample : Plugin
    {
        private ChartRobotsControl _chartRobotsControl;
        
        protected override void OnStart()
        {
            var aspTab = Asp.AddTab("Chart Robots");
            
            _chartRobotsControl = new ChartRobotsControl(AlgoRegistry)
            {
                VerticalAlignment = VerticalAlignment.Top
            };

            aspTab.Child = _chartRobotsControl;

            SetControlChart();

            ChartManager.ActiveFrameChanged += _ => SetControlChart();
        }

        private void SetControlChart()
        {
            if (ChartManager.ActiveFrame is not ChartFrame chartFrame)
                return;

            _chartRobotsControl.Chart = chartFrame.Chart;
        }
    }        
}