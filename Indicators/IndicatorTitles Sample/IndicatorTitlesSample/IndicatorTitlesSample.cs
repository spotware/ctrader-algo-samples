/*
 * This sample uses the Chart.DisplaySettings.IndicatorTitles property
 * and prints its value on initialisation and whenever the user changes
 * the chart display options.
 */


namespace cAlgo;
using cAlgo.API;


[Indicator(AccessRights = AccessRights.None)]
public class IndicatorTitlesSample : Indicator
{

    protected override void Initialize()
    {
        // Adding a custom event handler for the DisplaySettingsChanged event
        Chart.DisplaySettingsChanged += OnDisplaySettingsChanged;
        
        Print(Chart.DisplaySettings.IndicatorTitles);
    }

    public override void Calculate(int index)
    {
        
    }
    
    protected void OnDisplaySettingsChanged(ChartDisplaySettingsEventArgs args) 
    {
        Print(Chart.DisplaySettings.IndicatorTitles);
    }
    
}