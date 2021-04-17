using cAlgo.API;
using cAlgo.API.Indicators;

namespace cAlgo.Robots
{
    /// <summary>
    /// This sample cBot shows how to use the Ultimate Oscillator indicator
    /// </summary>
    [Robot(TimeZone = TimeZones.UTC, AccessRights = AccessRights.None)]
    public class UltimateOscillatorSample : Robot
    {
        private double _volumeInUnits;

        private UltimateOscillator _ultimateOscillator;

        [Parameter("Volume (Lots)", DefaultValue = 0.01)]
        public double VolumeInLots { get; set; }

        [Parameter("Stop Loss (Pips)", DefaultValue = 10)]
        public double StopLossInPips { get; set; }

        [Parameter("Take Profit (Pips)", DefaultValue = 10)]
        public double TakeProfitInPips { get; set; }

        [Parameter("Label", DefaultValue = "Sample")]
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

            _ultimateOscillator = Indicators.UltimateOscillator(7, 14, 28);
        }

        protected override void OnBar()
        {
            if (_ultimateOscillator.Result.Last(1) > 70 && _ultimateOscillator.Result.Last(2) < 70)
            {
                ClosePositions(TradeType.Buy);

                ExecuteMarketOrder(TradeType.Sell, SymbolName, _volumeInUnits, Label, StopLossInPips, TakeProfitInPips);
            }
            else if (_ultimateOscillator.Result.Last(1) < 30 && _ultimateOscillator.Result.Last(2) > 30)
            {
                ClosePositions(TradeType.Sell);

                ExecuteMarketOrder(TradeType.Buy, SymbolName, _volumeInUnits, Label, StopLossInPips, TakeProfitInPips);
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