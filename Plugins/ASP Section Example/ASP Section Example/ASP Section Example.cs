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
            block.Child = webView;

            webView.Loaded += webView_Loaded;
        }

        private void webView_Loaded(WebViewLoadedEventArgs args)
        {
            args.WebView.NavigateAsync("https://ctrader.com/");
        }

        protected override void OnStop()
        {
            // Handle Plugin stop here
        }
    }        
}