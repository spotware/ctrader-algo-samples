// -------------------------------------------------------------------------------------------------
//
//    This code is a cTrader Algo API example.
//
//    This code is intended to be used as a sample and does not guarantee any particular outcome or
//    profit of any kind. Use it at your own risk.
//
//    The sample demonstrates how to create and configure a custom block in Active Symbol Panel (ASP)
//    using the cTrader Algo API.
//
// -------------------------------------------------------------------------------------------------

using cAlgo.API;

namespace cAlgo.Plugins
{
    // Declare the class as a plugin without requiring special access permissions.
    [Plugin(AccessRights = AccessRights.None)]
    public class ASPSectionExample : Plugin
    {
        // This method is triggered when the plugin starts.
        protected override void OnStart()
        {
            var block = Asp.SymbolTab.AddBlock("My title");  // Add a custom block to the ASP under the Symbol tab.

            block.Index = 2;  // Set the position index of the block in the panel.
            block.Height = 500;  // Define the height of the block in pixels.
            block.IsExpanded = true;  // Make the block expanded by default.

            var webView = new WebView();  // Create a WebView control to display a webpage.
            block.Child = webView;  // Adding the WebView as a child element to the block.

            webView.NavigateAsync("https://ctrader.com/");  // Navigate the WebView to the cTrader website.
        }
    }
}
