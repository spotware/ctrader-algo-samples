using cAlgo.API;

namespace cAlgo
{

    // This example shows how to use the Chart ChartDragEventArgs
    // ChartDragEventArgs derives from ChartMouseEventArgs
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
