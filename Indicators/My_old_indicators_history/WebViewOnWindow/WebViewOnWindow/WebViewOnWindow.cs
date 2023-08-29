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
    [Indicator(AccessRights = AccessRights.None, IsOverlay = true)]
    public class WebViewOnWindow : Indicator
    {
        private WebView _webView;

        [Parameter("On Window?", DefaultValue = false)]
        public bool OnWindow { get; set; }

        protected override void Initialize()
        {
            Print(InstanceId);
            _webView = new WebView
            {
                //DefaultBackgroundColor = Color.Red
            };

            _webView.Loaded += OnWebViewLoaded;

            if (OnWindow)
            {
                var window = new Window
                {
                    Child = _webView
                };

                window.Show();
            }
            else
            {
                Chart.AddControl(_webView);
                   
            }         
        }
        
        public override void Calculate(int index)
        {
        }

        private void OnWebViewLoaded(WebViewLoadedEventArgs args)
        {
            _webView.NavigateAsync("https://finviz.com/map.ashx?t=sec");
            //MessageBox.Show("Click from Plugin");
        }
    }
}