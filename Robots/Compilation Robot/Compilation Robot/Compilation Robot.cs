// -------------------------------------------------------------------------------------------------
//
//    This code is a cTrader Algo API example.
//
//    The code is provided as a sample only and does not guarantee any particular outcome or profit of any kind. Use it at your own risk.
//    
//    This example cBot generates and saves algorithms by compiling .csproj files.
//
// -------------------------------------------------------------------------------------------------




using System;
using cAlgo.API;
using cAlgo.API.Collections;
using cAlgo.API.Indicators;
using cAlgo.API.Internals;

namespace cAlgo.Robots
{
    [Robot(AccessRights = AccessRights.FullAccess)]
    public class CompilationRobot : Robot
    {

        protected override void OnStart()
        {
            CompilationOptions options = new CompilationOptions
            {
                IncludeSourceCode = true,
                OutputAlgoFilePath = @"C:\Users\{preferred path}\NameOfAlgo.algo"
            };

            CompilationResult resultSync = Compiler.Compile(@"C:\Users\{path to project}\NameOfCbot.csproj", options);
            Print(resultSync.Succeeded);
        }
    }
}
