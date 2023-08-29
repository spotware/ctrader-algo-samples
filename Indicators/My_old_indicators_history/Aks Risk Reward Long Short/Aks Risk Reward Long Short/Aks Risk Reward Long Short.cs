using System;
using System.Collections.Generic;
using System.Linq;
using cAlgo.API;

namespace cAlgo
{
    [Indicator(AccessRights = AccessRights.None, IsOverlay = true)]
    public class AksRiskRewardLongShort : Indicator
    {
        [Parameter("Target Color", DefaultValue = "Green")]
        public Color TargetColor { get; set; }

        [Parameter("Stop Color", DefaultValue = "Red")]
        public Color StopColor { get; set; }

        [Parameter("Transparency", DefaultValue = 100)]
        public int Transparent { get; set; }

        [Parameter("Is Filled", DefaultValue = true)]
        public bool IsFilledParam { get; set; }

        [Parameter("Font Size", Group = "Font", DefaultValue = 12)]
        public int FontSizeParam { get; set; }

        [Parameter("Is Bold", Group = "Font", DefaultValue = true)]
        public bool IsFontBoldParam { get; set; }

        [Parameter("Show Labels", Group = "Font", DefaultValue = true)]
        public bool ShowLabelsParam { get; set; }

        [Parameter("Target Text Color", Group = "Font", DefaultValue = "LightGreen")]
        public Color TargetTextColorParam { get; set; }

        [Parameter("Stop Text Color", Group = "Font", DefaultValue = "#FFFF999A")]
        public Color StopTextColorParam { get; set; }

        bool IsEnabled = false, IsLong = false;
        
        List<LongShortPositions> LspDrawings = new();
        Button BtnLong, BtnShort;
        readonly Color BtnDisabledColor = Color.FromArgb(120, Color.Red);
        readonly Color BtnEnabledColor = Color.FromArgb(120, Color.Green);
        
        public static Color TargetFillColor = Color.FromArgb(40, Color.Green);
        public static Color StopFillColor = Color.FromArgb(40, Color.Red);
        public static Color TargetTextColor, StopTextColor;
        public static int FontSize = 8;
        public static bool IsFontBold = true, IsFilled = true, ShowLabels = true;

        protected override void Initialize()
        {
            FontSize = FontSizeParam;
            IsFontBold = IsFontBoldParam;
            IsFilled = IsFilledParam;
            ShowLabels = ShowLabelsParam;
            TargetFillColor = TargetColor.A == 255 ? Color.FromArgb(Transparent, TargetColor) : TargetColor;
            StopFillColor = StopColor.A == 255 ? Color.FromArgb(Transparent, StopColor) : StopColor;
            TargetTextColor = TargetTextColorParam;
            StopTextColor = StopTextColorParam;

            StackPanel mainPanel = new();
            var grid = new Grid(2, 1);

            BtnLong = NewButton("LP");
            BtnShort = NewButton("SP");

            BtnLong.Click += (obj) => BtnClicked(obj, true);
            BtnShort.Click += (obj) => BtnClicked(obj, false);

            grid.AddChild(BtnLong, 0, 0);
            grid.AddChild(BtnShort, 1, 0);
            mainPanel.HorizontalAlignment = HorizontalAlignment.Left;
            mainPanel.VerticalAlignment = VerticalAlignment.Center;
            mainPanel.AddChild(grid);
            Chart.AddControl(mainPanel);

            Chart.MouseDown += Chart_MouseDown;
            Chart.ObjectsUpdated += Chart_ObjectsUpdated;
            Chart.ObjectsRemoved += Chart_ObjectsRemoved;

            RestoreDrawings();
        }

        private void BtnClicked(ButtonClickEventArgs obj, bool  isLong)
        {
            IsLong = isLong;
            IsEnabled = true;
            BtnLong.BackgroundColor = BtnDisabledColor;
            BtnShort.BackgroundColor = BtnDisabledColor;
            obj.Button.BackgroundColor = BtnEnabledColor;
        }

        private void Chart_ObjectsRemoved(ChartObjectsRemovedEventArgs obj)
        {
            bool isRemoved = false;
            foreach (var cobj in obj.ChartObjects)
            {
                string Oname = cobj.Name;
                if (Oname.StartsWith("LSP") && cobj.ObjectType == ChartObjectType.Rectangle)
                {
                    var d = LspDrawings.Find(d => Oname.StartsWith(d.ObjName));
                    d?.Remove();
                    LspDrawings.Remove(d);
                    isRemoved = true;
                }
            }

            if(isRemoved)
                UpdateStorage();
        }

        private void Chart_ObjectsUpdated(ChartObjectsUpdatedEventArgs obj)
        {
            try
            {
                bool isUpdated = false;
                foreach (var cobj in obj.ChartObjects)
                {
                    string Oname = cobj.Name;
                    if (Oname.StartsWith("LSP") && cobj.ObjectType == ChartObjectType.Rectangle)
                    {
                        var d = LspDrawings.Find(d => Oname.StartsWith(d.ObjName));
                        d?.Update((ChartRectangle)cobj);
                        isUpdated = true;
                    }
                }

                if (isUpdated)
                    UpdateStorage();

            } catch (Exception e)
            {
                Print(e.Message + "---> " + e.StackTrace);
            }
        }

        private void Chart_MouseDown(ChartMouseEventArgs obj)
        {
            if (IsEnabled) 
            {
                IsEnabled = false;

                var y = obj.YValue;
                try
                {
                    int Pips = (int)Math.Max(Math.Floor((Chart.TopY - Chart.BottomY) / Symbol.PipSize / 9), 10);
                    double defaultSize = Pips * Symbol.PipSize;

                    int NoOfBars = Chart.MaxVisibleBars / 9;
                    var d = new LongShortPositions(Print, Chart, IsLong, 
                        obj.TimeValue, obj.TimeValue + (Bars[1].OpenTime - Bars[0].OpenTime) * NoOfBars, 
                        y, y + defaultSize, y - defaultSize);

                    LspDrawings.Add(d);
                    UpdateStorage();
                }
                catch (Exception e)
                {
                    Print(e.StackTrace);
                }

                BtnLong.BackgroundColor = BtnDisabledColor;
                BtnShort.BackgroundColor = BtnDisabledColor;
            }
        }

        Button NewButton(string text)
        {
            return new Button
            {
                Text = text,
                FontSize = 8,
                Margin = "1, 1, 1, 1",
                BackgroundColor = BtnDisabledColor
            };
        }

        void UpdateStorage()
        {
            string lspData = "";
            LspDrawings.ForEach(d => lspData += $"{d}||");
            LocalStorage.SetString("LSP", lspData);
            LocalStorage.Flush(LocalStorageScope.Instance);
            Print(lspData.Replace("||", "\n"));
        }

        void RestoreDrawings()
        {
            string lspData = LocalStorage.GetString("LSP").Trim();
            var lspItems = lspData?.Split("||");
            Print("Restore Drawings: " + (lspItems.Length - 1));

            foreach (var item in lspItems)
            {
                if (!String.IsNullOrWhiteSpace(item))
                {
                    var d = LongShortPositions.GetFromString(Print, Chart, item);
                    if (d is not null)
                        LspDrawings.Add(d);
                }
            }
        }

        public override void Calculate(int index) {}

        protected override void OnDestroy() => LspDrawings.ForEach(d => d.Remove());
    }

    public class LongShortPositions
    {
        ChartRectangle Upper, Lower;
        ChartText Utext, Ltext, Ctext;
        double LastUY1, LastUY2, LastLY1, LastLY2;
        DateTime LastUTime1, LastUTime2, LastLTime1, LastLTime2;

        readonly Chart _chart;
        readonly public string ObjName;
        Action<object> Print;

        double UpperRange, LowerRange, RR = 1;
        readonly bool IsLong;
        bool IsShort { get => !IsLong; }

        public static LongShortPositions GetFromString(Action<object> print, Chart chart, string LspStr)
        {
            LspStr = LspStr.Trim().Trim('|');

            if (!String.IsNullOrEmpty(LspStr))
            {
                 //"type, open time, close time, open, target, stop, rr";

                var values = LspStr.Split(",");
                string symbol = values[0].Trim();
                bool isLong = values[1].Trim().Equals("Long");
                DateTime startTime = DateTime.Parse(values[2].Trim());
                DateTime endTime = DateTime.Parse(values[3].Trim());
                double positionY = Double.Parse(values[4].Trim());
                double upperY = isLong ? Double.Parse(values[5].Trim()) : Double.Parse(values[6].Trim());
                double lowerY = isLong ? Double.Parse(values[6].Trim()) : Double.Parse(values[5].Trim());

                if (symbol.Equals(chart.Symbol.Name))
                    return new LongShortPositions(print, chart, isLong, startTime, endTime, positionY, upperY, lowerY);
                else return null;
            }
            else return null;
        }

        public LongShortPositions(Action<object> print, Chart chart, bool isLong, DateTime startTime, DateTime endTime, double y, double upperY, double lowerY)
        {
            _chart = chart;
            IsLong = isLong;
            this.Print = print;

            ObjName = "LSP_" + Guid.NewGuid().ToString();
            DateTime midTime = startTime.Add((endTime - startTime) / 2);

            // Upper Rectangle
            Upper = _chart.DrawRectangle(ObjName + "_U", startTime, y, endTime, upperY, 
                IsLong ? AksRiskRewardLongShort.TargetFillColor : AksRiskRewardLongShort.StopFillColor);
            (Upper.IsFilled, Upper.IsInteractive) = (AksRiskRewardLongShort.IsFilled, true);
            (UpperRange, LastUY1, LastUY2) = (Math.Abs(Upper.Y1 - Upper.Y2), Upper.Y1, Upper.Y2);
            (LastUTime1, LastUTime2) = (Upper.Time1, Upper.Time2);

            //Upper Text
            Utext = _chart.DrawText(ObjName + "_UT", "", midTime, upperY, IsLong ? AksRiskRewardLongShort.TargetTextColor : AksRiskRewardLongShort.StopTextColor);
            Utext.VerticalAlignment = VerticalAlignment.Top;
            Utext.HorizontalAlignment = HorizontalAlignment.Right;
            (Utext.FontSize, Utext.IsBold) = (AksRiskRewardLongShort.FontSize, AksRiskRewardLongShort.IsFontBold);

            UpdateUpper(Upper.Time1, Upper.Time2, Upper.Y1, Upper.Y2);

            //Lower Rectangle
            Lower = _chart.DrawRectangle(ObjName + "_L", startTime, y, endTime, lowerY, 
                IsLong ? AksRiskRewardLongShort.StopFillColor : AksRiskRewardLongShort.TargetFillColor);
            (Lower.IsFilled, Lower.IsInteractive) = (AksRiskRewardLongShort.IsFilled, true);
            (LowerRange, LastLY1, LastLY2) = (Math.Abs(Lower.Y1 - Lower.Y2), Lower.Y1, Lower.Y2);
            (LastLTime1, LastLTime2) = (Lower.Time1, Lower.Time2);

            //Lower Text
            Ltext = _chart.DrawText(ObjName + "_LT", "", midTime, lowerY, IsLong ? AksRiskRewardLongShort.StopTextColor : AksRiskRewardLongShort.TargetTextColor);
            Ltext.VerticalAlignment = VerticalAlignment.Bottom;
            Ltext.HorizontalAlignment = HorizontalAlignment.Right;
            (Ltext.FontSize, Ltext.IsBold) = (AksRiskRewardLongShort.FontSize, AksRiskRewardLongShort.IsFontBold);

            UpdateLower(Lower.Time1, Lower.Time2, Lower.Y1, Lower.Y2);

            //RR Text
            Ctext = _chart.DrawText(ObjName + "_CT", "", endTime, y, Color.White);
            Ctext.VerticalAlignment = IsLong ? VerticalAlignment.Top : VerticalAlignment.Bottom;
            Ctext.HorizontalAlignment = HorizontalAlignment.Left;
            (Ctext.FontSize, Ctext.IsBold) = (AksRiskRewardLongShort.FontSize, AksRiskRewardLongShort.IsFontBold);
            RR = Math.Round(IsLong ? UpperRange / LowerRange : LowerRange / UpperRange, 2);
            Ctext.Text = (AksRiskRewardLongShort.ShowLabels ? "R:R " : "") + RR;
        }

        public void Update(ChartRectangle rect)
        {
            if (rect.Name.EndsWith("U"))
            {
                if (rect.Y2 < rect.Y1 || rect.Time1 > rect.Time2)
                    UpdateUpper(LastUTime1, LastUTime2, LastUY1, LastUY2);
                else
                {
                    UpperRange = Math.Abs(rect.Y1 - rect.Y2);
                    LowerRange = Math.Abs(Lower.Y1 - Lower.Y2);
                    UpdateUpper(rect.Time1, rect.Time2, rect.Y1, rect.Y2);
                    UpdateLower(rect.Time1, rect.Time2, rect.Y1, rect.Y1 - LowerRange);
                }
            }
            else if (rect.Name.EndsWith("L"))
            {
                if (rect.Y2 > rect.Y1 || rect.Time1 > rect.Time2)
                    UpdateLower(LastLTime1, LastLTime2, LastLY1, LastLY2);
                else
                {
                    LowerRange = Math.Abs(rect.Y1 - rect.Y2);
                    UpperRange = Math.Abs(Upper.Y1 - Upper.Y2);
                    UpdateLower(rect.Time1, rect.Time2, rect.Y1, rect.Y2);
                    UpdateUpper(rect.Time1, rect.Time2, rect.Y1, rect.Y1 + UpperRange);
                }
            }

            Ctext.Y = Upper.Y1;
            Ctext.Time = Upper.Time2;
            RR = Math.Round(IsLong ? UpperRange / LowerRange : LowerRange / UpperRange, 2);
            Ctext.Text = (AksRiskRewardLongShort.ShowLabels ? "R:R " : "") + RR;
        }

        void UpdateUpper(DateTime time1, DateTime time2, double Y1, double Y2)
        {
            (Upper.Y1, Upper.Y2) = (Math.Round(Y1, _chart.Symbol.Digits), Math.Round(Y2, _chart.Symbol.Digits));
            (Upper.Time1, Upper.Time2) = (time1, time2);
            (LastUY1, LastUY2, LastUTime1, LastUTime2) = (Upper.Y1, Upper.Y2, time1, time2);

            (Utext.Y, Utext.Time) = (Upper.Y2, Upper.Time1);

            if (AksRiskRewardLongShort.ShowLabels)
                Utext.Text = (IsLong ? "Target" : "Stop") + $": {Math.Round(Upper.Y2, _chart.Symbol.Digits)}, Pips: {Math.Round(UpperRange / _chart.Symbol.PipSize, 1)}";
            else
                Utext.Text = $"{Math.Round(Upper.Y2, _chart.Symbol.Digits)}, ({Math.Round(UpperRange / _chart.Symbol.PipSize, 1)})";
        }

        void UpdateLower(DateTime time1, DateTime time2, double Y1, double Y2)
        {
            (Lower.Y1, Lower.Y2) = (Math.Round(Y1, _chart.Symbol.Digits), Math.Round(Y2, _chart.Symbol.Digits));
            (Lower.Time1, Lower.Time2) = (time1, time2);
            (LastLY1, LastLY2, LastLTime1, LastLTime2) = (Lower.Y1, Lower.Y2, time1, time2);

            (Ltext.Y, Ltext.Time) = (Lower.Y2, Lower.Time1);

            if (AksRiskRewardLongShort.ShowLabels)
                Ltext.Text = (IsLong ? "Stop" : "Target") + $": {Math.Round(Lower.Y2, _chart.Symbol.Digits)}, Pips: {Math.Round(LowerRange / _chart.Symbol.PipSize, 1)}";
            else
                Ltext.Text = $"{Math.Round(Lower.Y2, _chart.Symbol.Digits)}, ({Math.Round(LowerRange / _chart.Symbol.PipSize, 1)})";
        }

        public override string ToString()
        {
            return _chart.Symbol.Name + (IsLong ? $",Long, {Upper.Time1}, {Upper.Time2}, {Upper.Y1}, {Upper.Y2}, {Lower.Y2}, {RR}" : 
                $",Short, {Upper.Time1}, {Upper.Time2}, {Upper.Y1}, {Lower.Y2}, {Upper.Y2}, {RR}");
        }

        public void Remove()
        {
            _chart.RemoveObject(Upper.Name);
            _chart.RemoveObject(Lower.Name);
            _chart.RemoveObject(Utext.Name);
            _chart.RemoveObject(Ctext.Name);
            _chart.RemoveObject(Ltext.Name);
        }
    }
}