using System;
using cAlgo.API;
using cAlgo.API.Collections;
using cAlgo.API.Indicators;
using cAlgo.API.Internals;
using System.Collections.Generic;

namespace cAlgo
{
    [Indicator(ScalePrecision = 0, AccessRights = AccessRights.None)]
    public class TradeVolumeIndex : Indicator
    {
        private enum Direction
        {
            Accumulative,
            Distribute,
        }

        private readonly IndicatorState<Direction> _directions = new(() => Direction.Accumulative);

        [Parameter]
        public DataSeries Source { get; set; }

        [Output("Main")]
        public IndicatorDataSeries Result { get; set; }

        public override void Calculate(int index)
        {
            if (index == 0)
            {
                Result[index] = 0;
                return;
            }

            var change = Source[index] - Source[index - 1];

            if (change > Symbol.PipSize)
                _directions[index] = Direction.Accumulative;
            else if (change < -Symbol.PipSize)
                _directions[index] = Direction.Distribute;
            else
                _directions[index] = _directions[index - 1];

            Result[index] = Result[index - 1];
            if (_directions[index] == Direction.Accumulative)
                Result[index] += Bars.TickVolumes[index];
            else
                Result[index] -= Bars.TickVolumes[index];
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