// -------------------------------------------------------------------------------------------------
//
//    This code is a cTrader Algo API example.
//
//    This code is intended to be used as a sample and does not guarantee any particular outcome or
//    profit of any kind. Use it at your own risk.
//    
//    The sample adds a new block into the ASP. The block displays a ComboBox in which the user
//    can select any of their currently open positions. Below the ComboBox, the block displays the
//    current price of the symbol for which the selected position was opened.
//
// -------------------------------------------------------------------------------------------------

using System;
using cAlgo.API;
using cAlgo.API.Collections;
using cAlgo.API.Indicators;
using cAlgo.API.Internals;

namespace cAlgo.Plugins
{
    // Declaring the class as a plugin without requiring special access permissions.
    [Plugin(AccessRights = AccessRights.None)]
    public class PositionCurrentPriceSample : Plugin
    {
        private TextBlock _currentPriceText;  // Declaring a TextBlock to display the current price.
        ComboBox _positionSelectionComboBox;  // Declaring a ComboBox for selecting a position.
        Position _selectedPosition;  // Declaring a variable to hold the selected position.
        Grid _blockGrid;  // Declaring a grid for layout purposes.
        
        // This method is executed when the plugin starts.        
        protected override void OnStart()
        {
            // Configuring the new TextBlock for position selection.
            _currentPriceText = new TextBlock
            {
                Text = "Select a position above",
                FontSize = 30,
                TextAlignment = TextAlignment.Center,
                FontWeight = FontWeight.ExtraBold,
            };
            
            // Adding a new ComboBox for position selection and populating it with existing positions.
            _positionSelectionComboBox = new ComboBox();
            
            // Iterating through all positions to add them as items.
            foreach (var position in Positions) 
            {
                _positionSelectionComboBox.AddItem(position.Id.ToString());  // Adding the position Id to ComboBox.
            }
            
            // Setting up an event handler that triggers when the selected item changes in the ComboBox.
            _positionSelectionComboBox.SelectedItemChanged += SelectedPositionChanged;
                     
            // Configuring the grid where the ComboBox and the price TextBlock are placed.
            _blockGrid = new Grid(2, 1);
            _blockGrid.AddChild(_positionSelectionComboBox, 0, 0);
            _blockGrid.AddChild(_currentPriceText, 1, 0);
            
            // Adding a new block into the ASP.
            Asp.SymbolTab.AddBlock("Position.CurrentPrice").Child = _blockGrid;
                        
            // Starting the timer with 100 milliseconds as the tick.
            Timer.Start(TimeSpan.FromMilliseconds(100));
            
        }
        
        // Updating the _selectedPosition field by finding a position object with the Id chosen in the ComboBox.
        private void SelectedPositionChanged(ComboBoxSelectedItemChangedEventArgs obj)
        {
            _selectedPosition = FindPositionById(obj.SelectedItem);
        }

        // Overriding the built-in handler of the Timer.TimerTick event.
        protected override void OnTimer()
        {
            // Updating the text inside the TextBlock by using Position.CurrentPrice.
            _currentPriceText.Text = _selectedPosition.CurrentPrice.ToString();
        }
        
        // Defining a custom method to find a position by its Id.
        private Position FindPositionById(string positionId)
        {
            // Iterating through all open positions.
            foreach (var position in Positions)
            {
                if (position.Id.ToString() == positionId)  // Matching the Id of the position.
                {
                    return position;  // Returning the matched position.
                }
            }
            return null;  // Returning null if no position matches.
        }

    }        
}
