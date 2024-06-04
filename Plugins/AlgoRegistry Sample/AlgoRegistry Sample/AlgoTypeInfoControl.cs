using System.Linq;
using cAlgo.API;

namespace cAlgo.Plugins;

public class AlgoTypeInfoControl: CustomControl
{
    private const string FontFamily = "Calibri";

    private readonly AlgoRegistry _algoRegistry;
    private readonly ComboBox _algoTypesComboBox;
    private readonly TextBlock _algoTypeKindTextBlock;
    private readonly TextBlock _algoTypeParametersTextBlock;
    private readonly TextBlock _algoTypeOutputsTextBlock;

    public AlgoTypeInfoControl(AlgoRegistry algoRegistry)
    {
        _algoRegistry = algoRegistry;
        
        var panel = new Grid(5, 2) {BackgroundColor = Color.Gray};

        var titleTextBlock = GetTextBlock("Algo Type Info");
        
        titleTextBlock.HorizontalAlignment = HorizontalAlignment.Center;
        
        panel.AddChild(titleTextBlock, 0, 0, 1, 2);
        
        panel.AddChild(GetTextBlock("Types"), 1, 0);

        _algoTypesComboBox = new ComboBox
        {
            Margin = 3,
            FontSize = 20,
            FontWeight = FontWeight.Bold,
            FontFamily = FontFamily,
            Padding = 2
        };
        
        panel.AddChild(_algoTypesComboBox, 1, 1);

        panel.AddChild(GetTextBlock("Kind"), 2, 0);

        _algoTypeKindTextBlock = GetTextBlock();
        
        panel.AddChild(_algoTypeKindTextBlock, 2, 1);
        
        panel.AddChild(GetTextBlock("Parameters"), 3, 0);

        _algoTypeParametersTextBlock = GetTextBlock();
        
        panel.AddChild(_algoTypeParametersTextBlock, 3, 1);
        
        panel.AddChild(GetTextBlock("Outputs"), 4, 0);

        _algoTypeOutputsTextBlock = GetTextBlock();
        
        panel.AddChild(_algoTypeOutputsTextBlock, 4, 1);
        
        AddChild(panel);

        _algoTypesComboBox.SelectedItemChanged += _ => OnAlgoTypesComboBoxSelectedItemChanged();
        
        PopulateTypes();

        _algoRegistry.AlgoTypeInstalled += _ => PopulateTypes();
        _algoRegistry.AlgoTypeDeleted += _ => PopulateTypes();
    }

    private void OnAlgoTypesComboBoxSelectedItemChanged()
    {
        if (_algoRegistry.Get(_algoTypesComboBox.SelectedItem) is not { } algoType)
        {
            _algoTypeKindTextBlock.Text = null;
            _algoTypeParametersTextBlock.Text = null;
            _algoTypeOutputsTextBlock.Text = null;
            
            return;
        }

        _algoTypeKindTextBlock.Text = algoType.AlgoKind.ToString();
        _algoTypeParametersTextBlock.Text = algoType switch
        {
            IndicatorType indicatorType => string.Join(", ", indicatorType.Parameters.Select(p => p.Name)),
            RobotType robotType => string.Join(", ", robotType.Parameters.Select(p => p.Name)),
            _ => null
        };
        _algoTypeOutputsTextBlock.Text = algoType switch
        {
            IndicatorType indicatorType => string.Join(", ", indicatorType.Outputs.Select(p => p.Name)),
            _ => null
        };
    }

    private void PopulateTypes()
    {
        foreach (var algoType in _algoRegistry)
        {
            _algoTypesComboBox.AddItem(algoType.Name);
        }
        
        _algoTypesComboBox.SelectedItem = _algoRegistry.FirstOrDefault()?.Name;

        OnAlgoTypesComboBoxSelectedItemChanged();
    }
    
    private TextBlock GetTextBlock(string text = null) => new()
    {
        Margin = 3,
        FontSize = 20,
        FontWeight = FontWeight.Bold,
        FontFamily = FontFamily,
        Text = text
    };
}