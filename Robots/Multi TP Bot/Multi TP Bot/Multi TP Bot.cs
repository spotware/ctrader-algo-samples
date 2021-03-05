using cAlgo.API;
using System;
using System.Collections.Generic;

namespace cAlgo.Robots
{
    /// <summary>
    /// Allows setting multiple take profit levels for positions
    /// Set a take profit volume parameter value to 0 if you don't want to use it
    /// </summary>
    [Robot(TimeZone = TimeZones.UTC, AccessRights = AccessRights.None)]
    public class MultiTPBot : Robot
    {
        private double _firstTakeProfitVolumeInUnits;
        private double _secondTakeProfitVolumeInUnits;
        private double _thirdTakeProfitVolumeInUnits;
        private double _fourthTakeProfitVolumeInUnits;

        private List<long> _firstTakeProfitTriggeredPositionIds = new List<long>();
        private List<long> _secondTakeProfitTriggeredPositionIds = new List<long>();
        private List<long> _thirdTakeProfitTriggeredPositionIds = new List<long>();
        private List<long> _fourthTakeProfitTriggeredPositionIds = new List<long>();

        [Parameter("1st TP", DefaultValue = 0.01, MinValue = 0, Group = "Volume (Lots)")]
        public double FirstTakePrfitVolumeInLots { get; set; }

        [Parameter("2nd TP", DefaultValue = 0.03, MinValue = 0, Group = "Volume (Lots)")]
        public double SecondTakePrfitVolumeInLots { get; set; }

        [Parameter("3rd TP", DefaultValue = 0.05, MinValue = 0, Group = "Volume (Lots)")]
        public double ThirdTakePrfitVolumeInLots { get; set; }

        [Parameter("4th TP", DefaultValue = 0.1, MinValue = 0, Group = "Volume (Lots)")]
        public double FourthTakePrfitVolumeInLots { get; set; }

        [Parameter("1st TP", DefaultValue = 10, MinValue = 0, Group = "Pips")]
        public double FirstTakePrfitPips { get; set; }

        [Parameter("2nd TP", DefaultValue = 20, MinValue = 0, Group = "Pips")]
        public double SecondTakePrfitPips { get; set; }

        [Parameter("3rd TP", DefaultValue = 30, MinValue = 0, Group = "Pips")]
        public double ThirdTakePrfitPips { get; set; }

        [Parameter("4th TP", DefaultValue = 40, MinValue = 0, Group = "Pips")]
        public double FourthTakePrfitPips { get; set; }

        protected override void OnStart()
        {
            _firstTakeProfitVolumeInUnits = Symbol.QuantityToVolumeInUnits(FirstTakePrfitVolumeInLots);
            _secondTakeProfitVolumeInUnits = Symbol.QuantityToVolumeInUnits(SecondTakePrfitVolumeInLots);
            _thirdTakeProfitVolumeInUnits = Symbol.QuantityToVolumeInUnits(ThirdTakePrfitVolumeInLots);
            _fourthTakeProfitVolumeInUnits = Symbol.QuantityToVolumeInUnits(FourthTakePrfitVolumeInLots);
        }

        protected override void OnTick()
        {
            foreach (var position in Positions)
            {
                if (!position.SymbolName.Equals(SymbolName, StringComparison.OrdinalIgnoreCase)) continue;

                TriggerTakeProfitIfReached(position, _firstTakeProfitVolumeInUnits, FirstTakePrfitPips,
                    _firstTakeProfitTriggeredPositionIds);

                TriggerTakeProfitIfReached(position, _secondTakeProfitVolumeInUnits, SecondTakePrfitPips,
                    _secondTakeProfitTriggeredPositionIds);

                TriggerTakeProfitIfReached(position, _thirdTakeProfitVolumeInUnits, ThirdTakePrfitPips,
                    _thirdTakeProfitTriggeredPositionIds);

                TriggerTakeProfitIfReached(position, _fourthTakeProfitVolumeInUnits, FourthTakePrfitPips,
                    _fourthTakeProfitTriggeredPositionIds);
            }
        }

        private void ModifyPositionVolume(Position position, double newVolume)
        {
            if (position.VolumeInUnits > newVolume)
            {
                ModifyPosition(position, newVolume);
            }
            else
            {
                ClosePosition(position);
            }
        }

        private void TriggerTakeProfitIfReached(Position position, double takeProfitVolumeInUnits, double takeProfitPips,
            List<long> triggeredPositionIds)
        {
            if (triggeredPositionIds.Contains(position.Id)) return;

            if (takeProfitVolumeInUnits > 0 && position.Pips >= takeProfitPips)
            {
                triggeredPositionIds.Add(position.Id);

                ModifyPositionVolume(position, position.VolumeInUnits - takeProfitVolumeInUnits);
            }
        }
    }
}