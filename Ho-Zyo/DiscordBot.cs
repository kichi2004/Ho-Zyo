using System;
using System.Reflection;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;

namespace Ho_Zyo
{
    public class DiscordBot
    {
        private readonly string _token = Environment.GetEnvironmentVariable("DISCORD_TOKEN");
        private readonly DiscordSocketClient _client;
        private readonly CommandService _commands;
        private readonly IServiceProvider _services;

        public DiscordBot()
        {
            _client = new DiscordSocketClient();
            _commands = new CommandService();
            _services = new ServiceCollection().BuildServiceProvider();
        }

        public async Task Start()
        {
            _client.MessageReceived += OnReceiveCommandAsync;
            _client.Log += OnSendLog;

            await _commands.AddModulesAsync(Assembly.GetEntryAssembly(), _services);
            await _client.LoginAsync(TokenType.Bot, _token);
            await _client.StartAsync();
        }

        private static Task OnSendLog(LogMessage arg)
        {
            Console.WriteLine($"[{DateTime.Now.Millisecond}]{arg.Message}");
            return Task.CompletedTask;
        }

        private async Task OnReceiveCommandAsync(SocketMessage msgParam)
        {
            if (!(msgParam is SocketUserMessage msg))
            {
                return;
            }

            var argPos = 0;
            if (msg.HasCharPrefix('!', ref argPos) == false ||
                msg.HasMentionPrefix(_client.CurrentUser, ref argPos))
            {
                return;
            }

            var context = new CommandContext(_client, msg);
            await _commands.ExecuteAsync(context, argPos, _services);
        }
    }
}