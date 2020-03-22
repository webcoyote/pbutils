// Copyright (C) 2019 One More Game - All Rights Reserved
// Unauthorized copying of client file, via any medium is strictly prohibited
// Proprietary and confidential

using CommandLine;
using System;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;

namespace TNet {
    class Program {
        private const int DefaultConnectSeconds = 6;

        private class Options {
            [Value(0, MetaName = "hostname[:port]", HelpText = "IP address of remote computer.")]
            public string Host { get; set; }

            [Value(1, MetaName = "port", HelpText = "Port of remote computer.")]
            public int? Port { get; set; }

            [Option('t', "timeout", Required = false, HelpText = "Set connect timeout in seconds [default = 6 seconds]")]
            public int? Timeout { get; set; }
        }

        static void Main (string[] args) {
            Parser.Default.ParseArguments<Options>(args)
            .WithNotParsed(errors => {
                foreach (var error in errors)
                    Console.Error.WriteLine(error);
                Environment.Exit(100);
            })
            .WithParsed(async options => await TNet(options));
        }

        private static async Task TNet (Options options) {
            var host = options.Host;
            var port = options.Port.HasValue ? options.Port.Value : -1;
            var timeoutMs = (options.Timeout.HasValue ? options.Timeout.Value : DefaultConnectSeconds) * 1000;

            // There are two ways to specify a port, which ParseEndpoint handles
            // - HOSTNAME PORT
            // - HOSTNAME:PORT
            var addressParts = NetHelpers.ParseEndpoint(host, port);
            if (addressParts == null)
                Fatal("Invalid IPEndPoint {0}:{1}", host, port);
            host = addressParts.Item1;
            port = addressParts.Item2;

            var client = new TcpClient();
            client.BeginConnect(host, port, null, null).AsyncWaitHandle.WaitOne(timeoutMs);
            if (!client.Connected)
                Fatal("FATAL: unable to connect to {0}:{1}", host, port);
            Info("> TNet connected");

            var stream = client.GetStream();
            var cancelSource = new CancellationTokenSource();
            var cancel = cancelSource.Token;
            var receiveTask = Task.Run(async () => {
                try {
                    await stream.CopyToAsync(Console.OpenStandardOutput(), 32 * 1024);
                } catch { }
                cancelSource.Cancel();
            });

            while (client.Connected && !cancel.IsCancellationRequested) {
                var request = Console.ReadLine() + "\n";
                byte[] buf = System.Text.ASCIIEncoding.ASCII.GetBytes(request);
                try {
                    await stream.WriteAsync(buf, 0, buf.Length, cancel);
                } catch { }
            }

            Info("\n> TNet disconnected");
        }

        private static void Info (string format, params object[] args) {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.Error.WriteLine(format, args);
            Console.ResetColor();
        }

        private static void Warn (string format, params object[] args) {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Error.WriteLine(format, args);
            Console.ResetColor();
        }

        private static void Fatal (string format, params object[] args) {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Error.WriteLine(format, args);
            Console.ResetColor();
            Environment.Exit(1);
        }
    }
}
