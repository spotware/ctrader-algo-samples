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
    // Define the cBot attributes, including AccessRights.
    [Robot(AccessRights = AccessRights.FullAccess)]
    public class CompilationRobot : Robot
    {

        // This method is called when the bot starts.
        protected override void OnStart()
        {
            // Initialise compilation options with custom settings for the algo file.
            CompilationOptions options = new CompilationOptions
            {
                IncludeSourceCode = true,  // Set to include the source code in the compiled output.
                OutputAlgoFilePath = @"C:\Users\{preferred path}\NameOfAlgo.algo"  // Specify the path for saving the compiled algo file.
            };

            // Compile the specified .csproj file with the defined options.
            CompilationResult resultSync = Compiler.Compile(@"C:\Users\{path to project}\NameOfCbot.csproj", options);

            // Output compilation success or failure to the log.
            Print(resultSync.Succeeded);
        }
    }
}
