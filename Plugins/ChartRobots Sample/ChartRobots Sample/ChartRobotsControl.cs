// -------------------------------------------------------------------------------------------------
//
//    This code is a cTrader Algo API example.
//
//    This code is intended to be used as a sample and does not guarantee any particular outcome or
//    profit of any kind. Use it at your own risk.
//    
//    This sample implements a custom control that interacts with the ChartRobots API to provide
//    statistics about cBots on the active chart. It also allows adding, removing, starting and
//    stopping cBots directly from the control interface.
//
// -------------------------------------------------------------------------------------------------

using System.Linq;
using cAlgo.API;

namespace cAlgo.Plugins;

public class ChartRobotsControl: CustomControl
{
    private readonly AlgoRegistry _algoRegistry;  // Hold a reference to the Algo Registry for accessing algorithm data.
    private const string FontFamily = "Calibri";  // Define a constant for the font family used in the UI.

    // Define controls for the user interface.
    private readonly Grid _panel;
    private readonly TextBlock _robotsCountTextBlock;
    private readonly TextBlock _robotsTextBlock;
    private readonly TextBlock _runningRobotsCountTextBlock;
    private readonly ComboBox _robotTypesComboBox;
    private readonly Button _addRobotButton;
    private readonly Button _removeRobotsButton;
    private readonly Button _startRobotsButton;
    private readonly Button _stopRobotsButton;
    
    private Chart _chart;  // Represent the active chart associated with this control.

    // This method initialises the control and sets up UI components.
    public ChartRobotsControl(AlgoRegistry algoRegistry)
    {
        _algoRegistry = algoRegistry;  // Store the reference to the algo registry.
        
        _panel = new Grid(7, 2);  // Create a grid with 7 rows and 2 columns.
        
        // Add and configure text blocks and buttons to the grid.
        _panel.AddChild(GetTextBlock("Robots #"), 0, 0);  // Label for robot count.
        _robotsCountTextBlock = GetTextBlock();  // Display robot count dynamically.
        _panel.AddChild(_robotsCountTextBlock, 0, 1);  // Add robot count text block.

        _panel.AddChild(GetTextBlock("Running Robots #"), 1, 0);  // Label for running robots.
        _runningRobotsCountTextBlock = GetTextBlock();  // Display running robot count.
        _panel.AddChild(_runningRobotsCountTextBlock, 1, 1);  // Add running robot count text block.
        
        _panel.AddChild(GetTextBlock("Robots"), 2, 0);  // Label for robot names.
        _robotsTextBlock = GetTextBlock();  // Display robot names dynamically.
        _panel.AddChild(_robotsTextBlock, 2, 1);  // Add robot names text block.

        // Initialise and configure the dropdown menu for selecting robot types.
        _robotTypesComboBox = new ComboBox
        {
            Margin = 3,
            FontSize = 16,
            FontWeight = FontWeight.Bold,
            FontFamily = FontFamily,
        };

        PopulateTypes();  // Populate the dropdown with available robot types.

        // Create and configure the button for adding robots.
        _addRobotButton = new Button
        {
            Text = "Add Robot",
            Margin = 3,
            FontSize = 16,
            FontWeight = FontWeight.Bold,
            FontFamily = FontFamily,
        };

        _addRobotButton.Click += OnAddRobotButtonClick;  // Attach the click event handler to the button.
        
        // Create a horizontal panel to hold the dropdown and the add button.
        var addRobotPanel = new StackPanel {Orientation = Orientation.Horizontal};  // Panel for dropdown and button.
        addRobotPanel.AddChild(_robotTypesComboBox);  // Add dropdown to panel.
        addRobotPanel.AddChild(_addRobotButton);  // Add button to panel.
        _panel.AddChild(addRobotPanel, 3, 0, 1, 2);  // Add the panel to the grid.

        // Initialise and configure the remove all robots button.
        _removeRobotsButton = new Button
        {
            Text = "Remove All Robots",
            Margin = 3,
            FontSize = 16,
            FontWeight = FontWeight.Bold,
            FontFamily = FontFamily,
        };

        _removeRobotsButton.Click += OnRemoveRobotsButtonClick;  // Attach click event handler.
        _panel.AddChild(_removeRobotsButton, 4, 0, 1, 2);  // Add to the grid, spanning two columns.

        // Initialize and configure the start all robots buttonю
        _startRobotsButton = new Button
        {
            Text = "Start All Robots",
            Margin = 3,
            FontSize = 16,
            FontWeight = FontWeight.Bold,
            FontFamily = FontFamily,
        };
        
        _startRobotsButton.Click += OnStartRobotsButtonClick;  // Attach click event handler.        
        _panel.AddChild(_startRobotsButton, 5, 0, 1, 2);  // Add to the grid, spanning two columns.

        // Initialise and configure the stop all robots button.
        _stopRobotsButton = new Button
        {
            Text = "Stop All Robots",
            Margin = 3,
            FontSize = 16,
            FontWeight = FontWeight.Bold,
            FontFamily = FontFamily,
        };
        
        _stopRobotsButton.Click += OnStopRobotsButtonClick;  // Attach click event handler.       
        _panel.AddChild(_stopRobotsButton, 6, 0, 1, 2);  // Add to the grid, spanning two columns.
        
        AddChild(_panel);  // Add the grid to the control as its root UI element.
        
        // Subscribe to events from the algorithm registry for updating dropdown options.
        _algoRegistry.AlgoTypeInstalled += _ => PopulateTypes();  // Update dropdown on new installations.
        _algoRegistry.AlgoTypeDeleted += _ => PopulateTypes();  // Update dropdown on deletions.
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

    // Update the control with a new chart instance, subscribing to events and updating the UI to reflect the new chart state.
    private void UpdateChart(Chart newChart)
    {
        var previousChart = _chart;  // Store the current chart before updating, to manage event unsubscriptions.
        
        _chart = newChart;  // Update the control with the new chart.

        UpdateStatus();  // Refresh the UI to display the status of robots in the new chart.
        
        // Subscribe to various robot-related events for the new chart.
        newChart.Robots.RobotAdded += OnRobotsAdded;
        newChart.Robots.RobotRemoved += OnRobotRemoved;
        newChart.Robots.RobotStarted += OnRobotStarted;
        newChart.Robots.RobotStopped += OnRobotStopped;

        // If there is a previous chart, unsubscribe from its events to prevent memory leaks.
        if (previousChart is null)
            return;
        
        previousChart.Robots.RobotAdded -= OnRobotsAdded;
        previousChart.Robots.RobotRemoved -= OnRobotRemoved;
        previousChart.Robots.RobotStarted -= OnRobotStarted;
        previousChart.Robots.RobotStopped -= OnRobotStopped;
    }

    private void OnRobotRemoved(ChartRobotRemovedEventArgs obj) => UpdateStatus();  // Update status when a robot is removed.

    private void OnRobotsAdded(ChartRobotAddedEventArgs obj) => UpdateStatus();  // Update status when a robot is added.

    private void OnRobotStopped(ChartRobotStoppedEventArgs obj) => UpdateStatus();  // Update status when a robot stops.

    private void OnRobotStarted(ChartRobotStartedEventArgs obj) => UpdateStatus();  // Update status when a robot starts.
    
    // Remove all robots from the chart when the "Remove All Robots" button is clicked.
    private void OnRemoveRobotsButtonClick(ButtonClickEventArgs obj)
    {
        foreach (var chartRobot in _chart.Robots)
        {
            _chart.Robots.Remove(chartRobot);  // Remove each robot from the chart.
        }
    }

    // Add a new robot to the chart when the "Add Robot" button is clicked.
    private void OnAddRobotButtonClick(ButtonClickEventArgs obj)
    {
        // Exit if no valid robot type is selected.
        if (_algoRegistry.Get(_robotTypesComboBox.SelectedItem) is not {AlgoKind: AlgoKind.Robot} robotType)
            return;

        _chart.Robots.Add(robotType.Name);  // Add the selected robot to the chart.
    }

    // Update the status display with the current counts and names of robots on the chart.
    private void UpdateStatus()
    {
        _robotsCountTextBlock.Text = _chart.Robots.Count.ToString();  // Display the total number of robots.
        _runningRobotsCountTextBlock.Text = _chart.Robots.Count(r => r.State == RobotState.Running).ToString();  // Display the count of running robots.
        _robotsTextBlock.Text = string.Join(", ", _chart.Robots.Select(r =>  r.Name));  // Display the names of robots on the chart.
    }
    
    // Populate the dropdown list with available robot types from the algorithm registry.
    private void PopulateTypes()
    {
        foreach (var algoType in _algoRegistry)
        {
            // Only include robots in the dropdown.
            if (algoType.AlgoKind != AlgoKind.Robot)
                continue;
            
            _robotTypesComboBox.AddItem(algoType.Name);  // Add each robot type to the dropdown.
        }
        
        // Set the default selected item in the dropdown to the first robot type found in the registry.
        _robotTypesComboBox.SelectedItem = _algoRegistry.FirstOrDefault(i => i.AlgoKind == AlgoKind.Robot)?.Name;
    }
    
    // Stop all running robots on the chart when the "Stop All Robots" button is clicked.
    private void OnStopRobotsButtonClick(ButtonClickEventArgs obj)
    {
        foreach (var chartRobot in _chart.Robots)
        {
            // Skip robots that are already stopped or stopping.
            if (chartRobot.State is (RobotState.Stopped or RobotState.Stopping))
                continue;
            
            chartRobot.Stop();  // Stop the robot.
        }
    }

    // Start all stopped or restarting robots on the chart when the "Start All Robots" button is clicked.
    private void OnStartRobotsButtonClick(ButtonClickEventArgs obj)
    {
        foreach (var chartRobot in _chart.Robots)
        {
            // Skip robots that are already running or restarting.
            if (chartRobot.State is (RobotState.Running or RobotState.Restarting))
                continue;
            
            chartRobot.Start();  // Start the robot.
        }
    }
    
    // Define a method to create a text block with specific settings.
    private TextBlock GetTextBlock(string text = null) => new()
    {
        Margin = 3,
        FontSize = 16,
        FontWeight = FontWeight.Bold,
        FontFamily = FontFamily,
        Text = text
    };
}
