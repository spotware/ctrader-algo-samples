using cAlgo.API;

namespace cAlgo.Robots
{
    /// <summary>
    /// This sample shows how to use the GetFitnessArgs to change the default fitness metric of optimizer
    /// </summary>
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