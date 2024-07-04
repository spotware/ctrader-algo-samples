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
    public class ParameterAttributreSample : Indicator
    {
        [Parameter("First Parameter Name", DefaultValue = 0.0, MinValue = 0, MaxValue = 10, Step = 1, Group = "Numeric Group")]
        public double FirstNumericParameter { get; set; }

        [Parameter("Second Parameter Name", DefaultValue = 0.0, MinValue = 0, MaxValue = 100, Step = 1, Group = "Numeric Group")]
        public int SecondNumericParameter { get; set; }

        [Parameter("First Parameter Name", DefaultValue = "Default value", Group = "String Group")]
        public string FirstStringParameter { get; set; }

        [Parameter("Second Parameter Name", DefaultValue = "Default value", Group = "String Group")]
        public string SecondStringParameter { get; set; }

        [Parameter("First Parameter Name", DefaultValue = TradeType.Buy, Group = "Enum Group")]
        public TradeType FirstEnumParameter { get; set; }

        [Parameter("Second Parameter Name", DefaultValue = TradeType.Sell, Group = "Enum Group")]
        public TradeType SecondEnumParameter { get; set; }

        protected override void Initialize()
        {
        }

        public override void Calculate(int index)
        {
        }
    }
}
