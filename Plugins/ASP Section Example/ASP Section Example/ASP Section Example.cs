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
            block.IsExpanded = true;
            
            var webView = new WebView();                        
            block.Child = webView;
            
            webView.NavigateAsync("https://ctrader.com/?theme=" + Application.ColorTheme);

            Application.ColorThemeChanged += Application_ColorThemeChanged;
        }

        private void Application_ColorThemeChanged(ColorThemeChangeEventArgs obj)
        {
            
        }

        protected override void OnStop()
        {
            // Handle Plugin stop here
        }
    }  
}