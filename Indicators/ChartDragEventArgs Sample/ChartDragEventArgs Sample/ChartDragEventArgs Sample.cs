// -------------------------------------------------------------------------------------------------
//
//    This code is a cTrader Algo API example.
//
//    This Indicator is intended to be used as a sample and does not guarantee any particular outcome or
//    profit of any kind. Use it at your own risk.
//
// -------------------------------------------------------------------------------------------------

using cAlgo.API;

namespace cAlgo
{
    [Indicator(IsOverlay = true, TimeZone = TimeZones.UTC, AccessRights = AccessRights.None)]
    public class ChartDragEventArgsSample : Indicator
    {
        protected override void Initialize()
        {
            Chart.DragStart += Chart_DragStart;
            Chart.DragEnd += Chart_DragEnd;
        }

        private void Chart_DragEnd(ChartDragEventArgs obj)
        {
            Print("Chart {0} {1} Drag Started | Mouse Location: ({2}, {3})", obj.Chart.SymbolName, obj.Chart.TimeFrame, obj.MouseX, obj.MouseY);
        }

        private void Chart_DragStart(ChartDragEventArgs obj)
        {
            Print("Chart {0} {1} Drag Ended | Mouse Location: ({2}, {3})", obj.Chart.SymbolName, obj.Chart.TimeFrame, obj.MouseX, obj.MouseY);
        }

        public override void Calculate(int index)
        {
        }
    }
}
