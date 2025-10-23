using System;
using cAlgo.API;
using cAlgo.API.Collections;
using cAlgo.API.Indicators;
using cAlgo.API.Internals;

namespace cAlgo
{
    [Indicator(AccessRights = AccessRights.None)]
    public class GatorOscillator : Indicator
    {
        [Parameter]
        public DataSeries Source { get; set; }

        [Parameter("Jaws Periods", DefaultValue = 13, MinValue = 1, MaxValue = 2000)]
        public int JawsPeriods { get; set; }

        [Parameter("Jaws Shift", DefaultValue = 8, MinValue = 0, MaxValue = 1000)]
        public int JawsShift { get; set; }

        [Parameter("Teeth Periods", DefaultValue = 8, MinValue = 1, MaxValue = 2000)]
        public int TeethPeriods { get; set; }

        [Parameter("Teeth Shift", DefaultValue = 5, MinValue = 0, MaxValue = 1000)]
        public int TeethShift { get; set; }

        [Parameter("Lips Periods", DefaultValue = 5, MinValue = 1, MaxValue = 2000)]
        public int LipsPeriods { get; set; }

        [Parameter("Lips Shift", DefaultValue = 3, MinValue = 0, MaxValue = 1000)]
        public int LipsShift { get; set; }

        [Parameter("MA Type", DefaultValue = MovingAverageType.WilderSmoothing)]
        public MovingAverageType MAType { get; set; }

        [Parameter("Up", DefaultValue = "Green")]
        public Color UpColor { get; set; }

        [Parameter("Down", DefaultValue = "Red")]
        public Color DownColor { get; set; }

        [Output("Upper", PlotType = PlotType.Histogram, IsColorCustomizable = false)]
        public IndicatorDataSeries Upper { get; set; }

        [Output("Lower", PlotType = PlotType.Histogram, IsColorCustomizable = false)]
        public IndicatorDataSeries Lower { get; set; }

        private Alligator _alligator;

        protected override void Initialize()
        {
            Source ??= Indicators.MedianPrice(Bars).Result;
            _alligator = Indicators.Alligator(Source, JawsPeriods, JawsShift, TeethPeriods, TeethShift, LipsPeriods, LipsShift, MAType);
        }

        public override void Calculate(int index)
        {
            Upper[index] = Math.Abs(_alligator.Jaws[index] - _alligator.Teeth[index]);
            Lower[index] = Math.Abs(_alligator.Teeth[index] - _alligator.Lips[index]) * -1;

            SetLineAppearance(Upper.LineOutput, index, Upper[index] > Upper[index - 1] ? UpColor : DownColor);
            SetLineAppearance(Lower.LineOutput, index, Lower[index] < Lower[index - 1] ? UpColor : DownColor);
        }
    }
}