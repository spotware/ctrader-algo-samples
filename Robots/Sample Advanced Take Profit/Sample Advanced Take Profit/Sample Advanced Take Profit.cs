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
// -------------------------------------------------------------------------------------------------

using System;
using System.Linq;
using cAlgo.API;

namespace cAlgo
{
    [Robot(TimeZone = TimeZones.UTC, AccessRights = AccessRights.None)]
    public class SampleAdvancedTakeProfit : Robot
    {
        private const string DefaultPositionIdParameterValue = "PID";

        [Parameter("Position Id", Group = "Position", DefaultValue = DefaultPositionIdParameterValue)]
        public string PositionId { get; set; }

        [Parameter("Enabled", Group = "Take Profit 1", DefaultValue = false)]
        public bool TakeProfit1Enabled { get; set; }

        [Parameter("Pips", Group = "Take Profit 1", DefaultValue = 10)]
        public double TakeProfit1Pips { get; set; }

        [Parameter("Volume", Group = "Take Profit 1", DefaultValue = 1000)]
        public int TakeProfit1Volume { get; set; }

        [Parameter("Enabled", Group = "Take Profit 2", DefaultValue = false)]
        public bool TakeProfit2Enabled { get; set; }

        [Parameter("Pips", Group = "Take Profit 2", DefaultValue = 20)]
        public double TakeProfit2Pips { get; set; }

        [Parameter("Volume", Group = "Take Profit 2", DefaultValue = 2000)]
        public int TakeProfit2Volume { get; set; }

        [Parameter("Enabled", Group = "Take Profit 3", DefaultValue = false)]
        public bool TakeProfit3Enabled { get; set; }

        [Parameter("Pips", Group = "Take Profit 3", DefaultValue = 10)]
        public double TakeProfit3Pips { get; set; }

        [Parameter("Volume", Group = "Take Profit 3", DefaultValue = 3000)]
        public int TakeProfit3Volume { get; set; }

        private TakeProfitLevel[] _levels;

        private SymbolInfo _symbolInfo;

        protected override void OnStart()
        {
            if (PositionId == DefaultPositionIdParameterValue)
                PrintErrorAndStop("You have to specify \"Position Id\" in cBot Parameters");

            var position = FindPositionOrStop();
            _symbolInfo = Symbols.GetSymbolInfo(position.SymbolName);
            _levels = GetTakeProfitLevels();

            ValidateLevels(position);
        }

        private void ValidateLevels(Position position)
        {
            MakeSureAnyLevelEnabled();
            ValidateTotalVolume(position);
            ValidateReachedLevels(position);
            ValidateVolumes();
        }

        private void ValidateVolumes()
        {
            var enabledLevels = _levels.Where(level => level.IsEnabled);
            foreach (var level in enabledLevels)
            {
                if (level.Volume < _symbolInfo.VolumeInUnitsMin)
                    PrintErrorAndStop("Volume for " + _symbolInfo.Name + " cannot be less than " + _symbolInfo.VolumeInUnitsMin);
                if (level.Volume > _symbolInfo.VolumeInUnitsMax)
                    PrintErrorAndStop("Volume for " + _symbolInfo.Name + " cannot be greater than " + _symbolInfo.VolumeInUnitsMax);
                if (level.Volume % _symbolInfo.VolumeInUnitsMin != 0)
                    PrintErrorAndStop("Volume " + level.Volume + " is invalid");
            }
        }

        private void ValidateReachedLevels(Position position)
        {
            var reachedLevel = _levels.FirstOrDefault(l => l.Pips <= position.Pips);
            if (reachedLevel != null)
                PrintErrorAndStop("Level " + reachedLevel.Name + " is already reached. The amount of Pips must be more than the amount of Pips that the Position is already gaining");
        }

        private void MakeSureAnyLevelEnabled()
        {
            if (_levels.All(level => !level.IsEnabled))
                PrintErrorAndStop("You have to enable at least one \"Take Profit\" in cBot Parameters");
        }

        private void ValidateTotalVolume(Position position)
        {
            var totalVolume = _levels.Where(level => level.IsEnabled).Sum(level => level.Volume);

            if (totalVolume > position.VolumeInUnits)
                PrintErrorAndStop("The sum of all Take Profit respective volumes cannot be larger than the Position's volume");
        }

        private TakeProfitLevel[] GetTakeProfitLevels()
        {
            return new[] 
            {
                new TakeProfitLevel("Take Profit 1", TakeProfit1Enabled, TakeProfit1Pips, TakeProfit1Volume),
                new TakeProfitLevel("Take Profit 2", TakeProfit2Enabled, TakeProfit2Pips, TakeProfit2Volume),
                new TakeProfitLevel("Take Profit 3", TakeProfit3Enabled, TakeProfit3Pips, TakeProfit3Volume)
            };
        }

        private Position FindPositionOrStop()
        {
            var position = Positions.FirstOrDefault(p => "PID" + p.Id == PositionId || p.Id.ToString() == PositionId);
            if (position == null)
                PrintErrorAndStop("Position with Id = " + PositionId + " doesn't exist");

            return position;
        }

        private void PrintErrorAndStop(string errorMessage)
        {
            Print(errorMessage);
            Stop();

            throw new Exception(errorMessage);
        }

        protected override void OnTick()
        {
            var position = FindPositionOrStop();
            var reachedLevels = _levels.Where(level => level.IsEnabled && !level.IsTriggered && level.Pips <= position.Pips);

            foreach (var reachedLevel in reachedLevels)
            {
                reachedLevel.MarkAsTriggered();

                Print("Level \"" + reachedLevel.Name + "\" is reached. Level.Pips: " + reachedLevel.Pips + ", Position.Pips: " + position.Pips + ", Position.Id: " + position.Id);
                var volumeToClose = Math.Min(reachedLevel.Volume, position.VolumeInUnits);
                ClosePosition(position, volumeToClose);

                if (!LastResult.IsSuccessful)
                    Print("Cannot close position, Id: " + position.Id + ", Error: " + LastResult.Error);

                var remainingLevels = _levels.Where(level => level.IsEnabled && !level.IsTriggered);
                if (!remainingLevels.Any())
                {
                    Print("All levels were reached. cBot is stopping...");
                    Stop();
                    return;
                }
            }
        }
    }

    internal class TakeProfitLevel
    {
        public TakeProfitLevel(string name, bool isEnabled, double pips, int volume)
        {
            Name = name;
            IsEnabled = isEnabled;
            Pips = pips;
            Volume = volume;
        }

        public string Name { get; private set; }

        public bool IsEnabled { get; private set; }

        public double Pips { get; private set; }

        public int Volume { get; private set; }

        public bool IsTriggered { get; private set; }

        public void MarkAsTriggered()
        {
            IsTriggered = true;
        }
    }
}
