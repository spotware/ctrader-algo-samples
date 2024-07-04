// -------------------------------------------------------------------------------------------------
//
//    This code is a cTrader Algo API example.
//
//    This cBot is intended to be used as a sample and does not guarantee any particular outcome or
//    profit of any kind. Use it at your own risk.
//
// -------------------------------------------------------------------------------------------------

using cAlgo.API;
using cAlgo.API.Indicators;

namespace cAlgo.Robots
{
    [Robot(TimeZone = TimeZones.UTC, AccessRights = AccessRights.None, AddIndicators = true)]
    public class VidyaSample : Robot
    {
        private double _volumeInUnits;

        private Vidya _fastVidya;

        private Vidya _slowVidya;

        [Parameter("Source", Group = "Fast MA")]
        public DataSeries FastMaSource { get; set; }

        [Parameter("Period", DefaultValue = 9, Group = "Fast MA")]
        public int FastMaPeriod { get; set; }

        [Parameter("Source", Group = "Slow MA")]
        public DataSeries SlowMaSource { get; set; }

        [Parameter("Period", DefaultValue = 20, Group = "Slow MA")]
        public int SlowMaPeriod { get; set; }

        [Parameter("Sigma", DefaultValue = 0.65, Group = "Fast MA", MinValue = 0.1, MaxValue = 0.95)]
        public double FastSigma { get; set; }

        [Parameter("Sigma", DefaultValue = 0.6, Group = "Slow MA", MinValue = 0.1, MaxValue = 0.95)]
        public double SlowSigma { get; set; }

        [Parameter("Volume (Lots)", DefaultValue = 0.01, Group = "Trade")]
        public double VolumeInLots { get; set; }

        [Parameter("Stop Loss (Pips)", DefaultValue = 10, MaxValue = 100, MinValue = 1, Step = 1)]
        public double StopLossInPips { get; set; }

        [Parameter("Take Profit (Pips)", DefaultValue = 10, MaxValue = 100, MinValue = 1, Step = 1)]
        public double TakeProfitInPips { get; set; }

        [Parameter("Label", DefaultValue = "VidyaSample", Group = "Trade")]
        public string Label { get; set; }

        public Position[] BotPositions
        {
            get
            {
                return Positions.FindAll(Label);
            }
        }

        protected override void OnStart()
        {
            _volumeInUnits = Symbol.QuantityToVolumeInUnits(VolumeInLots);

            _fastVidya = Indicators.Vidya(FastMaSource, FastMaPeriod, FastSigma);
            _slowVidya = Indicators.Vidya(SlowMaSource, SlowMaPeriod, SlowSigma);

            _fastVidya.Result.Line.Color = Color.Blue;
            _slowVidya.Result.Line.Color = Color.Red;
        }

        protected override void OnBarClosed()
        {
            if (_fastVidya.Result.HasCrossedAbove(_slowVidya.Result, 0))
            {
                ClosePositions(TradeType.Sell);

                ExecuteMarketOrder(TradeType.Buy, SymbolName, _volumeInUnits, Label, StopLossInPips, TakeProfitInPips);
            }
            else if (_fastVidya.Result.HasCrossedBelow(_slowVidya.Result, 0))
            {
                ClosePositions(TradeType.Buy);

                ExecuteMarketOrder(TradeType.Sell, SymbolName, _volumeInUnits, Label, StopLossInPips, TakeProfitInPips);
            }
        }

        private void ClosePositions(TradeType tradeType)
        {
            foreach (var position in BotPositions)
            {
                if (position.TradeType != tradeType) continue;

                ClosePosition(position);
            }
        }
    }
}