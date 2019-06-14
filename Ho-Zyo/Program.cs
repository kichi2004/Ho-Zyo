using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;

namespace Ho_Zyo
{
    internal static class Program
    {
        private static readonly string Port = Environment.GetEnvironmentVariable("PORT");
        private static async Task Main(string[] args)
        {
            var url = $"http://localhost:{Port}/";
            
            using (var listener = new HttpListener())
            {
                listener.Prefixes.Add(url);
                listener.Start();
                while (true)
                {
                    var context = await listener.GetContextAsync();
                    using (var res = context.Response)
                    using (var stream = res.OutputStream)
                    using (var writer = new StreamWriter(stream))
                    {
                        await writer.WriteLineAsync("Ho-Zyo is Available!");
                    }
                }
            }
        }
    }
}