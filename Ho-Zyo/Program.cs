using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;

namespace Ho_Zyo
{
    internal static class Program
    {
        private static readonly string Port = Environment.GetEnvironmentVariable("PORT");
        private static readonly string Addr = Environment.GetEnvironmentVariable("ADDRESS");

        private static async Task Main(string[] args)
        {
            var url = $"http://{Addr}:{Port}/";
            var server = new HttpServer(url);
            server.Start();
            await Task.Delay(-1);
        }
    }
}