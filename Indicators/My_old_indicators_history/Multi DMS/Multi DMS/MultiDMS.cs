using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using cAlgo.API;
using cAlgo.API.Collections;
using cAlgo.API.Indicators;
using cAlgo.API.Internals;

namespace cAlgo
{
    [Indicator(AccessRights = AccessRights.FullAccess, IsOverlay = true)]
    public class MultiDMS : Indicator
    {
        [Parameter(DefaultValue = 20)]
        public int Periods { get; set; }

        [Parameter(DefaultValue = true, Group = "Time frames")]
        public bool m1 { get; set; }
        
        [Parameter(DefaultValue = false, Group = "Time frames")]
        public bool m2 { get; set; }
        
        [Parameter(DefaultValue = false, Group = "Time frames")]
        public bool m3 { get; set; }
        
        [Parameter(DefaultValue = false, Group = "Time frames")]
        public bool m4 { get; set; }
                
        [Parameter(DefaultValue = true, Group = "Time frames")]
        public bool m5 { get; set; }
                
        [Parameter(DefaultValue = false, Group = "Time frames")]
        public bool m6 { get; set; }
                
        [Parameter(DefaultValue = false, Group = "Time frames")]
        public bool m7 { get; set; }
        
        [Parameter(DefaultValue = false, Group = "Time frames")]
        public bool m8 { get; set; }       
       
        [Parameter(DefaultValue = false, Group = "Time frames")]
        public bool m9 { get; set; }
       
        [Parameter(DefaultValue = true, Group = "Time frames")]
        public bool m10 { get; set; }
               
        [Parameter(DefaultValue = true, Group = "Time frames")]
        public bool m15 { get; set; }
               
        [Parameter(DefaultValue = false, Group = "Time frames")]
        public bool m20 { get; set; }
               
        [Parameter(DefaultValue = true, Group = "Time frames")]
        public bool m30 { get; set; }        
               
        [Parameter(DefaultValue = false, Group = "Time frames")]
        public bool m45 { get; set; }
        
        [Parameter(DefaultValue = true, Group = "Time frames")]
        public bool h1 { get; set; }
        
        [Parameter(DefaultValue = false, Group = "Time frames")]
        public bool h2 { get; set; }
        
        [Parameter(DefaultValue = false, Group = "Time frames")]
        public bool h3 { get; set; }
        
        [Parameter(DefaultValue = false, Group = "Time frames")]
        public bool h4 { get; set; }
        
        [Parameter(DefaultValue = false, Group = "Time frames")]
        public bool h6 { get; set; }
        
        [Parameter(DefaultValue = false, Group = "Time frames")]
        public bool h8 { get; set; }
                
        [Parameter(DefaultValue = false, Group = "Time frames")]
        public bool h12 { get; set; }
                
        [Parameter(DefaultValue = true, Group = "Time frames")]
        public bool D1 { get; set; }
                
        [Parameter(DefaultValue = false, Group = "Time frames")]
        public bool D2 { get; set; }
                
        [Parameter(DefaultValue = false, Group = "Time frames")]
        public bool D3 { get; set; }
                        
        [Parameter(DefaultValue = false, Group = "Time frames")]
        public bool W1 { get; set; }
        
        [Parameter(DefaultValue = false, Group = "Time frames")]
        public bool M1 { get; set; }
        
        private Dictionary<TimeFrame, DirectionalMovementSystem> _dms = new Dictionary<TimeFrame, DirectionalMovementSystem>();
        private Dictionary<TimeFrame, MyBlock> _blocks = new Dictionary<TimeFrame, MyBlock>();
        private List<TimeFrame> _timeframes;
        private Border _border;
       
        protected override void Initialize()
        {
            var result = System.Diagnostics.Debugger.Launch();

            if (result is false)
            {
                Print("Debugger launch failed");
            }

            _timeframes = new List<TimeFrame>();
            
            if (m1)
                _timeframes.Add(TimeFrame.Minute);
            if (m2)
                _timeframes.Add(TimeFrame.Minute2);
            if (m3)
                _timeframes.Add(TimeFrame.Minute3);
            if (m4)
                _timeframes.Add(TimeFrame.Minute4);
            if (m5)
                _timeframes.Add(TimeFrame.Minute5);
            if (m6)
                _timeframes.Add(TimeFrame.Minute6);
            if (m7)
                _timeframes.Add(TimeFrame.Minute7);
            if (m8)
                _timeframes.Add(TimeFrame.Minute8);
            if (m9)
                _timeframes.Add(TimeFrame.Minute9);
            if (m10)
                _timeframes.Add(TimeFrame.Minute10);
            if (m15)
                _timeframes.Add(TimeFrame.Minute15);
            if (m20)
                _timeframes.Add(TimeFrame.Minute20);
            if (m30)
                _timeframes.Add(TimeFrame.Minute30);
            if (m45)
                _timeframes.Add(TimeFrame.Minute45);
            if (h1)
                _timeframes.Add(TimeFrame.Hour);
            if (h2)
                _timeframes.Add(TimeFrame.Hour2);
            if (h3)
                _timeframes.Add(TimeFrame.Hour3);
            if (h4)
                _timeframes.Add(TimeFrame.Hour4);
            if (h6)
                _timeframes.Add(TimeFrame.Hour6);
            if (h8)
                _timeframes.Add(TimeFrame.Hour8);
            if (h12)
                _timeframes.Add(TimeFrame.Hour12);                
            if (D1)
                _timeframes.Add(TimeFrame.Daily);
            if (D2)
                _timeframes.Add(TimeFrame.Day2);                
            if (D3)
                _timeframes.Add(TimeFrame.Day3);                
            if (W1)
                _timeframes.Add(TimeFrame.Weekly);        
            if (M1)
                _timeframes.Add(TimeFrame.Monthly);
            
            var stackPanel = new StackPanel
            {
                Orientation =  Orientation.Horizontal,                
            };
            stackPanel.AddChild(new TextBlock{
                Text = "DMS",
                VerticalAlignment = VerticalAlignment.Center,
                Margin = new Thickness(0,0,10,0),
                FontWeight = FontWeight.Bold,
                FontStyle = FontStyle.Italic,
            });
            
            foreach (var timeframe in _timeframes)
            {
                var block = new MyBlock(timeframe.ShortName);
                Print(timeframe.Name);
                stackPanel.AddChild(block);
                _blocks[timeframe] = block;                
            }
            
            _border = new Border();            
            _border.BorderThickness = new Thickness(1);                  
            _border.Child = stackPanel;
                                                
            var grid = new Grid();            
            grid.AddChild(_border);
            _border.Padding = new Thickness(5,5,0,5);
            _border.CornerRadius = new CornerRadius(3);
            _border.HorizontalAlignment = HorizontalAlignment.Right;
            _border.VerticalAlignment = VerticalAlignment.Top;
            _border.Margin = new Thickness(0,10,25,0);
            Chart.AddControl(grid);
                                    
            foreach (var timeframe in _timeframes)
            {                
                var bars = MarketData.GetBars(timeframe);
                var rsi = Indicators.DirectionalMovementSystem(bars, Periods);
                _dms[timeframe] = rsi;
            }
            
            ApplyColorTheme();
            Application.ColorThemeChanged += args => ApplyColorTheme();
        }        
        
        private void ApplyColorTheme()
        {
            _border.BackgroundColor = Application.ColorTheme == ColorTheme.Dark
                ? "#292929"
                : "#ECEFF1";
            _border.BorderColor = Application.ColorTheme == ColorTheme.Dark
                ? "#B3B3B3"
                : "#DFDFDF";
        }
      

        public override void Calculate(int index)
        {
            if (!IsLastBar)
              return;
              
            foreach (var timeframe in _timeframes)
            {
                var plusValue = _dms[timeframe].DIPlus.LastValue;
                var minusValue = _dms[timeframe].DIMinus.LastValue;
                var block = _blocks[timeframe];
                if (plusValue >= minusValue)
                    block.SetDirection(TradeType.Buy);
                else 
                    block.SetDirection(TradeType.Sell);
            }
        }
    }
    
    public class MyBlock : CustomControl
    {
        public void SetDirection(TradeType? tradeType)
        {
            if (tradeType == TradeType.Buy)
            {
                _valueTextBlock.Text = "▲";
                _valueTextBlock.ForegroundColor = Color.Green;
            }
            if (tradeType == TradeType.Sell)
            {
                _valueTextBlock.Text = "▼";
                _valueTextBlock.ForegroundColor = Color.Red;
            }
            if (tradeType == null)
            {
                _valueTextBlock.Text = "-";
                _valueTextBlock.ForegroundColor = Color.White;
            }
        }
        
        private TextBlock _valueTextBlock;
    
        public MyBlock(string title)
        {
            var stackPanel = new StackPanel();
            stackPanel.Orientation = Orientation.Vertical;
            
            var titleTextBlock = new TextBlock
            {
                Text = title,
                HorizontalAlignment = HorizontalAlignment.Center,
            };           
            stackPanel.AddChild(titleTextBlock);
            
            _valueTextBlock = new TextBlock();
            _valueTextBlock.HorizontalAlignment = HorizontalAlignment.Center;
            
            stackPanel.AddChild(_valueTextBlock);
            stackPanel.Margin = new Thickness(0,0,5,0);
            
            AddChild(stackPanel);
        }        
    }
}