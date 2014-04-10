using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fleck;

namespace Websocket
{
    class Program
    {
        static void Main(string[] args)
        {
            FleckLog.Level = LogLevel.Debug;
            var allsockets = new List<IWebSocketConnection>();
            var server = new WebSocketServer("ws://localhost:8181");

            server.Start(socket =>
            {
                socket.OnOpen = () =>
                    {
                        Console.WriteLine("Open");
                        allsockets.Add(socket);

                    };

                socket.OnClose = () =>
                    {
                        Console.WriteLine("Closed connection");
                        allsockets.Remove(socket);
                    };

                socket.OnMessage = (message) =>
                {
                    Console.WriteLine("[JSON MESSAGE] " + message);
                    allsockets.ToList().ForEach(s => s.Send(message));
                };
            });

            var input = Console.ReadLine();
            while (input != "exit")
            {
                foreach (var socket in allsockets.ToList())
                {
                    socket.Send(input);
                }

                input = Console.ReadLine();
            }
        }
    }
}
