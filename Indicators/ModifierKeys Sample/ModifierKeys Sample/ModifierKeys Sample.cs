using cAlgo.API;

namespace cAlgo
{
    // This sample shows how to use the ModifierKeys
    [Indicator(IsOverlay = true, TimeZone = TimeZones.UTC, AccessRights = AccessRights.None)]
    public class ModifierKeysSample : Indicator
    {
        private double _mouseBarIndex, _mousePrice;

        [Parameter(DefaultValue = Key.R)]
        public Key HotKey { get; set; }

        [Parameter(DefaultValue = ModifierKeys.Control)]
        public ModifierKeys HotKeyModifier { get; set; }

        protected override void Initialize()
        {
            Chart.MouseMove += Chart_MouseMove;
            Chart.MouseEnter += ResetMouseLocation;
            Chart.MouseLeave += ResetMouseLocation;

            ResetMouseLocation(null);

            Chart.AddHotkey(DrawLines, HotKey, HotKeyModifier);
        }

        private void ResetMouseLocation(ChartMouseEventArgs obj)
        {
            _mouseBarIndex = -1;
            _mousePrice = double.NaN;
        }

        private void Chart_MouseMove(ChartMouseEventArgs obj)
        {
            _mouseBarIndex = obj.BarIndex;
            _mousePrice = obj.YValue;
        }

        private void DrawLines()
        {
            if (_mouseBarIndex == -1 || double.IsNaN(_mousePrice)) return;

            Chart.DrawVerticalLine(_mouseBarIndex.ToString(), (int)_mouseBarIndex, Color.Red);
            Chart.DrawHorizontalLine(_mousePrice.ToString(), _mousePrice, Color.Red);
        }

        public override void Calculate(int index)
        {
        }
    }
}