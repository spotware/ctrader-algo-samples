 using cAlgo.API;
 namespace cAlgo
 {
     // A sample indicator that shows how to use Border chart control
     [Indicator(IsOverlay = true, TimeZone = TimeZones.UTC, AccessRights = AccessRights.None)]
     public class BorderSample : Indicator
     {
         private WebView webView;        
         
         protected override void Initialize()
         {
            
             var border = new Border
             {
                 BorderColor = Color.Yellow,
                 BorderThickness = 2,
                 Opacity = 0.5,
                 BackgroundColor = Color.Green,
                 HorizontalAlignment = HorizontalAlignment.Center,
                 VerticalAlignment = VerticalAlignment.Center,
                 Width = 200,
                 Height = 100,
                 Margin = 10
             };
             var stackPanel = new StackPanel
             {
                 Orientation = Orientation.Vertical
             };
             stackPanel.AddChild(new TextBlock
             {
                 Text = "Text",
                 Margin = 5,
                 HorizontalAlignment = HorizontalAlignment.Center,
                 FontWeight = FontWeight.ExtraBold
             });
             stackPanel.AddChild(new Button
             {
                 Text = "Button",
                 Margin = 5,
                 HorizontalAlignment = HorizontalAlignment.Center,
                 FontWeight = FontWeight.ExtraBold
             });
             stackPanel.AddChild(new TextBox
             {
                 Text = "Type text...",
                 Margin = 5,
                 HorizontalAlignment = HorizontalAlignment.Center,
                 FontWeight = FontWeight.ExtraBold,
                 Width = 100
             });
             border.Child = stackPanel;
             Chart.AddControl(border);
             
             
             var border2 = new Border
             {
                 BorderColor = Color.Yellow,
                 BorderThickness = 2,
                 Opacity = 0.5,
                 BackgroundColor = Color.Green,
                 HorizontalAlignment = HorizontalAlignment.Left,
                 VerticalAlignment = VerticalAlignment.Center,
                 Width = 200,
                 Height = 100,
                 Margin = 10
             };
             
                webView = new WebView();
                webView.Loaded += webView_Loaded;
                
               
                border2.Child = webView;
                Chart.AddControl(border2);
            
         }
         public override void Calculate(int index)
         {
         }
         
         private void webView_Loaded(WebViewLoadedEventArgs obj)
         {
            obj.WebView.NavigateAsync("https://finviz.com/map.ashx");
         }
     }
 }
