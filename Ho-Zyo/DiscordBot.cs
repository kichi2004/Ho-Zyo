using System;
using System.Linq;
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
        private const char Prefix = '!';

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
            Console.WriteLine($"[{DateTime.Now}]{arg.Message}");
            return Task.CompletedTask;
        }

        private async Task OnReceiveCommandAsync(SocketMessage socketMessage)
        {
            if (!(socketMessage is SocketUserMessage socketUserMessage))
            {
                return;
            }

            if (_commands.Commands.Any(x => x.Name.ToLower() != socketUserMessage.Content.Split()[0]) && //コマンドが存在するか
                socketUserMessage.Author.IsBot == false)                                                 //bot以外からの発言か
            {
                await socketUserMessage.AddReactionAsync(new Emoji("\uD83E\uDD14"));
                return;
            }

            var argPos = 0;
            if (socketUserMessage.HasCharPrefix(Prefix, ref argPos) &&
                socketUserMessage.Channel.Name != "bot" && //#botチャンネル以外で呼ばれたか  
                socketUserMessage.Author.IsBot == false)   //そのコマンドはbot以外によるものか
            {
                await socketUserMessage.Channel.SendMessageAsync(
                    $"{socketUserMessage.Author.Mention} #bot チャンネルで話してね？");
                return;
            }

            argPos = 0;
            if (socketUserMessage.HasCharPrefix(Prefix, ref argPos) == false ||
                socketUserMessage.HasMentionPrefix(_client.CurrentUser, ref argPos))
            {
                return;
            }

            var context = new CommandContext(_client, socketUserMessage);
            await _commands.ExecuteAsync(context, argPos, _services);
        }
    }
}