using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace WineCollector
{
    public class WineTcpServer
    {
        private TcpListener _listener;
        private bool _running;
        public string ServerName { get; set; } = "Moj podrum";

        public event Action<string> MessageReceived;

        public async Task StartAsync(int port = 7777)
        {
            _listener = new TcpListener(IPAddress.Any, port);
            _listener.Start();
            _running = true;

            Console.WriteLine($"TCP server sluša na portu {port}...");

            while (_running)
            {
                try
                {
                    TcpClient client = await _listener.AcceptTcpClientAsync();
                    _ = Task.Run(() => HandleClient(client));
                }
                catch { break; }
            }
        }

        private void HandleClient(TcpClient client)
        {
            using (client)
            {
                var stream = client.GetStream();
                var reader = new StreamReader(stream, Encoding.UTF8);
                var writer = new StreamWriter(stream, Encoding.UTF8) { AutoFlush = true };

                writer.WriteLine($"Dobrodošli u podrum '{ServerName}'!");

                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    Console.WriteLine($"Primljeno: {line}");
                    MessageReceived?.Invoke(line);
                    writer.WriteLine("OK");
                }
            }
        }

        public void Stop()
        {
            _running = false;
            _listener?.Stop();
        }
    }

    public class WineTcpClient
    {
        private TcpClient _client;
        private StreamWriter _writer;
        private StreamReader _reader;
        public string ClientName { get; set; } = "Kolekcionar";

        public event Action<string> MessageReceived;

        public async Task ConnectAsync(string host, int port = 7777)
        {
            _client = new TcpClient();
            await _client.ConnectAsync(host, port);

            var stream = _client.GetStream();
            _writer = new StreamWriter(stream, Encoding.UTF8) { AutoFlush = true };
            _reader = new StreamReader(stream, Encoding.UTF8);

            _ = Task.Run(Listen);
        }

        private void Listen()
        {
            string line;
            while ((line = _reader.ReadLine()) != null)
                MessageReceived?.Invoke(line);
        }

        public void SendWine(Wine wine)
        {
            var window = wine.GetDrinkingWindow();
            string msg = $"PREPORUKA: {wine.Name} ({wine.Vintage}) | {wine.Region} | " +
                         $"{window.Start}-{window.End} | {wine.GetMaturityStatus()}";
            _writer.WriteLine(msg);
        }

        public void Disconnect()
        {
            _writer?.Close();
            _reader?.Close();
            _client?.Close();
        }
    }
}