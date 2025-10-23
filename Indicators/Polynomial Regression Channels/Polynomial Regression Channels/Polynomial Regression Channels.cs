using System;
using cAlgo.API;
using cAlgo.API.Collections;
using cAlgo.API.Indicators;
using cAlgo.API.Internals;

namespace cAlgo
{
    [Indicator(IsOverlay = true, AccessRights = AccessRights.None)]
    public class PolynomialRegressionChannels : Indicator
    {
        private readonly double[,] _ai = new double[10, 10];
        private readonly double[] _b = new double[10];
        private const int I0 = 0;
        private int _n;
        private int _nn;
        private double _sum;
        private readonly double[] _sx = new double[10];
        private readonly double[] _x = new double[10];

        [Parameter(DefaultValue = 3.0, MinValue = 1, MaxValue = 4)]
        public int Degree { get; set; }

        [Parameter(DefaultValue = 120, MinValue = 1, MaxValue = 2000)]
        public int Periods { get; set; }

        [Parameter("Standard Deviation", DefaultValue = 1.62, Step = 0.01)]
        public double StandardDeviation { get; set; }

        [Parameter("Standard Deviation 2", DefaultValue = 2, Step = 0.01)]
        public double StandardDeviation2 { get; set; }

        [Output("PRC", LineColor = "Gray")]
        public IndicatorDataSeries Prc { get; set; }

        [Output("SQH", LineColor = "Red")]
        public IndicatorDataSeries Sqh { get; set; }

        [Output("SQL", LineColor = "Blue")]
        public IndicatorDataSeries Sql { get; set; }

        [Output("SQL2", LineColor = "Blue")]
        public IndicatorDataSeries Sql2 { get; set; }

        [Output("SQH2", LineColor = "Red")]
        public IndicatorDataSeries Sqh2 { get; set; }

        [Parameter(DefaultValue = 0, MinValue = 0, MaxValue = 200)]
        public int Shift { get; set; }

        public override void Calculate(int index)
        {
            _sx[1] = Periods + 1;
            _nn = Degree + 1;

            CalculateSx();
            CalculateSyx(index);
            CalculateMatrix();
            CalculateGaussDeviations(index);
        }

        private void CalculateSx()
        {
            for (var i = 1; i <= _nn * 2 - 2; i++)
            {
                _sum = 0;
                for (_n = I0; _n <= I0 + Periods; _n++)
                    _sum += Math.Pow(_n, i);

                _sx[i + 1] = _sum;
            }
        }

        private void CalculateSyx(int index)
        {
            for (var i = 1; i <= _nn; i++)
            {
                _sum = 0.00000;
                for (_n = I0; _n <= I0 + Periods; _n++)
                    if (i == 1)
                        _sum += Bars.ClosePrices[index - _n];
                    else
                        _sum += Bars.ClosePrices[index - _n] * Math.Pow(_n, i - 1);

                _b[i] = _sum;
            }
        }

        private void CalculateMatrix()
        {
            for (var j = 1; j <= _nn; j++)
                for (var i = 1; i <= _nn; i++)
                {
                    var k = i + j - 1;
                    _ai[i, j] = _sx[k];
                }
        }

        private void CalculateGaussDeviations(int index)
        {
            for (var k = 1; k <= _nn - 1; k++)
            {
                var ll = 0;
                var mm = 0d;
                for (var i = k; i <= _nn; i++)
                    if (Math.Abs(_ai[i, k]) > mm)
                    {
                        mm = Math.Abs(_ai[i, k]);
                        ll = i;
                    }

                if (ll == 0)
                    return;

                if (ll != k)
                {
                    for (var j = 1; j <= _nn; j++)
                    {
                        var temp = _ai[k, j];
                        _ai[k, j] = _ai[ll, j];
                        _ai[ll, j] = temp;
                    }

                    var bk = _b[k];
                    _b[k] = _b[ll];
                    _b[ll] = bk;
                }

                for (var i = k + 1; i <= _nn; i++)
                {
                    var qq = _ai[i, k] / _ai[k, k];
                    for (var j = 1; j <= _nn; j++)
                        if (j == k)
                            _ai[i, j] = 0;
                        else
                            _ai[i, j] = _ai[i, j] - qq * _ai[k, j];

                    _b[i] = _b[i] - qq * _b[k];
                }
            }

            _x[_nn] = _b[_nn] / _ai[_nn, _nn];
            for (var i = _nn - 1; i >= 1; i--)
            {
                var t = 0d;
                for (var j = 1; j <= _nn - i; j++)
                {
                    t += _ai[i, i + j] * _x[i + j];
                    _x[i] = 1 / _ai[i, i] * (_b[i] - t);
                }
            }

            var sq = 0.0;
            var sq2 = 0.0;
            for (var n = I0; n <= I0 + Periods; n++)
            {
                _sum = 0;
                for (var k = 1; k <= Degree; k++)
                    _sum += _x[k + 1] * Math.Pow(n, k);

                Prc[index + Shift - n] = _x[1] + _sum;
                sq += Math.Pow(Bars.ClosePrices[index - n] - Prc[index + Shift - n], 2);
                sq2 += Math.Pow(Bars.ClosePrices[index - n] - Prc[index + Shift - n], 2);
            }

            sq = Math.Sqrt(sq / (Periods + 1)) * StandardDeviation;
            sq2 = Math.Sqrt(sq2 / (Periods + 1)) * StandardDeviation2;
            for (_n = I0; _n <= I0 + Periods; _n++)
            {
                Sqh[index + Shift - _n] = Prc[index + Shift - _n] + sq;
                Sql[index + Shift - _n] = Prc[index + Shift - _n] - sq;
                Sqh2[index + Shift - _n] = Prc[index + Shift - _n] + sq2;
                Sql2[index + Shift - _n] = Prc[index + Shift - _n] - sq2;
            }
        }
    }
}