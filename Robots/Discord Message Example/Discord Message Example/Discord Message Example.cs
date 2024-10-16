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
    [Robot(AccessRights = AccessRights.None, AddIndicators = true)]
    public class DiscordMessageExample : Robot
    {
        [Parameter("Discord Bot Token")]
        public string BotToken { get; set; }

        [Parameter("Discord Channel ID")]
        public string ChannelID { get; set; }

        DiscordSocketClient _discordSocketClient;
        IMessageChannel _channel;

        protected override void OnStart()
        {
            _discordSocketClient = new DiscordSocketClient();
            _discordSocketClient.LoginAsync(TokenType.Bot, BotToken);
            _discordSocketClient.StartAsync();

            var channelID = Convert.ToUInt64(ChannelID);
            _channel = _discordSocketClient.GetChannelAsync(channelID).Result as IMessageChannel;
            _channel.SendMessageAsync("Example cBot Started");
        }

        protected override void OnTick()
        {
            // Handle price updates here
        }

        protected override void OnStop()
        {
            // Handle cBot stop here
        }
    }
}