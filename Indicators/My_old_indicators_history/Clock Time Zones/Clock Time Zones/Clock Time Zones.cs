using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using cAlgo.API;
using cAlgo.API.Collections;
using cAlgo.API.Indicators;
using cAlgo.API.Internals;

namespace cAlgo
{
    [Indicator(IsOverlay = true,  AccessRights = AccessRights.None)]
    public class ClockTimeZones : Indicator
    {

        [Parameter("Horizontal Position",  DefaultValue = HorizontalAlignment.Left)]
        public HorizontalAlignment HorizontalPos { get; set; }
        
        [Parameter("Vertical Position",  DefaultValue = VerticalAlignment.Bottom)]
        public VerticalAlignment VerticalPos { get; set; }

        [Parameter("Local Time", Group = "Local Time Zone",  DefaultValue = false)]
        public bool LocaltimeOption { get; set; }
        
        [Parameter("New York Time", Group = "New York Time Zone" , DefaultValue = true)]
        public bool NewyorktimeOption { get; set; }
        
        [Parameter("London Time", Group = "London Time Zone",  DefaultValue = true)]
        public bool LondontimeOption { get; set; }
        
        [Parameter("Frankfurt Time", Group = "Frankfurt Time Zone",  DefaultValue = false)]
        public bool FrankfurttimeOption { get; set; }
        
        [Parameter("Hong Kong Time", Group = "Hong Kong Time Zone",  DefaultValue = false)]
        public bool HongKongtimeOption { get; set; }
        
        [Parameter("Tokyo Time", Group = "Tokyo Time Zone",  DefaultValue = false)]
        public bool TokyotimeOption { get; set; }
        
        [Parameter("Sydney Time", Group = "Sydney Time Zone",  DefaultValue = false)]
        public bool SydneytimeOption { get; set; }
        
        [Parameter("New Zealand Time", Group = "New Zealand Time Zone",  DefaultValue = false)]
        public bool NewZealandtimeOption { get; set; }

        
        public DateTime currentTime = DateTime.Now;
        
        public DateTime utcTime;
        
        public DateTime NewYorkTime;
        
        public DateTime LondonTime;
        
        public DateTime FrankfurtTime;
        
        public DateTime HongKongTime;
        
        public DateTime TokyoTime;
        
        public DateTime SydneyTime;
        
        public DateTime NewZealandTime;
        
        public DateTime LocalTime;
         
        
        public StackPanel NewYorkStack;
        
        public TextBlock newYorkText;
        
        public Border NewYorkBorder;
                
        
        public StackPanel LondonStack;
        
        public TextBlock LondonText;
        
        public Border LondonBorder;
        
               
        public StackPanel FrankfurtStack;
        
        public TextBlock FrankfurtText;
        
        public Border FrankfurtBorder;
        
                
        public StackPanel HongKongStack;
        
        public TextBlock HongKongText;
        
        public Border HongKongBorder;
        
                
        public StackPanel TokyoStack;
        
        public TextBlock TokyoText;
        
        public Border TokyoBorder;
        
        
        public StackPanel SydneyStack;
        
        public TextBlock SydneyText;
        
        public Border SydneyBorder;
            
            
        public StackPanel NewZealandStack;
        
        public TextBlock NewZealandText;
        
        public Border NewZealandBorder;
        
              
        public StackPanel LocalTimeStack;
        
        public TextBlock LocalTimeText;
        
        public Border LocalTimeBorder;
             
        public StackPanel mainStack;
        
        Dictionary<string, Border> SelectedClocks =  new Dictionary<string, Border>();
                
        protected override void Initialize()
        {

            CheckSelectedClocks();
            Timer.TimerTick += OnTimerTick;
            Timer.Start(1);
        }

        public override void Calculate(int index)
        {

        }
        
        private void OnTimerTick()
        {
            currentTime = DateTime.Now;
          
            if(NewyorktimeOption)
            {    
                NewYorkTime = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(currentTime, TimeZoneInfo.Local.Id, "Eastern Standard Time");
                newYorkText.Text = " New York " +  NewYorkTime.ToString("HH:mm:ss");
            }
            if(LondontimeOption)
            {    
                LondonTime = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(currentTime, TimeZoneInfo.Local.Id, "GMT Standard Time");  
                LondonText.Text = "  London " + LondonTime.ToString("HH:mm:ss");
            }
            if(FrankfurttimeOption)
            {    
                FrankfurtTime = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(currentTime, TimeZoneInfo.Local.Id, "W. Europe Standard Time");
                FrankfurtText.Text = "Frankfurt " + FrankfurtTime.ToString("HH:mm:ss");
            }
            if(HongKongtimeOption)
            {  
                HongKongTime = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(currentTime, TimeZoneInfo.Local.Id, "China Standard Time");
                HongKongText.Text = "Hong Kong " + HongKongTime.ToString("HH:mm:ss");
            }
            if(TokyotimeOption)
            {  
                TokyoTime = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(currentTime, TimeZoneInfo.Local.Id, "Tokyo Standard Time");
                TokyoText.Text = "    Tokyo " + TokyoTime.ToString("HH:mm:ss");
            }
            if(SydneytimeOption)
            {  
                SydneyTime = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(currentTime, TimeZoneInfo.Local.Id, "AUS Eastern Standard Time");
                SydneyText.Text = "   Sydney " + SydneyTime.ToString("HH:mm:ss");
            }
            if(NewZealandtimeOption)
            {  
                NewZealandTime = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(currentTime, TimeZoneInfo.Local.Id, "New Zealand Standard Time");
                NewZealandText.Text = "NewZealand " + NewZealandTime.ToString("HH:mm:ss");
            }
            if(LocaltimeOption)
            {  
                LocalTime = currentTime.ToLocalTime();
                LocalTimeText.Text = "Local Time " + LocalTime.ToString("HH:mm:ss");
            }

        }
        public void CheckSelectedClocks()
        {
            if(LocaltimeOption)
            {
                LocalTimeText = new TextBlock()
                {
                    Padding = "15 10 10 10",
                    Text = "Local Time " +  LocalTime.ToString("HH:mm:ss"),
                    ForegroundColor = "White",
                    FontSize = 20,
                    FontWeight = FontWeight.Bold,
                    FontStyle = FontStyle.Normal,
                    
                    FontFamily = "Digital-7"
  
                };

                LocalTimeStack = new StackPanel()
                {
                    BackgroundColor = "Transparent",
            
                    Width = 185,
                    Height = 45,

                };
        
                LocalTimeBorder = new Border()
                {
                    BackgroundColor = "D21C1B21",
                    BorderColor = Color.LightSlateGray,
                    BorderThickness = 1,
                    CornerRadius = 5,
                    Width = 185,
                    Height = 40,
                    HorizontalAlignment = HorizontalAlignment.Left,
                    Margin = "0 0 0 1",
                    Child = LocalTimeStack
 
                }; 
                LocalTimeStack.AddChild(LocalTimeText);

                SelectedClocks.Add("LocalTime", LocalTimeBorder);
            }
            if(NewyorktimeOption)
            {
                newYorkText = new TextBlock()
                {
                    Padding = "15 10 10 10",
                    Text = "New York " +  NewYorkTime.ToString("HH:mm:ss"),
                    ForegroundColor = "White",
                    FontSize = 20,
                    FontWeight = FontWeight.Bold,
                    FontStyle = FontStyle.Normal,
                    FontFamily = "Digital-7"
                };

                NewYorkStack = new StackPanel()
                {
                    BackgroundColor = "Transparent",
                    Width = 185,
                    Height = 45,
                };
        
                NewYorkBorder = new Border()
                {
                    BackgroundColor = "D21C1B21",
                    BorderColor = Color.LightSlateGray,
                    BorderThickness = 1,
                    CornerRadius = 5,
                    Width = 185,
                    Height = 40,
                    VerticalAlignment = VerticalAlignment.Bottom,
                    HorizontalAlignment = HorizontalAlignment.Left,
                    Child = NewYorkStack
                }; 
                NewYorkStack.AddChild(newYorkText);
                
                SelectedClocks.Add("NewYork", NewYorkBorder);
            }
            if(LondontimeOption)
            {
                LondonText = new TextBlock()
                {
                    Padding = "15 10 10 10",
                    Text = "  London " +  LondonTime.ToString("HH:mm:ss"),
                    ForegroundColor = "White",
                    FontSize = 20,
                    FontWeight = FontWeight.Bold,
                    FontStyle = FontStyle.Normal,
                    FontFamily = "Digital-7"
                };

                LondonStack = new StackPanel()
                {
                    BackgroundColor = "Transparent",
                    Width = 185,
                    Height = 40,
                };
        
                LondonBorder = new Border()
                {
                    BackgroundColor = "D21C1B21",
                    BorderColor = Color.LightSlateGray,
                    BorderThickness = 1,
                    CornerRadius = 5,
                    Width = 185,
                    Height = 40,
                    VerticalAlignment = VerticalAlignment.Bottom,
                    HorizontalAlignment = HorizontalAlignment.Left,
                    Child = LondonStack
                };

                LondonStack.AddChild(LondonText);
                SelectedClocks.Add("London", LondonBorder);
            }
            if(FrankfurttimeOption)
            {
                FrankfurtText = new TextBlock()
                {
                    Padding = "14 10 10 10",
                    Text = "Frankfurt " +  FrankfurtTime.ToString("HH:mm:ss"),
                    ForegroundColor = "White",
                    FontSize = 20,
                    FontWeight = FontWeight.Bold,
                    FontStyle = FontStyle.Normal,
                    FontFamily = "Digital-7"
                };

                FrankfurtStack = new StackPanel()
                {
                    BackgroundColor = "Transparent",
                    Width = 185,
                    Height = 40,
                };
                FrankfurtBorder = new Border()
                {
                    BackgroundColor = "D21C1B21",
                    BorderColor = Color.LightSlateGray,
                    BorderThickness = 1,
                    CornerRadius = 5,
                    Width = 185,
                    Height = 40,
                    VerticalAlignment = VerticalAlignment.Bottom,
                    HorizontalAlignment = HorizontalAlignment.Left,
                    Child = FrankfurtStack
                };

                FrankfurtStack.AddChild(FrankfurtText);
                SelectedClocks.Add("Frankfurt", FrankfurtBorder);
            }
            if(HongKongtimeOption)
            {
                HongKongText = new TextBlock()
                {
                    Padding = "12 10 10 10",
                    Text = "Hong Kong " + HongKongTime.ToString("HH:mm:ss"),
                    ForegroundColor = "White",
                    FontSize = 20,
                    FontWeight = FontWeight.Bold,
                    FontStyle = FontStyle.Normal,
                    FontFamily = "Digital-7"
                };

                HongKongStack = new StackPanel()
                {
                    BackgroundColor = "Transparent",
                    Width = 185,
                    Height = 40,
                };
                HongKongBorder = new Border()
                {
                    BackgroundColor = "D21C1B21",
                    BorderColor = Color.LightSlateGray,
                    BorderThickness = 1,
                    CornerRadius = 5,
                    Width = 185,
                    Height = 40,
                    VerticalAlignment = VerticalAlignment.Bottom,
                    HorizontalAlignment = HorizontalAlignment.Left,
                    Child = HongKongStack
                };

                HongKongStack.AddChild(HongKongText);
                 
                SelectedClocks.Add("HongKong", HongKongBorder);
            }
            if(TokyotimeOption)
            {
                TokyoText = new TextBlock()
                {
                    Padding = "10 10 10 10",
                    Text = "    Tokyo " + TokyoTime.ToString("HH:mm:ss"),
                    ForegroundColor = "White",
                    FontSize = 20,
                    FontWeight = FontWeight.Bold,
                    FontStyle = FontStyle.Normal,
                    FontFamily = "Digital-7"
                };

                TokyoStack = new StackPanel()
                {
                    BackgroundColor = "Transparent",
                    Width = 185,
                    Height = 40,
                };
                TokyoBorder = new Border()
                {
                    BackgroundColor = "D21C1B21",
                    BorderColor = Color.LightSlateGray,
                    BorderThickness = 1,
                    CornerRadius = 5,
                    Width = 185,
                    Height = 40,
                    VerticalAlignment = VerticalAlignment.Bottom,
                    HorizontalAlignment = HorizontalAlignment.Left,
                    Child = TokyoStack
                };

                TokyoStack.AddChild(TokyoText);
                SelectedClocks.Add("Tokyo", TokyoBorder);
            }
            if(SydneytimeOption)
            {
                SydneyText = new TextBlock()
                {
                    Padding = "10 10 10 10",
                    Text = " Sydney " + SydneyTime.ToString("HH:mm:ss"),
                    ForegroundColor = "White",
                    FontSize = 20,
                    FontWeight = FontWeight.Bold,
                    FontStyle = FontStyle.Normal,
                    FontFamily = "Digital-7"
                };

                SydneyStack = new StackPanel()
                {
                    BackgroundColor = "Transparent",
                    Width = 185,
                    Height = 40,
                };
                SydneyBorder = new Border()
                {
                    BackgroundColor = "D21C1B21",
                    BorderColor = Color.LightSlateGray,
                    BorderThickness = 1,
                    CornerRadius = 5,
                    Width = 185,
                    Height = 40,
                    VerticalAlignment = VerticalAlignment.Bottom,
                    HorizontalAlignment = HorizontalAlignment.Left,
                    Child = SydneyStack
                };

                SydneyStack.AddChild(SydneyText);   
                SelectedClocks.Add("Sydney", SydneyBorder);
            }
            if(NewZealandtimeOption)
            {
                
                
                NewZealandText = new TextBlock()
                {
                    Padding = "6 10 10 10",
                    Text = "NewZealand " + NewZealandTime.ToString("HH:mm:ss"),
                    ForegroundColor = "White",
                    FontSize = 19,
                    FontWeight = FontWeight.Bold,
                    FontStyle = FontStyle.Normal,
                    FontFamily = "Digital-7"
                };

                NewZealandStack = new StackPanel()
                {
                    BackgroundColor = "Transparent",
                    Width = 185,
                    Height = 40,
                };
                NewZealandBorder = new Border()
                {
                    BackgroundColor = "D21C1B21",
                    BorderColor = Color.LightSlateGray,
                    BorderThickness = 1,
                    CornerRadius = 5,
                    Width = 185,
                    Height = 40,
                    VerticalAlignment = VerticalAlignment.Bottom,
                    HorizontalAlignment = HorizontalAlignment.Left,
                    Child = NewZealandStack
                };

                NewZealandStack.AddChild(NewZealandText);
                SelectedClocks.Add("NewZealand", NewZealandBorder);
            }
            
            
            GenerateClocks();
        }
        
        public void GenerateClocks()
        {
             mainStack = new StackPanel()
            {
                Width = 190,
                HorizontalAlignment = HorizontalPos,
                VerticalAlignment = VerticalPos
            };

            var clockGrid = new Grid(SelectedClocks.Count,1)
            {
                Margin = "2 2 2 10"    
            };
            
            clockGrid.Columns[0].SetWidthInPixels(220);
  
            for(int i = 0; i < SelectedClocks.Count; i++)
            {
                Print(i);
                var item = SelectedClocks.ElementAt(i);
                var obj = item;
                clockGrid.Rows[i].SetHeightInPixels(42);
                clockGrid.AddChild(obj.Value, i, 0);
            }
            
            mainStack.AddChild(clockGrid);
            Chart.AddControl(mainStack);
        }
    }
}