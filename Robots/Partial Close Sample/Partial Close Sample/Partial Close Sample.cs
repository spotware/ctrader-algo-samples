// -------------------------------------------------------------------------------------------------
//
//    This code is a cTrader Algo API example.
//
//    This cBot is intended to be used as a sample and does not guarantee any particular outcome or
//    profit of any kind. Use it at your own risk.
//
// -------------------------------------------------------------------------------------------------

using System;
using System.Linq;
using cAlgo.API;
using System.Collections.Generic;

namespace cAlgo.Robots
{
    [Robot(TimeZone = TimeZones.UTC, AccessRights = AccessRights.None)]
    public class PartialCloseSample : Robot
    {
        private readonly List<long> _firstLevelClosedPositions = new List<long>();
        private readonly List<long> _secondLevelClosedPositions = new List<long>();

        [Parameter("Close %", DefaultValue = 20, Group = "First Level")]
        public double FirstLevelCloseAmountInPercentage { get; set; }

        [Parameter("Pips", DefaultValue = 20, Group = "First Level")]
        public double FirstLevelClosePips { get; set; }

        [Parameter("Close %", DefaultValue = 20, Group = "Second Level")]
        public double SecondLevelCloseAmountInPercentage { get; set; }

        [Parameter("Pips", DefaultValue = 35, Group = "Second Level")]
        public double SecondLevelClosePips { get; set; }

        protected override void OnStart()
        {
            FirstLevelCloseAmountInPercentage /= 100;
            SecondLevelCloseAmountInPercentage /= 100;

            Positions.Opened += Positions_Opened;
            Positions.Closed += Positions_Closed;
        }

        private void Positions_Closed(PositionClosedEventArgs obj)
        {
            // In case position closed fully clean it's enteries from ID collections
            if (Positions.Any(position => position.Id == obj.Position.Id) == false)
            {
                if (_firstLevelClosedPositions.Contains(obj.Position.Id))
                {
                    _firstLevelClosedPositions.Remove(obj.Position.Id);
                }

                if (_secondLevelClosedPositions.Contains(obj.Position.Id))
                {
                    _secondLevelClosedPositions.Remove(obj.Position.Id);
                }
            }

            // If there are other positions from same symbol then don't remove the symbol Tick event handler
            if (Positions.Any(position => position.SymbolName.Equals(obj.Position.SymbolName, StringComparison.Ordinal)))
            {
                return;
            }

            // If there is no other position from the closed position symbol then remove the Tick event handler
            var positionSymbol = Symbols.GetSymbol(obj.Position.SymbolName);

            positionSymbol.Tick -= PositionSymbol_Tick;
        }

        private void Positions_Opened(PositionOpenedEventArgs obj)
        {
            // If there are other positions from same symbol then don't add the symbol Tick event handler
            // Because we already have one
            if (Positions.Count(position => position.SymbolName.Equals(obj.Position.SymbolName, StringComparison.Ordinal)) > 1)
            {
                return;
            }

            // Add position symbol tick event handler
            var positionSymbol = Symbols.GetSymbol(obj.Position.SymbolName);

            positionSymbol.Tick += PositionSymbol_Tick;
        }

        private void PositionSymbol_Tick(SymbolTickEventArgs obj)
        {
            var symbolPositions = Positions.Where(position => position.SymbolName.Equals(obj.SymbolName, StringComparison.Ordinal)).ToArray();

            foreach (var position in symbolPositions)
            {
                if (_firstLevelClosedPositions.Contains(position.Id) == false && position.Pips >= FirstLevelClosePips)
                {
                    ClosePositionByVolumePercenatage(position, FirstLevelCloseAmountInPercentage);

                    _firstLevelClosedPositions.Add(position.Id);
                }
                else if (_secondLevelClosedPositions.Contains(position.Id) == false && position.Pips >= SecondLevelClosePips)
                {
                    ClosePositionByVolumePercenatage(position, SecondLevelCloseAmountInPercentage);

                    _secondLevelClosedPositions.Add(position.Id);
                }
            }
        }

        private void ClosePositionByVolumePercenatage(Position position, double volumePercent)
        {
            var symbol = Symbols.GetSymbol(position.SymbolName);

            var volumeToClose = symbol.NormalizeVolumeInUnits(position.VolumeInUnits * volumePercent);

            ClosePosition(position, volumeToClose);
        }
    }
}