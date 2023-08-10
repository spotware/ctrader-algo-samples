using cAlgo.API;

namespace cAlgo.Plugins
{
    [Plugin(AccessRights = AccessRights.None)]
    public class ASPSectionExample : Plugin
    {
        protected override void OnStart()
        {
            var block = Asp.SymbolTab.AddBlock("My title");
            block.Index = 2;
            block.Height = 500;
            
            var webView = new WebView();
            webView.NavigateAsync("https://ctrader.com/");
            block.Child = webView;
        }
    }        
}