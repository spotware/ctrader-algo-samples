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
    public class NormalizingVolumeSample : Indicator
    {
        [Parameter("Volume Unit", DefaultValue = VolumeUnit.Units)]
        public VolumeUnit VolumeUnit { get; set; }

        [Parameter("Volume Amount", DefaultValue = 0.01)]
        public double VolumeAmount { get; set; }

        [Parameter("Rounding Mode", DefaultValue = RoundingMode.ToNearest)]
        public RoundingMode RoundingMode { get; set; }

        protected override void Initialize()
        {
            double volumeInUnits = VolumeUnit == VolumeUnit.Units ? VolumeAmount : Symbol.QuantityToVolumeInUnits(VolumeAmount);

            double normalizedVolume = Symbol.NormalizeVolumeInUnits(volumeInUnits, RoundingMode);

            Print(normalizedVolume);
        }

        public override void Calculate(int index)
        {
        }
    }

    public enum VolumeUnit
    {
        Units,
        Lots
    }
}
