using cAlgo.API;

namespace cAlgo.Plugins
{
    [Plugin(AccessRights = AccessRights.None)]
    public class CurrentSymbolplugin : Plugin
    {
        private TextBlock _symbolNameTextBlock;
        private TextBlock _askTextBlock;
        private TextBlock _bidTextBlock;
    
        protected override void OnStart()
        {
            var aspBlock = Asp.SymbolTab.AddBlock("Current symbol example");
            aspBlock.Index = 1;
            aspBlock.Height = 100;
            
            var rootPanel = new StackPanel{Margin = new Thickness(10, 10, 10, 10)};            
            aspBlock.Child = rootPanel;
            
            _symbolNameTextBlock = new TextBlock
            { 
               Text = Asp.SymbolTab.Symbol.Name
            };
            rootPanel.AddChild(_symbolNameTextBlock);          
            
            var pricesPanel = new StackPanel
            {
                Orientation = Orientation.Horizontal
            };
            rootPanel.AddChild(pricesPanel);
            
            _askTextBlock = new TextBlock
            {
                ForegroundColor = Color.LightBlue,
                Text = Asp.SymbolTab.Symbol.Ask.ToString(),
            };
            _bidTextBlock = new TextBlock
            {
                ForegroundColor = Color.Red,
                Text = Asp.SymbolTab.Symbol.Bid.ToString(),
                Margin = new Thickness(5, 0, 0, 0),
            };
            
            pricesPanel.AddChild(_askTextBlock);
            pricesPanel.AddChild(_bidTextBlock);

            Asp.SymbolTab.SymbolChanged += AspSymbolTab_SymbolChanged;
            Asp.SymbolTab.Symbol.Tick += OnAspSymbolTick;
        }

        private void AspSymbolTab_SymbolChanged(AspSymbolChangedEventArgs args)
        {
            if (args.OldSymbol != null)
            {
                args.OldSymbol.Tick -= OnAspSymbolTick;                
            }
        
            if (args.NewSymbol != null)
            {        
              args.NewSymbol.Tick += OnAspSymbolTick;
            
              _symbolNameTextBlock.Text = args.NewSymbol.Name;
              _askTextBlock.Text = args.NewSymbol.Ask.ToString();
              _bidTextBlock.Text = args.NewSymbol.Bid.ToString();
            }
            
        }
        
        private void OnAspSymbolTick(SymbolTickEventArgs args)
        {
            _askTextBlock.Text = args.Ask.ToString();
            _bidTextBlock.Text = args.Bid.ToString();
            
        }
    }        
}