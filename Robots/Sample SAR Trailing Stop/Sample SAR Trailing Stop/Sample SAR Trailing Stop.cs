// -------------------------------------------------------------------------------------------------
//
//    This code is a cTrader Algo API example.
//
//    This cBot is intended to be used as a sample and does not guarantee any particular outcome or
//    profit of any kind. Use it at your own risk.
//    
//    All changes to this file might be lost on the next application update.
//    If you are going to modify this file please make a copy using the "Duplicate" command.
//
//    The "Sample SAR Trailing Stop" will create a market Buy order if the parabolic SAR of the previous bar is 
//    below the candlestick. A Sell order will be created if the parabolic SAR of the previous bar is above the candlestick.  
//    The order's volume is specified in the "Volume" parameter. The order will have a trailing stop defined by the 
//    previous periods' Parabolic SAR levels. The user can change the Parabolic SAR settings by adjusting the "MinAF" 
//    and "MaxAF" parameters.
//
// -------------------------------------------------------------------------------------------------

using cAlgo.API;
using cAlgo.API.Indicators;

namespace cAlgo
{
    [Robot(TimeZone = TimeZones.UTC, AccessRights = AccessRights.None, AddIndicators = true)]
    public class SampleSARTrailingStop : Robot
    {
        [Parameter("Quantity (Lots)", Group = "Volume", DefaultValue = 1, MinValue = 0.01, Step = 0.01)]
        public double Quantity { get; set; }

        [Parameter("Min AF", Group = "Parabolic SAR", DefaultValue = 0.02, MinValue = 0)]
        public double MinAF { get; set; }

        [Parameter("Max AF", Group = "Parabolic SAR", DefaultValue = 0.2, MinValue = 0)]
        public double MaxAF { get; set; }

        private ParabolicSAR parabolicSAR;

        protected override void OnStart()
        {
            parabolicSAR = Indicators.ParabolicSAR(MinAF, MaxAF);
            var tradeType = parabolicSAR.Result.LastValue < Bid ? TradeType.Buy : TradeType.Sell;
            Print("Trade type is {0}, Parabolic SAR is {1}, Bid is {2}", tradeType, parabolicSAR.Result.LastValue, Bid);

            var volumeInUnits = Symbol.QuantityToVolumeInUnits(Quantity);
            ExecuteMarketOrder(tradeType, SymbolName, volumeInUnits, "PSAR TrailingStops");
        }

        protected override void OnTick()
        {
            var position = Positions.Find("PSAR TrailingStops", SymbolName);

            if (position == null)
            {
                Stop();
            }
            else
            {
                double newStopLoss = parabolicSAR.Result.LastValue;
                bool isProtected = position.StopLoss.HasValue;

                if (position.TradeType == TradeType.Buy && isProtected)
                {
                    if (newStopLoss > Bid)
                        return;
                    if (newStopLoss - position.StopLoss < Symbol.TickSize)
                        return;
                }

                if (position.TradeType == TradeType.Sell && isProtected)
                {
                    if (newStopLoss < Bid)
                        return;
                    if (position.StopLoss - newStopLoss < Symbol.TickSize)
                        return;
                }

                ModifyPosition(position, newStopLoss, null);
            }
        }
    }
}
