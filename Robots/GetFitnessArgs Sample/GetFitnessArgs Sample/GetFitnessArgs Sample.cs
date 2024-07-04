// -------------------------------------------------------------------------------------------------
//
//    This code is a cTrader Algo API example.
//
//    This cBot is intended to be used as a sample and does not guarantee any particular outcome or
//    profit of any kind. Use it at your own risk.
//
// -------------------------------------------------------------------------------------------------

using cAlgo.API;

namespace cAlgo.Robots
{
    [Robot(TimeZone = TimeZones.UTC, AccessRights = AccessRights.None)]
    public class GetFitnessArgsSample : Robot
    {
        protected override void OnStart()
        {
        }

        protected override double GetFitness(GetFitnessArgs args)
        {
            // Here we are using the win rate as fitness
            // You can use any other value by combining the values of GetFitnessArgs object properties
            return args.WinningTrades / args.TotalTrades;
        }
    }
}