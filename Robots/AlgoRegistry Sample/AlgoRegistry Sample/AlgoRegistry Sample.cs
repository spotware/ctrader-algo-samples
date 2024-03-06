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
    [Robot(AccessRights = AccessRights.None, AddIndicators = true)]
    public class AlgoRegistrySample : Robot
    {
        // Declaring the required controls and storing
        // the name of the cBot selected in the ComboBox
        private Window _cBotManagementWindow;
        private Grid _controlsGrid;
        private ComboBox _cBotSelectionComboBox;
        private TextBlock _parametersInfoBlock;
        private string _selectedCBotName;

        protected override void OnStart()
        {
            // Initialising the Grid
            _controlsGrid = new Grid(2, 1);

            // Initialising the ComboBox and populating it
            // with the names of all installed cBots
            _cBotSelectionComboBox = new ComboBox();

            foreach (var algo in AlgoRegistry)
            {
                if (algo.AlgoKind == AlgoKind.Robot)
                {
                    _cBotSelectionComboBox.AddItem(algo.Name);
                }
            }
            
            // Handling the SelectedItemChanged event
            _cBotSelectionComboBox.SelectedItemChanged += CBotSelectionComboBoxOnSelectedItemChanged;

            // Initialising the TextBlock
            _parametersInfoBlock = new TextBlock
            {
                LineStackingStrategy = LineStackingStrategy.MaxHeight,
                TextWrapping = TextWrapping.Wrap,
            };
            
            // Adding other controls to the Grid
            _controlsGrid.AddChild(_cBotSelectionComboBox, 0, 0);
            _controlsGrid.AddChild(_parametersInfoBlock, 1, 0);
            
            // Initialising the Window and adding the Grid
            // as a child control
            _cBotManagementWindow = new Window
            {
                Height = 300,
                Width = 300,
                Padding = new Thickness(10, 10, 10 , 10),
            };

            _cBotManagementWindow.Child = _controlsGrid;
            _cBotManagementWindow.Show();
        }

        private void CBotSelectionComboBoxOnSelectedItemChanged(ComboBoxSelectedItemChangedEventArgs obj)
        {
            // Storing the name of the selected cBot
            // and changing the text inside the TextBlock
            _selectedCBotName = obj.SelectedItem;
            _parametersInfoBlock.Text = GenerateParametersInfo();
        }

        private string GenerateParametersInfo()
        {
            string result = "";
            
            // Finding the currently selected cBot by its name
            var selectedCBot = AlgoRegistry.Get(_selectedCBotName, AlgoKind.Robot) as RobotType;
            
            // Generating strings containing information
            // about the parameters of the selected cBot
            foreach (var parameter in selectedCBot.Parameters)
            {
                result += $"Param name: {parameter.Name} Param default value: {parameter.DefaultValue}\n";
            }

            return result;

        }
    }
}