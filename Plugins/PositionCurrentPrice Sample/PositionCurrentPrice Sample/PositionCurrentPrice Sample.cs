using System;
using cAlgo.API;
using cAlgo.API.Collections;
using cAlgo.API.Indicators;
using cAlgo.API.Internals;

namespace cAlgo.Plugins
{
    [Plugin(AccessRights = AccessRights.None)]
    public class PositionCurrentPriceSample : Plugin
    {
        private TextBlock _currentPriceText;
        
        
        protected override void OnStart()
        {
            _currentPriceText = new TextBlock
            {
                Text = "Initialising...",
                FontSize = 50,
                TextAlignment = TextAlignment.Center,
                FontWeight = FontWeight.ExtraBold,
            };
            
            Asp.SymbolTab.AddBlock("Position.CurrentPrice").Child = _currentPriceText;
            
            ExecuteMarketOrder(TradeType.Buy, "EURUSD", 10000, "plugin position");
            
            _currentPriceText.Text = Positions.Find("plugin position").CurrentPrice.ToString();
            
            Timer.Start(TimeSpan.FromMilliseconds(100));
            
        }

        protected override void OnTimer()
        {
            _currentPriceText.Text = Positions.Find("plugin position").CurrentPrice.ToString();
        }

    }        
}