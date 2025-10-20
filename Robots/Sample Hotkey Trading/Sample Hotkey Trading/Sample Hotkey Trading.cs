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
//    The "Sample Hotkey Trading" allows you to execute trades by using hotkeys.
//
// -------------------------------------------------------------------------------------------------

using System;
using System.Linq;
using cAlgo.API;

namespace cAlgo
{
    [Robot(TimeZone = TimeZones.UTC, AccessRights = AccessRights.None)]
    public class SampleHotkeyTrading : Robot
    {
        [Parameter("Buy", DefaultValue = "Ctrl + Up", Group = "Trading Hotkeys")]
        public string BuyKeys { get; set; }

        [Parameter("Sell", DefaultValue = "Ctrl + Down", Group = "Trading Hotkeys")]
        public string SellKeys { get; set; }

        [Parameter("Close Current Symbol", DefaultValue = "Ctrl + Space", Group = "Trading Hotkeys")]
        public string CloseCurrentSymbolKeys { get; set; }

        [Parameter("Close All", DefaultValue = "Ctrl + Shift + Space", Group = "Trading Hotkeys")]
        public string CloseAllKeys { get; set; }


        [Parameter("Increase small step", DefaultValue = "Ctrl + PageUp", Group = "Volume Hotkeys")]
        public string VolumeIncreaseSmallStepKeys { get; set; }

        [Parameter("Decrease small step", DefaultValue = "Ctrl + PageDown", Group = "Volume Hotkeys")]
        public string VolumeDecreaseSMallStepKeys { get; set; }

        [Parameter("Increase big step", DefaultValue = "Ctrl + Shift + PageUp", Group = "Volume Hotkeys")]
        public string VolumeIncreaseBigStepKeys { get; set; }

        [Parameter("Decrease big step", DefaultValue = "Ctrl + Shift + PageDown", Group = "Volume Hotkeys")]
        public string VolumeDecreaseBigStepKeys { get; set; }


        [Parameter("Default Volume", DefaultValue = 1000, Group = "Volume Steps")]
        public double DefaultVolume { get; set; }

        [Parameter("Small step", DefaultValue = 1000, Group = "Volume Steps")]
        public double VolumeSmallStep { get; set; }

        [Parameter("Big step", DefaultValue = 10000, Group = "Volume Steps")]
        public double VolumeBigStep { get; set; }


        private double CurrentVolume;

        protected override void OnStart()
        {
            CurrentVolume = DefaultVolume;
            UpdateVolumeLabel();

            Chart.AddHotkey(Buy, BuyKeys);
            Chart.AddHotkey(Sell, SellKeys);
            Chart.AddHotkey(ClosePositionForCurrentSymbol, CloseCurrentSymbolKeys);
            Chart.AddHotkey(CloseAllPositions, CloseAllKeys);

            Chart.AddHotkey(() => IncreaseVolume(VolumeSmallStep), VolumeIncreaseSmallStepKeys);
            Chart.AddHotkey(() => IncreaseVolume(VolumeBigStep), VolumeIncreaseBigStepKeys);
            Chart.AddHotkey(() => DecreaseVolume(VolumeSmallStep), VolumeDecreaseSMallStepKeys);
            Chart.AddHotkey(() => DecreaseVolume(VolumeBigStep), VolumeDecreaseBigStepKeys);
        }

        private void Buy()
        {
            Print("Hotkey triggered: Buy");
            ExecuteMarketOrder(TradeType.Buy, SymbolName, CurrentVolume);
        }

        private void Sell()
        {
            Print("Hotkey triggered: Sell");
            ExecuteMarketOrder(TradeType.Sell, SymbolName, CurrentVolume);
        }

        private void ClosePositionForCurrentSymbol()
        {
            Print("Hotkey triggered: Close current symbol positions");
            var currentSymbolPositions = Positions.Where(p => p.SymbolName == SymbolName).ToList();
            currentSymbolPositions.ForEach(p => ClosePositionAsync(p));
        }

        private void CloseAllPositions()
        {
            Print("Hotkey triggered: Close all position");
            Positions.ToList().ForEach(p => ClosePositionAsync(p));
        }

        private void IncreaseVolume(double volumeDelta)
        {
            CurrentVolume = Math.Min(CurrentVolume + volumeDelta, Symbol.VolumeInUnitsMax);
            UpdateVolumeLabel();
        }

        private void DecreaseVolume(double volumeDelta)
        {
            CurrentVolume = Math.Max(CurrentVolume - volumeDelta, Symbol.VolumeInUnitsMin);
            UpdateVolumeLabel();
        }

        private void UpdateVolumeLabel()
        {
            var text = String.Format("Volume {0}", CurrentVolume);
            Chart.DrawStaticText("volume", text, VerticalAlignment.Top, HorizontalAlignment.Left, Chart.ColorSettings.ForegroundColor);
        }
    }
}
