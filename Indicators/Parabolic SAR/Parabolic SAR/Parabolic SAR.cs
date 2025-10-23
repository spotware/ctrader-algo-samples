using System;
using cAlgo.API;
using cAlgo.API.Collections;
using cAlgo.API.Indicators;
using cAlgo.API.Internals;
using System.Linq;
using System.Collections.Generic;

namespace cAlgo
{
    [Indicator(ScalePrecision = 2, IsOverlay = true, AutoRescale = false, AccessRights = AccessRights.None)]
    public class ParabolicSAR : Indicator
    {
        private readonly IndicatorState<SarIndexState> _states = new(() => new SarIndexState());

        [Parameter("Min AF", DefaultValue = 0.02, MinValue = 0)]
        public double MinAf { get; set; }

        [Parameter("Max AF", DefaultValue = 0.2, MinValue = 0)]
        public double MaxAf { get; set; }

        [Parameter(DefaultValue = 0, MinValue = -1000, MaxValue = 1000)]
        public int Shift { get; set; }

        [Output("Main", LineColor = "Green", LineStyle = LineStyle.Dots, Thickness = 3, PlotType = PlotType.Points)]
        public IndicatorDataSeries Result { get; set; }

        public override void Calculate(int index)
        {
            var currentHigh = Bars.HighPrices[index];
            var currentLow = Bars.LowPrices[index];

            var previousHigh = Bars.HighPrices[index - 1];
            var previousLow = Bars.LowPrices[index - 1];

            if (index == 1)
            {
                var isUpTrend = currentHigh > previousHigh && currentLow > previousLow;
                if (isUpTrend)
                    SetupInitialValue(index, previousLow, true, previousHigh);
                else
                    SetupInitialValue(index, previousHigh, false, previousLow);
            }

            var currentSar = CalculateCurrentSar(index);
            var previousState = _states[index - 1];
            var currentState = _states[index];

            CopyState(previousState, currentState);

            if (previousState.IsUpTrend)
            {
                currentSar = GetMin(currentSar, Bars.LowPrices[index - 1], Bars.LowPrices[index - 2]);

                if (currentSar > currentLow)
                {
                    currentState.IsUpTrend = false;
                    currentSar = previousState.ExtremePoint;
                    currentState.AccelerationFactor = MinAf;
                    currentState.ExtremePoint = currentLow;
                }
                else
                {
                    if (currentHigh > previousState.ExtremePoint)
                    {
                        currentState.ExtremePoint = currentHigh;
                        currentState.AccelerationFactor = IncreaseAccelerationFactor(previousState.AccelerationFactor);
                    }
                }
            }
            else
            {
                currentSar = GetMax(currentSar, Bars.HighPrices[index - 1], Bars.HighPrices[index - 2]);

                if (currentHigh > currentSar)
                {
                    currentState.IsUpTrend = true;
                    currentSar = previousState.ExtremePoint;
                    currentState.AccelerationFactor = MinAf;
                    currentState.ExtremePoint = currentHigh;
                }
                else
                {
                    if (currentLow < previousState.ExtremePoint)
                    {
                        currentState.ExtremePoint = currentLow;
                        currentState.AccelerationFactor = IncreaseAccelerationFactor(previousState.AccelerationFactor);
                    }
                }
            }

            Result[index + Shift] = currentSar;
        }

        private double GetMin(params double[] values)
        {
            var notNaNDoubles = values.Where(d => !double.IsNaN(d)).ToArray();
            return notNaNDoubles.Length > 0 ? notNaNDoubles.Min() : double.NaN;
        }

        private double GetMax(params double[] values)
        {
            var notNaNDoubles = values.Where(d => !double.IsNaN(d)).ToArray();
            return notNaNDoubles.Length > 0 ? notNaNDoubles.Max() : double.NaN;
        }

        private static void CopyState(SarIndexState from, SarIndexState to)
        {
            to.ExtremePoint = from.ExtremePoint;
            to.IsUpTrend = from.IsUpTrend;
            to.AccelerationFactor = from.AccelerationFactor;
        }

        private void SetupInitialValue(int index, double initialSar, bool isUpTrend, double initialExtremePoint)
        {
            Result[index + Shift] = initialSar;
            Result[index + Shift - 1] = initialSar;

            var currentState = _states[index];
            currentState.IsUpTrend = isUpTrend;
            currentState.ExtremePoint = initialExtremePoint;
            currentState.AccelerationFactor = MinAf;

            var previousState = _states[index - 1];
            previousState.ExtremePoint = initialExtremePoint;
            previousState.AccelerationFactor = MinAf;
            previousState.IsUpTrend = isUpTrend;
        }

        private double CalculateCurrentSar(int index)
        {
            var previousState = _states[index - 1];
            var previousSar = Result[index + Shift - 1];

            var currentSar = previousSar + previousState.AccelerationFactor * (previousState.ExtremePoint - previousSar);
            return currentSar;
        }

        private double IncreaseAccelerationFactor(double accelerationFactor)
        {
            var increasedAccelerationFactor = accelerationFactor + MinAf;
            if (increasedAccelerationFactor > MaxAf)
                return accelerationFactor;

            return increasedAccelerationFactor;
        }

        private class SarIndexState
        {
            public double ExtremePoint { get; set; }
            public double AccelerationFactor { get; set; }
            public bool IsUpTrend { get; set; }
        }

        private class IndicatorState<T>
        {
            private readonly Func<T> _defaultValueFactory;

            private readonly List<T> _list = new();

            public IndicatorState(Func<T> defaultValueFactory)
            {
                _defaultValueFactory = defaultValueFactory;
            }

            public T this[int index]
            {
                get
                {
                    if (index < 0)
                        return _defaultValueFactory();

                    FillListWithDefaultValues(index + 1);

                    return _list[index];
                }
                set
                {
                    if (index < 0)
                        return;

                    FillListWithDefaultValues(index + 1);

                    _list[index] = value;
                }
            }

            private void FillListWithDefaultValues(int desiredCount)
            {
                while (_list.Count < desiredCount)
                    _list.Add(_defaultValueFactory());
            }
        }
    }
}