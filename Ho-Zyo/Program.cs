using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;

namespace Ho_Zyo
{
    internal class Program
    {
        private readonly string _port = Environment.GetEnvironmentVariable("PORT");
        private async Task Main(string[] args)
        {
            var url = $"http://localhost:{_port}/";
            
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