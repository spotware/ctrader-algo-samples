// -------------------------------------------------------------------------------------------------
//
//    This code is a cTrader Algo API example.
//
//    This cBot is intended to be used as a sample and does not guarantee any particular outcome or
//    profit of any kind. Use it at your own risk.
//
//    This sample cBot demonstrates the use of the GetFitness method to evaluate performance metrics.
//    The fitness value is calculated based on the win rate.
//
// -------------------------------------------------------------------------------------------------

using cAlgo.API;

namespace cAlgo.Robots
{
    // Define the cBot attributes, including timezone and access rights.
    [Robot(TimeZone = TimeZones.UTC, AccessRights = AccessRights.None)]
    public class GetFitnessArgsSample : Robot
    {
        // This method is called when the bot starts and is used for initialization, if needed.
        protected override void OnStart()
        {
        }

        // This method is used to calculate the fitness value based on trading performance.
        protected override double GetFitness(GetFitnessArgs args)
        {
            // Here we are using the win rate as fitness.
            // You can use any other value by combining the values of GetFitnessArgs object properties.
            return args.WinningTrades / args.TotalTrades;
        }
    }
}
