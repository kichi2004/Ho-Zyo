﻿using System;
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
            var bot = new DiscordBot();
            await bot.Start();
            
            var url = $"http://{Addr}:{Port}/";
            var server = new HttpServer(url);
            await server.Start();
            
            await Task.Delay(-1);
        }

        private static async void StartServer()
        {
            
        }
    }
}