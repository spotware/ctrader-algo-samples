// -------------------------------------------------------------------------------
//
//    This code is a cTrader Algo API example.
//
//    This cBot is intended to be used as a sample and does not guarantee any particular outcome or
//    profit of any kind. Use it at your own risk.
//    
//    All changes to this file might be lost on the next application update.
//    If you are going to modify this file please make a copy using the "Duplicate" command.
//
//    This cBot closes all profitable positions of the current account.
//
// -------------------------------------------------------------------------------

using cAlgo.API;

namespace cAlgo
{
    [Robot(TimeZone = TimeZones.UTC, AccessRights = AccessRights.None)]
    public class SampleCloseProfitablePositionscBot : Robot
    {
        protected override void OnStart()
        {
            foreach (var position in Positions)
                if (position.GrossProfit > 0)
                    ClosePosition(position);
        }
    }
}
