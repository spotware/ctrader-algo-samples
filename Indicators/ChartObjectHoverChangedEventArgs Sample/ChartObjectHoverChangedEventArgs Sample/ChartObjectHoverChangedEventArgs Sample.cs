using cAlgo.API;

namespace cAlgo
{
    // This example shows how to use the Chart ChartObjectHoverChangedEventArgs
    // ChartObjectHoverChangedEventArgs is derived from ChartObjectsEventArgs
    // Draw an object and move mouse over it
    [Indicator(IsOverlay = true, TimeZone = TimeZones.UTC, AccessRights = AccessRights.None)]
    public class ChartObjectHoverChangedEventArgsSample : Indicator
    {
        protected override void Initialize()
        {
            Chart.ObjectHoverChanged += Chart_ObjectHoverChanged;
            ;
        }

        private void Chart_ObjectHoverChanged(ChartObjectHoverChangedEventArgs obj)
        {
            Chart.DrawStaticText("hover", string.Format("Is Object Hovered: {0}", obj.IsObjectHovered), VerticalAlignment.Top, HorizontalAlignment.Right, Color.Red);
        }

        public override void Calculate(int index)
        {
        }
    }
}
