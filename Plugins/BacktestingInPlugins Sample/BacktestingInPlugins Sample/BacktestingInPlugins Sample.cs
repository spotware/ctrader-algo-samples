// -------------------------------------------------------------------------------------------------
//
//    This code is a cTrader Algo API example.
//
//    This code is intended to be used as a sample and does not guarantee any particular outcome or
//    profit of any kind. Use it at your own risk.
//    
//    The sample adds a new block into Active Symbol Panel (ASP). The block contains a ComboBox, 
//    a custom Button and a TextBlock. Upon choosing one of their installed cBots in the ComboBox, 
//    the user can click on the Button and backtest the chosen cBot on EURUSD h1. When backtesting 
//    is finished, the plugin will display its results in the TextBlock.
//
// -------------------------------------------------------------------------------------------------

using System;
using cAlgo.API;
using cAlgo.API.Collections;
using cAlgo.API.Indicators;
using cAlgo.API.Internals;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace cAlgo.Plugins
{
    // Declaring the class as a plugin without requiring special access permissions.
    [Plugin(AccessRights = AccessRights.None)]
    public class BacktestingInPluginsSample : Plugin
    {
        // Declaring the necessary UI elements and the cBot (RobotType) selected in the ComboBox.
        private Grid _grid;
        private ComboBox _cBotsComboBox;
        private Button _startBacktestingButton;
        private TextBlock _resultsTextBlock;
        private RobotType _selectedRobotType;

        // This method is triggered when the plugin starts.
        protected override void OnStart()
        {
            // Initialising and structuring the UI elements into a grid layout.
            _grid = new Grid(3, 1);
            _cBotsComboBox = new ComboBox();
            
            // Initialising a button for starting the backtesting process.
            _startBacktestingButton = new Button
            {
                BackgroundColor = Color.Green,
                CornerRadius = new CornerRadius(5),
                Text = "Start Backtesting",
            };
            
            // Creating a TextBlock to display information.
            _resultsTextBlock = new TextBlock
            {
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
                Text = "Select a cBot...",
            };

            // Adding UI elements to the grid.
            _grid.AddChild(_cBotsComboBox, 0, 0);
            _grid.AddChild(_startBacktestingButton, 1, 0);
            _grid.AddChild(_resultsTextBlock, 2, 0);
           
            // Adding a new block in the ASP with the grid as its content.
            var block = Asp.SymbolTab.AddBlock("Backtesting Plugin");
            block.Child = _grid;
            
             // Populating the ComboBox with existing cBots.
            PopulateCBotsComboBox();

            // Assigning event handlers to the Button.Click, ComboBox.SelectedItemChanged and Backtesting.Completed events.
            _startBacktestingButton.Click += StartBacktestingButton_Click;
            _cBotsComboBox.SelectedItemChanged += CBotsComboBox_SelectedItemChanged;
            Backtesting.Completed += Backtesting_Completed;
        }
        
        // Event handler triggered when the backtesting button is clicked.
        protected void StartBacktestingButton_Click(ButtonClickEventArgs obj)
        {
            // Initialising and configuring the backtesting settings.
            var backtestingSettings = new BacktestingSettings 
            {
                DataMode = BacktestingDataMode.M1,
                StartTimeUtc = new DateTime(2023, 6, 1),
                EndTimeUtc = DateTime.UtcNow,
                Balance = 10000,
            };
            
            // Starting backtesting on EURUSD h1.
            Backtesting.Start(_selectedRobotType, "EURUSD", TimeFrame.Hour, backtestingSettings);
            
            // Disabling other controls and changing the text inside the TextBlock.
            _cBotsComboBox.IsEnabled = false;
            _startBacktestingButton.IsEnabled = false;
            _resultsTextBlock.Text = "Backtesting in progress...";
        }

        // Populating the ComboBox with the names of all installed cBots available in the Algo Registry.
        protected void PopulateCBotsComboBox()
        {
            // Iterating over the Algo Registry and getting the names of all installed cBots.
            foreach (var robotType in AlgoRegistry.OfType<RobotType>())
            {
                _cBotsComboBox.AddItem(robotType.Name);
            }
        }

        // Event handler triggered when backtesting is completed.
        protected void Backtesting_Completed(BacktestingCompletedEventArgs obj)
        {
            // Attaining the JSON results of backtesting.
            string jsonResults = obj.JsonReport;
            
            // Converting the JSON string into a JsonNode.
            JsonNode resultsNode = JsonNode.Parse(jsonResults);
            
            // Attaining the ROI and net profit from backtesting results.
            _resultsTextBlock.Text = $"ROI: {resultsNode["main"]["roi"]}\nNet Profit: {resultsNode["main"]["netProfit"]}";
            
            // Re-enabling controls after backteting is finished.
            _cBotsComboBox.IsEnabled = true;
            _startBacktestingButton.IsEnabled = true;
        }

        // Event handler triggered when the ComboBox selection changes.
        protected void CBotsComboBox_SelectedItemChanged(ComboBoxSelectedItemChangedEventArgs obj)
        {
            // Updading the variable to always contain the cBot selected in the ComboBox.
            _selectedRobotType = AlgoRegistry.Get(obj.SelectedItem) as RobotType;
        }

    }
}
