using cAlgo.API;

namespace cAlgo
{
    // This example shows how to use the Chart ChartKeyboardEventArgs
    [Indicator(IsOverlay = true, TimeZone = TimeZones.UTC, AccessRights = AccessRights.None)]
    public class ChartKeyboardEventArgsSample : Indicator
    {
        protected override void Initialize()
        {
            Chart.KeyDown += Chart_KeyDown;
        }

        private void Chart_KeyDown(ChartKeyboardEventArgs obj)
        {
            Print("Key: {0} | Modifier Key: {1}", obj.Key, obj.Modifiers);
        }

        public override void Calculate(int index)
        {
        }
    }
}
