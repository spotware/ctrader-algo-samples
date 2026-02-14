// -------------------------------------------------------------------------------------------------
//
//    This code is a cTrader Algo API example.
//
//    The code is provided as a sample only and does not guarantee any particular outcome or profit of any kind. Use it at your own risk.
//    
//    This example cBot send messages to a Discord channel.
//
//    For a detailed tutorial on creating this cBot, watch the video at: https://youtu.be/NhEeySAKZUo
//
// -------------------------------------------------------------------------------------------------

using System;
using cAlgo.API;
using cAlgo.API.Collections;
using cAlgo.API.Indicators;
using cAlgo.API.Internals;
using Discord.WebSocket;
using Discord;

namespace cAlgo.Robots
{
    // Define the cBot attributes, such as AccessRights and its ability to add indicators.
    [Robot(AccessRights = AccessRights.None, AddIndicators = true)]
    public class DiscordMessageExample : Robot
    {
        // Parameters for the Discord bot token and channel ID.
        [Parameter("Discord Bot Token")]
        public string BotToken { get; set; }  // Discord bot token for authentication.

        [Parameter("Discord Channel ID")]
        public string ChannelID { get; set; }  // Channel ID where the message will be sent.

        // Declare private fields for the Discord client and channel.
        DiscordSocketClient _discordSocketClient;  // Discord client to interact with Discord API.
        IMessageChannel _channel;  // The channel where messages will be sent.

        // This method is called when the cBot starts.
        protected override void OnStart()
        {
            // Initialise the Discord client and log in using the bot token.
            _discordSocketClient = new DiscordSocketClient();
            _discordSocketClient.LoginAsync(TokenType.Bot, BotToken);  // Log into Discord using the bot token.
            _discordSocketClient.StartAsync();  // Start the Discord client asynchronously.

            // Convert the provided channel ID to ulong and get the channel to send messages to.
            var channelID = Convert.ToUInt64(ChannelID);
            _channel = _discordSocketClient.GetChannelAsync(channelID).Result as IMessageChannel;

            // Send a message indicating the cBot has started.
            _channel.SendMessageAsync("Example cBot Started");
        }

        // This method is called on every tick.
        protected override void OnTick()
        {
            // Handle price updates here
        }

        // This method is called when the cBot stops.
        protected override void OnStop()
        {
            // Handle cBot stop here
        }
    }
}
