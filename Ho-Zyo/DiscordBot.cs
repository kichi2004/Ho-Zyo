using System;
using System.IO;
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
        private const char PREFIX = '!';
        public const string SETTINGS_PATH = "./DiscordBotSettings.json";

        public DiscordBot()
        {
            _client = new DiscordSocketClient();
            _commands = new CommandService();
            _services = new ServiceCollection().BuildServiceProvider();
            InitSettingsFile();
        }

        public async Task Start()
        {
            _client.MessageReceived += OnReceiveCommandAsync;
            _client.Log += OnSendLog;

            await _commands.AddModulesAsync(Assembly.GetEntryAssembly(), _services);
            await _client.LoginAsync(TokenType.Bot, _token);
            await _client.StartAsync();
        }

        private static void InitSettingsFile()
        {
            if (File.Exists(SETTINGS_PATH)) return;
            using (var writer = new StreamWriter(SETTINGS_PATH))
            {
                writer.Write(new Settings().ToJson());
            }
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

            var argPos = 0;

            // コマンドかどうか
            if (socketUserMessage.HasCharPrefix(PREFIX, ref argPos) == false ||
                socketUserMessage.HasMentionPrefix(_client.CurrentUser, ref argPos))
            {
                return;
            }

            if (_commands.Commands.Any(x => x.Name.ToLower() ==
                                            socketUserMessage.Content.Split()[0].Substring(1)) == false && // コマンドが存在するか
                socketUserMessage.Author.IsBot == false
            ) // そのコマンドはbot以外によるものか
            {
                await socketUserMessage.AddReactionAsync(new Emoji("\uD83E\uDD14"));
                return;
            }

            // botチャンネルかどうか
            if (socketUserMessage.Channel.Name != "bot" && //#botチャンネル以外で呼ばれたか  
                socketUserMessage.Author.IsBot == false)   //そのコマンドはbot以外によるものか
            {
                await socketUserMessage.Channel.SendMessageAsync(
                    $"{socketUserMessage.Author.Mention} #bot チャンネルで話してね？");
                return;
            }

            var context = new CommandContext(_client, socketUserMessage);
            await _commands.ExecuteAsync(context, argPos, _services);
        }
    }
}