// -------------------------------------------------------------------------------------------------
//
//    This code is a cTrader Algo API example.
//
//    This code is intended to be used as a sample and does not guarantee any particular outcome or
//    profit of any kind. Use it at your own risk.
//    
//    The sample creates a custom control that displays statistics about registered algorithms in the 
//    platform. It shows counts for total algos, custom indicators, standard indicators, cBots and 
//    plugins. The statistics dynamically update when algo types are installed or deleted.
//
// -------------------------------------------------------------------------------------------------


using System.Linq;
using cAlgo.API;

namespace cAlgo.Plugins;

public class AlgoStatsControl: CustomControl
{
    private const string FontFamily = "Calibri";  // Define a constant for the font family used in the UI.
    private readonly AlgoRegistry _algoRegistry;  // Hold a reference to the Algo Registry for accessing algorithm data.

    private readonly TextBlock _algosCountTextBlock;  // TextBlock to display the total number of algorithms.
    private readonly TextBlock _customIndicatorsCountTextBlock;  // Number of custom indicators.
    private readonly TextBlock _standardIndicatorsCountTextBlock;  // Number of standard indicators.
    private readonly TextBlock _botsCountTextBlock;  // Number of cBots.
    private readonly TextBlock _pluginsCountTextBlock;  // Number of plugins.

    // This method initialises the control that displays stats for installed algorithms, including cBots, custom indicators and plugins.
    public AlgoStatsControl(AlgoRegistry algoRegistry)
    {
        _algoRegistry = algoRegistry;  // Assign the provided Algo Registry instance to the private field.
        
        var panel = new Grid(6, 2);  // Create a grid with 6 rows and 2 columns for organising UI elements.
        var titleTextBlock = GetTextBlock("Algo Stats");  // Create a title TextBlock with the text "Algo Stats".
        titleTextBlock.HorizontalAlignment = HorizontalAlignment.Center;  // Centre the title horizontally.       
        panel.AddChild(titleTextBlock, 0, 0, 1, 2);  // Add the title to the first row, spanning both columns.
        
        // Add a label and its corresponding TextBlock for "Algos #" to the grid.
        panel.AddChild(GetTextBlock("Algos #"), 1, 0);  // Add a label and its corresponding TextBlock for "Algos #" to the grid.
        _algosCountTextBlock = GetTextBlock();  // Create a TextBlock to display the total algorithm count.    
        panel.AddChild(_algosCountTextBlock, 1, 1);  // Add the TextBlock to the grid.

        // Add a label and TextBlock for "Standard Indicators #".
        panel.AddChild(GetTextBlock("Standard Indicators #"), 2, 0);
        _standardIndicatorsCountTextBlock = GetTextBlock();
        panel.AddChild(_standardIndicatorsCountTextBlock, 2, 1);
        
        // Add a label and TextBlock for "Custom Indicators #".
        panel.AddChild(GetTextBlock("Custom Indicators #"), 3, 0);
        _customIndicatorsCountTextBlock = GetTextBlock();
        panel.AddChild(_customIndicatorsCountTextBlock, 3, 1);
        
        // Add a label and TextBlock for "cBots #".
        panel.AddChild(GetTextBlock("cBots #"), 4, 0);
        _botsCountTextBlock = GetTextBlock();
        panel.AddChild(_botsCountTextBlock, 4, 1);
        
        // Add a label and TextBlock for "Plugins #".
        panel.AddChild(GetTextBlock("Plugins #"), 5, 0);
        _pluginsCountTextBlock = GetTextBlock();
        panel.AddChild(_pluginsCountTextBlock, 5, 1);
        
        AddChild(panel);  // Add the grid containing all UI elements to the control.
        
        Populate();  // Populate the TextBlocks with the initial statistics.
        
        // Subscribe to events to update statistics when an algorithm type is installed or deleted.
        _algoRegistry.AlgoTypeInstalled += _ => Populate();  // Call Populate() on algorithm installation.
        _algoRegistry.AlgoTypeDeleted += _ => Populate();  // Call Populate() on algorithm deletion.
    }

    // This method updates the statistics displayed in the TextBlocks.
    private void Populate()
    {
        _algosCountTextBlock.Text = _algoRegistry.Count.ToString();  // Set the total algorithm count.
        _botsCountTextBlock.Text = _algoRegistry.Count(type => type.AlgoKind == AlgoKind.Robot).ToString();  // Count cBots.
        _customIndicatorsCountTextBlock.Text = _algoRegistry.Count(type => type.AlgoKind == AlgoKind.CustomIndicator).ToString();  // Count custom indicators.
        _standardIndicatorsCountTextBlock.Text =  _algoRegistry.Count(type => type.AlgoKind == AlgoKind.StandardIndicator).ToString();  // Count standard indicators.
        _pluginsCountTextBlock.Text = _algoRegistry.Count(type => type.AlgoKind == AlgoKind.Plugin).ToString();  // Count plugins.
    }

    // This method creates and returns a new TextBlock with optional text.
    private TextBlock GetTextBlock(string text = null) => new()
    {
        Margin = 3,  // Set a margin around the TextBlock for spacing.
        FontSize = 20,  // Set the font size to 20.
        FontWeight = FontWeight.Bold,  // Set the font weight to bold.
        FontFamily = FontFamily,  // Use the defined font family.
        Text = text  // Set the text content, if provided.
    };
}
