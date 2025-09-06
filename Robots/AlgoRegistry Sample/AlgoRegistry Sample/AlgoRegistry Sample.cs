// -------------------------------------------------------------------------------------------------
//
//    This code is a cTrader Algo API example.
//
//    This code is intended to be used as a sample and does not guarantee any particular outcome or
//    profit of any kind. Use it at your own risk.
//
//    On start, this sample displays a detached window containing a ComboBox and a TextBlock. 
//    Via the AlgoRegistry, the ComboBox is populated with the names of all cBots installed
//    on the user's machine. When a cBot is selected in the ComboBox, the TextBlock starts
//    showing information about its parameters and their default values.
//
// -------------------------------------------------------------------------------------------------

using System;
using cAlgo.API;
using cAlgo.API.Collections;
using cAlgo.API.Indicators;
using cAlgo.API.Internals;

namespace cAlgo.Robots
{
    // Define the cBot with access rights and the option to add indicators.   
    [Robot(AccessRights = AccessRights.None, AddIndicators = true)]
    public class AlgoRegistrySample : Robot
    {
        // Declare the required controls and store the name of the cBot selected in the ComboBox.
        private Window _cBotManagementWindow;
        private Grid _controlsGrid;
        private ComboBox _cBotSelectionComboBox;
        private TextBlock _parametersInfoBlock;
        private string _selectedCBotName;

        // This method is called when the bot starts and is used for initialisation.
        protected override void OnStart()
        {
            // Initialise a 2x1 Grid layout for arranging ComboBox and TextBlock.
            _controlsGrid = new Grid(2, 1);

            // Initialise the ComboBox and populate it with the names of all installed cBots.
            _cBotSelectionComboBox = new ComboBox();

            // Loop through all algorithms, adding cBots only to ComboBox.
            foreach (var algo in AlgoRegistry)
            {
                if (algo.AlgoKind == AlgoKind.Robot)
                {
                    _cBotSelectionComboBox.AddItem(algo.Name);
                }
            }
            
            // Set up an event handler for ComboBox selection change.
            _cBotSelectionComboBox.SelectedItemChanged += CBotSelectionComboBoxOnSelectedItemChanged;

            // Initialise the TextBlock for displaying selected cBot parameters.
            _parametersInfoBlock = new TextBlock
            {
                LineStackingStrategy = LineStackingStrategy.MaxHeight,
                TextWrapping = TextWrapping.Wrap,
            };
            
            // Add other controls to Grid layout.
            _controlsGrid.AddChild(_cBotSelectionComboBox, 0, 0);
            _controlsGrid.AddChild(_parametersInfoBlock, 1, 0);
            
            // Initialise the Window, add the Grid as a child and display it.
            _cBotManagementWindow = new Window
            {
                Height = 300,
                Width = 300,
                Padding = new Thickness(10, 10, 10 , 10),
            };

            _cBotManagementWindow.Child = _controlsGrid;
            _cBotManagementWindow.Show();
        }

        // Event handler to update TextBlock with selected cBot parameters.
        private void CBotSelectionComboBoxOnSelectedItemChanged(ComboBoxSelectedItemChangedEventArgs obj)
        {
            _selectedCBotName = obj.SelectedItem;  // Store the name of the selected cBot.
            _parametersInfoBlock.Text = GenerateParametersInfo();  // Update the text inside the TextBlock.
        }

        // Generate a string containing parameter names and default values of the selected cBot.
        private string GenerateParametersInfo()
        {
            string result = "";
            
            // Find the currently selected cBot by its name.
            var selectedCBot = AlgoRegistry.Get(_selectedCBotName, AlgoKind.Robot) as RobotType;
            
            // Generate strings containing information about the parameters of the selected cBot.
            foreach (var parameter in selectedCBot.Parameters)
            {
                result += $"Param name: {parameter.Name} Param default value: {parameter.DefaultValue}\n";
            }

            return result;

        }
    }
}
