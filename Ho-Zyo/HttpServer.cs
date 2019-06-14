using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;

namespace Ho_Zyo
{
    public class HttpServer : IDisposable
    {
        private HttpListener _listener;

        public HttpServer(string prefix)
        {
            _listener = new HttpListener();
            _listener.Prefixes.Add(prefix);
        }

        public async Task Start()
        {
            _listener.Start();
            await ListenPort();
        }

        private async Task ListenPort()
        {
            while (true)
            {
                try
                {
                    var context = await _listener.GetContextAsync();
                    using (var res = context.Response)
                    using (var stream = res.OutputStream)
                    using (var writer = new StreamWriter(stream))
                    {
                        writer.WriteLine("Ho-Zyo is Available!");
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
            }
        }

        public void Dispose()
        {
            _listener.Close();
        }
    }
}