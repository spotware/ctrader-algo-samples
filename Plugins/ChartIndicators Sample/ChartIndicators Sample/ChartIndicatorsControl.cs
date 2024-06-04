using System.Linq;
using cAlgo.API;

namespace cAlgo.Plugins;

public class ChartIndicatorsControl: CustomControl
{
    private readonly AlgoRegistry _algoRegistry;
    
    private const string FontFamily = "Calibri";

    private readonly Grid _panel;
    private readonly TextBlock _indicatorsCountTextBlock;
    private readonly TextBlock _indicatorsTextBlock;
    private readonly ComboBox _indicatorTypesComboBox;
    private readonly Button _addIndicatorButton;
    private readonly Button _removeIndicatorsButton;

    private Chart _chart;

    public ChartIndicatorsControl(AlgoRegistry algoRegistry)
    {
        _algoRegistry = algoRegistry;
        
        _panel = new Grid(6, 2);
        
        _panel.AddChild(GetTextBlock("Indicators #"), 0, 0);

        _indicatorsCountTextBlock = GetTextBlock();
        
        _panel.AddChild(_indicatorsCountTextBlock, 0, 1);

        _panel.AddChild(GetTextBlock("Indicators"), 1, 0);

        _indicatorsTextBlock = GetTextBlock();
        
        _panel.AddChild(_indicatorsTextBlock, 1, 1);

        _indicatorTypesComboBox = new ComboBox
        {
            Margin = 3,
            FontSize = 16,
            FontWeight = FontWeight.Bold,
            FontFamily = FontFamily,
        };

        PopulateTypes();

        _addIndicatorButton = new Button
        {
            Text = "Add Indicator",
            Margin = 3,
            FontSize = 16,
            FontWeight = FontWeight.Bold,
            FontFamily = FontFamily,
        };

        _addIndicatorButton.Click += OnAddIndicatorButtonClick;
        
        var addIndicatorPanel = new StackPanel {Orientation = Orientation.Horizontal};
        
        addIndicatorPanel.AddChild(_indicatorTypesComboBox);
        addIndicatorPanel.AddChild(_addIndicatorButton);

        _panel.AddChild(addIndicatorPanel, 2, 0, 1, 2);

        _removeIndicatorsButton = new Button
        {
            Text = "Remove All Indicators",
            Margin = 3,
            FontSize = 16,
            FontWeight = FontWeight.Bold,
            FontFamily = FontFamily,
        };

        _removeIndicatorsButton.Click += OnRemoveIndicatorsButtonClick;
        
        _panel.AddChild(_removeIndicatorsButton, 3, 0, 1, 2);

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
        
        newChart.Indicators.IndicatorAdded += OnIndicatorsAdded;
        newChart.Indicators.IndicatorRemoved += OnIndicatorRemoved;
        
        if (previousChart is null)
            return;
        
        previousChart.Indicators.IndicatorAdded -= OnIndicatorsAdded;
        previousChart.Indicators.IndicatorRemoved -= OnIndicatorRemoved;
    }

    private void OnIndicatorRemoved(ChartIndicatorRemovedEventArgs obj) => UpdateStatus();

    private void OnIndicatorsAdded(ChartIndicatorAddedEventArgs obj) => UpdateStatus();

    private void OnRemoveIndicatorsButtonClick(ButtonClickEventArgs obj)
    {
        foreach (var chartIndicator in _chart.Indicators)
        {
            _chart.Indicators.Remove(chartIndicator);
        }
    }

    private void OnAddIndicatorButtonClick(ButtonClickEventArgs obj)
    {
        if (_algoRegistry.Get(_indicatorTypesComboBox.SelectedItem) is not {AlgoKind: AlgoKind.CustomIndicator or AlgoKind.StandardIndicator} indicatorType)
            return;

        _chart.Indicators.Add(indicatorType.Name);
    }

    private void UpdateStatus()
    {
        _indicatorsCountTextBlock.Text = _chart.Indicators.Count.ToString();
        _indicatorsTextBlock.Text = string.Join(", ", _chart.Indicators.Select(i => i.Name));
    }
    
    private void PopulateTypes()
    {
        foreach (var algoType in _algoRegistry)
        {
            if (algoType.AlgoKind is not (AlgoKind.CustomIndicator or AlgoKind.StandardIndicator))
                continue;
            
            _indicatorTypesComboBox.AddItem(algoType.Name);
        }
        
        _indicatorTypesComboBox.SelectedItem = _algoRegistry.FirstOrDefault(i => i.AlgoKind == AlgoKind.StandardIndicator)?.Name;
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