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
            FleckLog.Level = LogLevel.Info;
            var allsockets = new List<IWebSocketConnection>();
            var server = new WebSocketServer("ws://localhost:8181");

            server.Start(socket =>
            {
                socket.OnOpen = () =>
                    {   //See socket.ConnectionInfo.* for additional informations
                        Console.WriteLine(String.Empty);
                        Console.WriteLine("[NEW CLIENT CONNECTION]======================");
                        Console.WriteLine("GUID: " + socket.ConnectionInfo.Id);
                        Console.WriteLine("IP: " + socket.ConnectionInfo.ClientIpAddress);
                        Console.WriteLine("Port: " + socket.ConnectionInfo.ClientPort);
                        Console.WriteLine("=============================================");
                        Console.WriteLine(String.Empty);
                        allsockets.Add(socket);

                    };

                socket.OnClose = () =>
                {
                        Console.WriteLine(String.Empty);
                        Console.WriteLine("[DISCONNECTED CLIENT]=======================");
                        Console.WriteLine("GUID: " + socket.ConnectionInfo.Id);
                        Console.WriteLine("IP: " + socket.ConnectionInfo.ClientIpAddress);
                        Console.WriteLine("Port: " + socket.ConnectionInfo.ClientPort);
                        Console.WriteLine("=============================================");
                        Console.WriteLine(String.Empty);
                        allsockets.Remove(socket);
                    };

                socket.OnMessage = (message) =>
                {
                    //TODO: Json.Net Deserialize
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
