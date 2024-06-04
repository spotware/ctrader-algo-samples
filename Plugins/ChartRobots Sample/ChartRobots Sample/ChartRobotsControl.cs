using System.Linq;
using cAlgo.API;

namespace cAlgo.Plugins;

public class ChartRobotsControl: CustomControl
{
    private readonly AlgoRegistry _algoRegistry;
    
    private const string FontFamily = "Calibri";

    private readonly Grid _panel;
    private readonly TextBlock _robotsCountTextBlock;
    private readonly TextBlock _robotsTextBlock;
    private readonly TextBlock _runningRobotsCountTextBlock;
    private readonly ComboBox _robotTypesComboBox;
    private readonly Button _addRobotButton;
    private readonly Button _removeRobotsButton;
    private readonly Button _startRobotsButton;
    private readonly Button _stopRobotsButton;
    
    private Chart _chart;

    public ChartRobotsControl(AlgoRegistry algoRegistry)
    {
        _algoRegistry = algoRegistry;
        
        _panel = new Grid(7, 2);
        
        _panel.AddChild(GetTextBlock("Robots #"), 0, 0);

        _robotsCountTextBlock = GetTextBlock();
        
        _panel.AddChild(_robotsCountTextBlock, 0, 1);

        _panel.AddChild(GetTextBlock("Running Robots #"), 1, 0);

        _runningRobotsCountTextBlock = GetTextBlock();
        
        _panel.AddChild(_runningRobotsCountTextBlock, 1, 1);
        
        _panel.AddChild(GetTextBlock("Robots"), 2, 0);

        _robotsTextBlock = GetTextBlock();
        
        _panel.AddChild(_robotsTextBlock, 2, 1);

        _robotTypesComboBox = new ComboBox
        {
            Margin = 3,
            FontSize = 16,
            FontWeight = FontWeight.Bold,
            FontFamily = FontFamily,
        };

        PopulateTypes();

        _addRobotButton = new Button
        {
            Text = "Add Robot",
            Margin = 3,
            FontSize = 16,
            FontWeight = FontWeight.Bold,
            FontFamily = FontFamily,
        };

        _addRobotButton.Click += OnAddRobotButtonClick;
        
        var addRobotPanel = new StackPanel {Orientation = Orientation.Horizontal};
        
        addRobotPanel.AddChild(_robotTypesComboBox);
        addRobotPanel.AddChild(_addRobotButton);

        _panel.AddChild(addRobotPanel, 3, 0, 1, 2);

        _removeRobotsButton = new Button
        {
            Text = "Remove All Robots",
            Margin = 3,
            FontSize = 16,
            FontWeight = FontWeight.Bold,
            FontFamily = FontFamily,
        };

        _removeRobotsButton.Click += OnRemoveRobotsButtonClick;
        
        _panel.AddChild(_removeRobotsButton, 4, 0, 1, 2);

        _startRobotsButton = new Button
        {
            Text = "Start All Robots",
            Margin = 3,
            FontSize = 16,
            FontWeight = FontWeight.Bold,
            FontFamily = FontFamily,
        };
        
        _startRobotsButton.Click += OnStartRobotsButtonClick;
        
        _panel.AddChild(_startRobotsButton, 5, 0, 1, 2);

        _stopRobotsButton = new Button
        {
            Text = "Stop All Robots",
            Margin = 3,
            FontSize = 16,
            FontWeight = FontWeight.Bold,
            FontFamily = FontFamily,
        };
        
        _stopRobotsButton.Click += OnStopRobotsButtonClick;
        
        _panel.AddChild(_stopRobotsButton, 6, 0, 1, 2);
        
        AddChild(_panel);
        
        _algoRegistry.AlgoTypeInstalled += _ => PopulateTypes();
        _algoRegistry.AlgoTypeDeleted += _ => PopulateTypes();
    }

    public Chart Chart
    {
        get => _chart;
        set
        {
            if (_chart == value)
                return;

            UpdateChart(value);
        }
    }

    private void UpdateChart(Chart newChart)
    {
        var previousChart = _chart;
        
        _chart = newChart;

        UpdateStatus();
        
        newChart.Robots.RobotAdded += OnRobotsAdded;
        newChart.Robots.RobotRemoved += OnRobotRemoved;
        newChart.Robots.RobotStarted += OnRobotStarted;
        newChart.Robots.RobotStopped += OnRobotStopped;

        if (previousChart is null)
            return;
        
        previousChart.Robots.RobotAdded -= OnRobotsAdded;
        previousChart.Robots.RobotRemoved -= OnRobotRemoved;
        previousChart.Robots.RobotStarted -= OnRobotStarted;
        previousChart.Robots.RobotStopped -= OnRobotStopped;
    }

    private void OnRobotRemoved(ChartRobotRemovedEventArgs obj) => UpdateStatus();

    private void OnRobotsAdded(ChartRobotAddedEventArgs obj) => UpdateStatus();

    private void OnRobotStopped(ChartRobotStoppedEventArgs obj) => UpdateStatus();

    private void OnRobotStarted(ChartRobotStartedEventArgs obj) => UpdateStatus();
    
    private void OnRemoveRobotsButtonClick(ButtonClickEventArgs obj)
    {
        foreach (var chartRobot in _chart.Robots)
        {
            _chart.Robots.Remove(chartRobot);
        }
    }

    private void OnAddRobotButtonClick(ButtonClickEventArgs obj)
    {
        if (_algoRegistry.Get(_robotTypesComboBox.SelectedItem) is not {AlgoKind: AlgoKind.Robot} robotType)
            return;

        _chart.Robots.Add(robotType.Name);
    }

    private void UpdateStatus()
    {
        _robotsCountTextBlock.Text = _chart.Robots.Count.ToString();
        _runningRobotsCountTextBlock.Text = _chart.Robots.Count(r => r.State == RobotState.Running).ToString();
        _robotsTextBlock.Text = string.Join(", ", _chart.Robots.Select(r =>  r.Name));
    }
    
    private void PopulateTypes()
    {
        foreach (var algoType in _algoRegistry)
        {
            if (algoType.AlgoKind != AlgoKind.Robot)
                continue;
            
            _robotTypesComboBox.AddItem(algoType.Name);
        }
        
        _robotTypesComboBox.SelectedItem = _algoRegistry.FirstOrDefault(i => i.AlgoKind == AlgoKind.Robot)?.Name;
    }
    
    private void OnStopRobotsButtonClick(ButtonClickEventArgs obj)
    {
        foreach (var chartRobot in _chart.Robots)
        {
            if (chartRobot.State is (RobotState.Stopped or RobotState.Stopping))
                continue;
            
            chartRobot.Stop();
        }
    }

    private void OnStartRobotsButtonClick(ButtonClickEventArgs obj)
    {
        foreach (var chartRobot in _chart.Robots)
        {
            if (chartRobot.State is (RobotState.Running or RobotState.Restarting))
                continue;
            
            chartRobot.Start();
        }
    }
    
    private TextBlock GetTextBlock(string text = null) => new()
    {
        Margin = 3,
        FontSize = 16,
        FontWeight = FontWeight.Bold,
        FontFamily = FontFamily,
        Text = text
    };
}