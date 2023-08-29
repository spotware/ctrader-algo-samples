using System;
using cAlgo.API;
using cAlgo.API.Internals;
using cAlgo.API.Indicators;
using cAlgo.Indicators;

namespace cAlgo
{
    [Indicator(IsOverlay = true, TimeZone = TimeZones.UTC, AccessRights = AccessRights.None)]
    public class Clock : Indicator
    {
        [Parameter("Margin", DefaultValue = 5)]
        public int ClockMargin { get; set; }

        [Parameter("Opacity", DefaultValue = 1, MaxValue = 1, MinValue = 0)]
        public double ClockOpacity { get; set; }

        [Parameter("Horizontal position", DefaultValue = HPosition.Left)]
        public HPosition ClockHorizontalPosition { get; set; }

        [Parameter("Vertical position", DefaultValue = VPosition.Top)]
        public VPosition ClockVerticalPosition { get; set; }

        private int ClockSizeInPixels = 250;
        private double HoursArrowLength = 55;
        private double MinutesArrowLength = 85;
        private double SecondsArrowLength = 95;
        private Line HoursArrow;
        private Line MinutesArrow;
        private Line SecondsArrow;

        protected override void Initialize()
        {
            var background = new Image 
            {
                Source = Resource1.clock_background,
                Width = ClockSizeInPixels,
                Height = ClockSizeInPixels
            };

            var canvas = new Canvas 
            {
                Width = ClockSizeInPixels,
                Height = ClockSizeInPixels,
                HorizontalAlignment = (HorizontalAlignment)ClockHorizontalPosition,
                VerticalAlignment = (VerticalAlignment)ClockVerticalPosition,
                Margin = ClockMargin,
                Opacity = ClockOpacity,
                IsHitTestVisible = false
            };

            CreateArrows();
            UpdateArrows();

            canvas.AddChild(background);
            canvas.AddChild(SecondsArrow);
            canvas.AddChild(MinutesArrow);
            canvas.AddChild(HoursArrow);
            Chart.AddControl(canvas);

            Timer.TimerTick += Timer_TimerTick;
            Timer.Start(1);
        }

        private void CreateArrows()
        {
            var centerXY = ClockSizeInPixels / 2;

            HoursArrow = new Line 
            {
                StrokeColor = Color.Black,
                X1 = ClockSizeInPixels / 2,
                Y1 = ClockSizeInPixels / 2,
                StrokeThickness = 7,
                StrokeEndLineCap = PenLineCap.Triangle
            };

            MinutesArrow = new Line 
            {
                StrokeColor = Color.Black,
                X1 = ClockSizeInPixels / 2,
                Y1 = ClockSizeInPixels / 2,
                StrokeThickness = 5,
                StrokeEndLineCap = PenLineCap.Triangle
            };

            SecondsArrow = new Line 
            {
                StrokeColor = Color.Red,
                X1 = ClockSizeInPixels / 2,
                Y1 = ClockSizeInPixels / 2,
                StrokeThickness = 3
            };
        }

        private void Timer_TimerTick()
        {
            UpdateArrows();
        }

        private void UpdateArrows()
        {
            double hour = Time.Hour % 12.0 + (Time.Minute + Time.Second / 60.0) / 60.0;
            double minute = Time.Minute + Time.Second / 60.0;
            double sec = Time.Second;
            double hourRadian = hour / 12 * 2 * Math.PI - Math.PI / 2;
            double minRadian = minute / 60 * 2 * Math.PI - Math.PI / 2;
            double secRadian = sec / 60 * 2 * Math.PI - Math.PI / 2;

            double hourX = HoursArrowLength * Math.Cos(hourRadian);
            double hourY = HoursArrowLength * Math.Sin(hourRadian);
            double minuteX = MinutesArrowLength * Math.Cos(minRadian);
            double minuteY = MinutesArrowLength * Math.Sin(minRadian);
            double secondsX = SecondsArrowLength * Math.Cos(secRadian);
            double secondsY = SecondsArrowLength * Math.Sin(secRadian);

            var centerXY = ClockSizeInPixels / 2;
            HoursArrow.X2 = hourX + centerXY;
            HoursArrow.Y2 = hourY + centerXY;
            MinutesArrow.X2 = minuteX + centerXY;
            MinutesArrow.Y2 = minuteY + centerXY;
            SecondsArrow.X2 = secondsX + centerXY;
            SecondsArrow.Y2 = secondsY + centerXY;
        }

        public override void Calculate(int index)
        {
        }
    }

    public enum HPosition
    {
        Left = 1,
        Center = 0,
        Right = 2
    }

    public enum VPosition
    {
        Top = 1,
        Center = 0,
        Bottom = 2
    }
}
