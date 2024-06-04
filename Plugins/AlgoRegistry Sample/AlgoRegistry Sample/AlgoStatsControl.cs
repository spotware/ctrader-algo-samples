using System.Linq;
using cAlgo.API;

namespace cAlgo.Plugins;

public class AlgoStatsControl: CustomControl
{
    private const string FontFamily = "Calibri";

    private readonly AlgoRegistry _algoRegistry;
    private readonly TextBlock _algosCountTextBlock;
    private readonly TextBlock _customIndicatorsCountTextBlock;
    private readonly TextBlock _standardIndicatorsCountTextBlock;
    private readonly TextBlock _botsCountTextBlock;
    private readonly TextBlock _pluginsCountTextBlock;

    public AlgoStatsControl(AlgoRegistry algoRegistry)
    {
        _algoRegistry = algoRegistry;
        
        var panel = new Grid(6, 2);

        var titleTextBlock = GetTextBlock("Algo Stats");

        titleTextBlock.HorizontalAlignment = HorizontalAlignment.Center;
        
        panel.AddChild(titleTextBlock, 0, 0, 1, 2);
        
        panel.AddChild(GetTextBlock("Algos #"), 1, 0);

        _algosCountTextBlock = GetTextBlock();
        
        panel.AddChild(_algosCountTextBlock, 1, 1);

        panel.AddChild(GetTextBlock("Standard Indicators #"), 2, 0);

        _standardIndicatorsCountTextBlock = GetTextBlock();
        
        panel.AddChild(_standardIndicatorsCountTextBlock, 2, 1);
        
        panel.AddChild(GetTextBlock("Custom Indicators #"), 3, 0);

        _customIndicatorsCountTextBlock = GetTextBlock();
        
        panel.AddChild(_customIndicatorsCountTextBlock, 3, 1);
        
        panel.AddChild(GetTextBlock("cBots #"), 4, 0);

        _botsCountTextBlock = GetTextBlock();
        
        panel.AddChild(_botsCountTextBlock, 4, 1);
        
        panel.AddChild(GetTextBlock("Plugins #"), 5, 0);

        _pluginsCountTextBlock = GetTextBlock();
        
        panel.AddChild(_pluginsCountTextBlock, 5, 1);
        
        AddChild(panel);
        
        Populate();
        
        _algoRegistry.AlgoTypeInstalled += _ => Populate();
        _algoRegistry.AlgoTypeDeleted += _ => Populate();
    }

    private void Populate()
    {
        _algosCountTextBlock.Text = _algoRegistry.Count.ToString();
        _botsCountTextBlock.Text = _algoRegistry.Count(type => type.AlgoKind == AlgoKind.Robot).ToString();
        _customIndicatorsCountTextBlock.Text = _algoRegistry.Count(type => type.AlgoKind == AlgoKind.CustomIndicator).ToString();
        _standardIndicatorsCountTextBlock.Text =  _algoRegistry.Count(type => type.AlgoKind == AlgoKind.StandardIndicator).ToString();
        _pluginsCountTextBlock.Text = _algoRegistry.Count(type => type.AlgoKind == AlgoKind.Plugin).ToString();
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