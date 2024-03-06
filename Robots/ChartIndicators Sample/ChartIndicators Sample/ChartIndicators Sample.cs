// -------------------------------------------------------------------------------------------------
//
//    This code is a cTrader Algo API example.
//
//    This code is intended to be used as a sample and does not guarantee any particular outcome or
//    profit of any kind. Use it at your own risk.
//    
//    This sample displays a custom detached window there the user is able to select
//    an indicator from among the indicators attached to the instance chart. When an
//    indicator is selected, the user can press on a button to remove it from the chart
//    via the ChartIndicators.Remove() method.
//
// -------------------------------------------------------------------------------------------------

using System;
using cAlgo.API;
using cAlgo.API.Collections;
using cAlgo.API.Indicators;
using cAlgo.API.Internals;

namespace cAlgo.Robots
{
    [Robot(AccessRights = AccessRights.None, AddIndicators = true)]
    public class ChartIndicatorsSample : Robot
    {
        // Declaring the necessary UI elements
        // and a variable for storing the selected item in the ComboBox
        private ComboBox _indicatorSelectionComboBox;
        private ChartIndicator _selectedIndicator;
        private Grid _windowGrid;
        private Button _removeIndicatorButton;
        private Window _indicatorRemovalWindow;
        
        protected override void OnStart()
        {
            // Initialising the indicator selection ComboBox
            // and populating it with options
            _indicatorSelectionComboBox = new ComboBox();
            
            foreach (var indicator in ChartIndicators) 
            {
                _indicatorSelectionComboBox.AddItem($"{indicator.Name} / {indicator.InstanceId}");
            }
            
            // Handling the SelectedItemChanged event
            _indicatorSelectionComboBox.SelectedItemChanged += IndicatorSelectionComboBoxOnSelectedItemChanged;

            // Initialising the indicator removal button
            _removeIndicatorButton = new Button
            {
                BackgroundColor = Color.Indigo,
                Text = "Remove Selected Indicator"
            };

            // Handling the Click event for the button
            _removeIndicatorButton.Click += OnClick;

            // Initialising the Grid to be used for storing other controls
            _windowGrid = new Grid(2, 1);
            _windowGrid.AddChild(_indicatorSelectionComboBox, 0, 0);
            _windowGrid.AddChild(_removeIndicatorButton, 1, 0);

            // Initialising the detached window and adding the Grid
            // as its child
            _indicatorRemovalWindow = new Window
            {
                Padding = new Thickness(10, 10, 10, 10),
                MaxHeight = 200,
                MaxWidth = 500,
                
            };

            _indicatorRemovalWindow.Child = _windowGrid;
            
            // Showing the window on cBot start
            _indicatorRemovalWindow.Show();

        }

        private void IndicatorSelectionComboBoxOnSelectedItemChanged(ComboBoxSelectedItemChangedEventArgs obj)
        {
            // Recovering the instanceId from the selected item
            // and storing the found ChartIndicator
            var instanceId = obj.SelectedItem.Split(" / ");
            _selectedIndicator = ChartIndicators[instanceId[1]];
        }

        private void OnClick(ButtonClickEventArgs obj)
        {
            // Removing the currently selected ChartIndicator
            // on button click
            ChartIndicators.Remove(_selectedIndicator);
        }
    }
}