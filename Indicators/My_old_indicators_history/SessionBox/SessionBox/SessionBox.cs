using System;
using System.Collections.Generic;
using cAlgo.API;
using cAlgo.API.Internals;
using cAlgo.API.Indicators;
using cAlgo.Indicators;

namespace cAlgo
{
    [Indicator(IsOverlay = true, TimeZone = TimeZones.UTC, AccessRights = AccessRights.None)]
    public class SessionBox : Indicator
    {

        [Parameter(DefaultValue = "22:00")]
        public string asia { get; set; }
        [Parameter(DefaultValue = 12)]
        public int asiaHour { get; set; }
        [Parameter(DefaultValue = "08:00")]
        public string euro { get; set; }
        [Parameter(DefaultValue = 9)]
        public int euroHour { get; set; }
        [Parameter(DefaultValue = "13:00")]
        public string us { get; set; }
        [Parameter(DefaultValue = 10)]
        public int usHour { get; set; }

        // store last Session Start and Session End
        public DateTime asStart;
        public DateTime asEnd;
        public DateTime euStart;
        public DateTime euEnd;
        public DateTime usStart;
        public DateTime usEnd;

        protected override void Initialize()
        {
            // Initialize and create nested indicators
        }

        public override void Calculate(int index)
        {
            var dateTime = MarketSeries.OpenTime[index];
            List<Box> boxs = sbox(index);
            for (int i = 0; i < boxs.Count; i++)
            {
                var box = boxs[i];
                double[] high_low = boxHighLow(index, box);
                drawBox(box.label, box.left, box.right, high_low[0], high_low[1], box.clr);
            }

        }

        // box calcuate logic
        public List<Box> sbox(int index)
        {
            List<Box> boxs = new List<Box>();
            DateTime current = MarketSeries.OpenTime[index];
            string asStartHour = asia.Split('-')[0].Split(':')[0];
            string asStartMinute = asia.Split('-')[0].Split(':')[1];

            string euroStartHour = euro.Split('-')[0].Split(':')[0];
            string euroStartMinute = euro.Split('-')[0].Split(':')[1];

            string usStartHour = us.Split('-')[0].Split(':')[0];
            string usStartMinute = us.Split('-')[0].Split(':')[1];

            if (current.Hour == Int32.Parse(asStartHour) && current.Minute == Int32.Parse(asStartMinute))
            {
                asStart = current;
                asEnd = current.AddHours(asiaHour);
            }
            if (current.Hour == Int32.Parse(euroStartHour) && current.Minute == Int32.Parse(euroStartMinute))
            {
                euStart = current;
                euEnd = current.AddHours(euroHour);
            }
            if (current.Hour == Int32.Parse(usStartHour) && current.Minute == Int32.Parse(usStartMinute))
            {
                usStart = current;
                usEnd = current.AddHours(usHour);
            }

            if (current >= asStart && current <= asEnd)
            {
                boxs.Add(new Box(asStart.ToString(), asStart, asEnd, Colors.Green));
            }

            if (current >= euStart && current <= euEnd)
            {
                boxs.Add(new Box(euStart.ToString(), euStart, euEnd, Colors.Red));
            }
            if (current >= usStart && current <= usEnd)
            {
                boxs.Add(new Box(usStart.ToString(), usStart, usEnd, Colors.Yellow));
            }
            return boxs;



        }

        // calculate session High Low
        private double[] boxHighLow(int index, Box box)
        {
            DateTime left = box.left;
            double[] high_low = new double[2] 
            {
                MarketSeries.High[index],
                MarketSeries.Low[index]
            };
            while (MarketSeries.OpenTime[index] >= left)
            {
                high_low[0] = Math.Max(high_low[0], MarketSeries.High[index]);
                high_low[1] = Math.Min(high_low[1], MarketSeries.Low[index]);
                index--;
            }
            return high_low;
        }

        // draw session box
        private void drawBox(String label, DateTime left, DateTime right, Double high, Double low, Colors clr)
        {
            ChartObjects.DrawLine(label + "_low", left, low, right, low, clr);
            ChartObjects.DrawLine(label + "_high", left, high, right, high, clr);
            ChartObjects.DrawLine(label + "_left", left, high, left, low, clr);
            ChartObjects.DrawLine(label + "_right", right, high, right, low, clr);
        }

        // box data struct
        public struct Box
        {
            public string label;
            public DateTime left;
            public DateTime right;
            public Colors clr;

            public Box(string label, DateTime left, DateTime right, Colors clr)
            {
                this.label = label;
                this.left = left;
                this.right = right;
                this.clr = clr;
            }
        }
    }
}
