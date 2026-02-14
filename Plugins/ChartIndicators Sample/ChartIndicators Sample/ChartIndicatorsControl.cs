// -------------------------------------------------------------------------------------------------
//
//    This code is a cTrader Algo API example.
//
//    This code is intended to be used as a sample and does not guarantee any particular outcome or
//    profit of any kind. Use it at your own risk.
//    
//    The sample implements a custom control for managing chart indicators using the ChartIndicators API.
//    It allows the user to view, add and remove indicators on the active chart.
//
// -------------------------------------------------------------------------------------------------

using System.Linq;
using cAlgo.API;

namespace cAlgo.Plugins;

public class ChartIndicatorsControl: CustomControl
{
    private readonly AlgoRegistry _algoRegistry;  // Reference to the Algo Registry for accessing algorithm data.
    
    private const string FontFamily = "Calibri";  // Specify the font family for UI elements.

    private readonly Grid _panel;  // Represent the main grid layout for arranging UI components.
    private readonly TextBlock _indicatorsCountTextBlock;  // Count of active indicators on the chart.
    private readonly TextBlock _indicatorsTextBlock;  // Names of active indicators on the chart.
    private readonly ComboBox _indicatorTypesComboBox;  // Dropdown to select indicator types to add.
    private readonly Button _addIndicatorButton;  // Button to add a selected indicator to the chart.
    private readonly Button _removeIndicatorsButton;  // Button to remove all indicators from the chart.

    private Chart _chart;  // Hold a reference to the current active chart.

    // This method initialises the control, accepting an Algo Registry object for managing available indicators.
    public ChartIndicatorsControl(AlgoRegistry algoRegistry)
    {
        _algoRegistry = algoRegistry;  // Assign the provided Algo Registry to the field.
        
        _panel = new Grid(6, 2);  // Create a grid with 6 rows and 2 columns.
        
        _panel.AddChild(GetTextBlock("Indicators #"), 0, 0);  // Add a label for the indicators count to the grid.

        _indicatorsCountTextBlock = GetTextBlock();  // Create a text block for displaying the indicators count.
        
        _panel.AddChild(_indicatorsCountTextBlock, 0, 1);  // Add the indicators count text block to the grid.

        _panel.AddChild(GetTextBlock("Indicators"), 1, 0);  // Add a label for the indicators list to the grid.

        _indicatorsTextBlock = GetTextBlock();  // Create a text block for displaying the indicators list.
        
        _panel.AddChild(_indicatorsTextBlock, 1, 1);  // Add the indicators list text block to the grid.

        _indicatorTypesComboBox = new ComboBox  // Initialise the combo box for selecting indicator types.
        {
            Margin = 3,
            FontSize = 16,
            FontWeight = FontWeight.Bold,
            FontFamily = FontFamily,
        };

        PopulateTypes();  // Populate the combo box with available indicator types.

        _addIndicatorButton = new Button  // Initialise the button to add an indicator.
        {
            Text = "Add Indicator",
            Margin = 3,
            FontSize = 16,
            FontWeight = FontWeight.Bold,
            FontFamily = FontFamily,
        };

        _addIndicatorButton.Click += OnAddIndicatorButtonClick;  // Assign the click event handler for the add button.
        
        var addIndicatorPanel = new StackPanel {Orientation = Orientation.Horizontal};  // Create a horizontal panel for the combo box and button.
        
        addIndicatorPanel.AddChild(_indicatorTypesComboBox);  // Add the combo box to the panel.
        addIndicatorPanel.AddChild(_addIndicatorButton);  // Add the add button to the panel.

        _panel.AddChild(addIndicatorPanel, 2, 0, 1, 2);  // Add the horizontal panel to the grid.

        _removeIndicatorsButton = new Button  // Initialise the button to remove all indicators.
        {
            Text = "Remove All Indicators",
            Margin = 3,
            FontSize = 16,
            FontWeight = FontWeight.Bold,
            FontFamily = FontFamily,
        };

        _removeIndicatorsButton.Click += OnRemoveIndicatorsButtonClick;  // Assign the click event handler for the remove button.
        
        _panel.AddChild(_removeIndicatorsButton, 3, 0, 1, 2);  // Add the remove button to the grid.

        AddChild(_panel);  // Add the main grid to the control.
        
        _algoRegistry.AlgoTypeInstalled += _ => PopulateTypes();  // Update the combo box when a new algorithm is installed.
        _algoRegistry.AlgoTypeDeleted += _ => PopulateTypes();  // Update the combo box when an algorithm is deleted.
    }

    // Property to get or set the current chart.
    public Chart Chart
    {
        get => _chart;  // Return the current chart.
        set
        {
            if (_chart == value)  // Check if the new chart is the same as the current chart.
                return;

            UpdateChart(value);  // Update the chart and event subscriptions.
        }
    }

    // Update the chart reference and subscriptions.
    private void UpdateChart(Chart newChart)
    {
        var previousChart = _chart;  // Store the previous chart.
        
        _chart = newChart;  // Update the chart reference.

        UpdateStatus();  // Update the status of indicators on the new chart.
        
        newChart.Indicators.IndicatorAdded += OnIndicatorsAdded;  // Subscribe to the indicator added event.
        newChart.Indicators.IndicatorRemoved += OnIndicatorRemoved;  // Subscribe to the indicator removed event.
        
        if (previousChart is null)  // If there is no previous chart, exit early.
            return;
        
        previousChart.Indicators.IndicatorAdded -= OnIndicatorsAdded;  // Unsubscribe from the indicator added event for the previous chart.
        previousChart.Indicators.IndicatorRemoved -= OnIndicatorRemoved;  // Unsubscribe from the indicator removed event for the previous chart.
    }

    // Update the status when an indicator is removed.
    private void OnIndicatorRemoved(ChartIndicatorRemovedEventArgs obj) => UpdateStatus();

    // Update the status when a new indicator is added.
    private void OnIndicatorsAdded(ChartIndicatorAddedEventArgs obj) => UpdateStatus();

    // Event handler for removing all indicators.
    private void OnRemoveIndicatorsButtonClick(ButtonClickEventArgs obj)
    {
        foreach (var chartIndicator in _chart.Indicators)  // Iterate through all indicators on the chart.
        {
            _chart.Indicators.Remove(chartIndicator);  // Remove each indicator from the chart.
        }
    }

    // Event handler for adding a selected indicator.
    private void OnAddIndicatorButtonClick(ButtonClickEventArgs obj)
    {
        if (_algoRegistry.Get(_indicatorTypesComboBox.SelectedItem) is not {AlgoKind: AlgoKind.CustomIndicator or AlgoKind.StandardIndicator} indicatorType)
            return;  // Exit if the selected item is not a valid indicator type.

        _chart.Indicators.Add(indicatorType.Name);  // Add the selected indicator to the chart.
    }

    // Update the displayed status of indicators on the chart.
    private void UpdateStatus()
    {
        _indicatorsCountTextBlock.Text = _chart.Indicators.Count.ToString();  // Update the count of indicators.
        _indicatorsTextBlock.Text = string.Join(", ", _chart.Indicators.Select(i => i.Name));  // Update the list of indicator names.
    }
    
    // Populate the combo box with available indicator types.
    private void PopulateTypes()
    {
        foreach (var algoType in _algoRegistry)  // Iterate through all registered algorithms.
        {
            if (algoType.AlgoKind is not (AlgoKind.CustomIndicator or AlgoKind.StandardIndicator))
                continue;  // Skip algorithms that are not indicators.
            
            _indicatorTypesComboBox.AddItem(algoType.Name);  // Add the algorithm name to the combo box.
        }
        
        _indicatorTypesComboBox.SelectedItem = _algoRegistry.FirstOrDefault(i => i.AlgoKind == AlgoKind.StandardIndicator)?.Name;  // Set the default selection to a standard indicator.
    }
    
    // Helper method to create a styled text block.
    private TextBlock GetTextBlock(string text = null) => new()
    {
        Margin = 3,
        FontSize = 16,
        FontWeight = FontWeight.Bold,
        FontFamily = FontFamily,
        Text = text
    };
}
