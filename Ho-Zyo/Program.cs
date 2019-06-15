using System;
using System.Threading.Tasks;

namespace Ho_Zyo
{
    internal static class Program
    {
        private static readonly string Port = Environment.GetEnvironmentVariable("PORT");
        private static readonly string Addr = Environment.GetEnvironmentVariable("ADDRESS");

        private static async Task Main(string[] args)
        {
            var bot = new DiscordBot();
            await bot.Start();

#if RELEASE
            var url = $"http://{Addr}:{Port}/";
            var server = new HttpServer(url);
            await server.Start();
#endif
            while (true)
            {
                var command = Console.ReadLine();
                if (command != "save" && command != "exit") continue;
                DiscordBot.SaveSettings();
                if (command == "exit") break;
            }
        }
    }
}