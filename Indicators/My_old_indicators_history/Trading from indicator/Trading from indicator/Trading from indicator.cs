using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using cAlgo.API;
using cAlgo.API.Collections;
using cAlgo.API.Indicators;
using cAlgo.API.Internals;

namespace cAlgo
{
    [Indicator(AccessRights = AccessRights.None)]
    public class Tradingfromindicator : Indicator
    {


        protected override void Initialize()
        {

            if (Positions.Count() != 0)
            {
                Print("Number of opened positions: {0}", Positions.Count());
                Print("Last position: {0}", Positions.Last());
                Print("Process of closing the position...");
                var position = Positions.Last();

                try
                {
                    position.Close();
                }
                catch (Exception ex)
                {
                    Print("An exception occurred when closing the position: {0}", ex.Message);
                }
            }

            if (PendingOrders.Count() != 0)
            {
                Print("Number of orders: {0}", PendingOrders.Count());
                Print("Last orders: {0}", PendingOrders.Last());
                Print("Process of closing the order...");
                var order = PendingOrders.Last();

                try
                {
                    order.Cancel();
                }
                catch (Exception ex)
                {
                    Print("An exception occurred when canceling the order: {0}", ex.Message);
                }
            }
            else
            {
                Print("No open positions or orders");
            }
        }

        public override void Calculate(int index)
        {

        }
    }
}
