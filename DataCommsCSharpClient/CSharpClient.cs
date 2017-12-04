/* CSharpClient.cs
 * 
 * (C) 2017 Michael Ranciglio
 * 
 * Prepared for CS480 at Southeast Missouri State University
 * 
 * Modeled after the DataCommsCppServer.
 */

using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace DataCommsCSharpClient
{
	class CSharpClient
	{
		static void Main(string[] args)
		{
			if (args.Length < 3 || args.Length > 4) { //check for correct args
				Console.WriteLine("usage: WebClient <compname> <path> [appnum]");
				System.Environment.Exit(1);
			}

			IPHostEntry serverInfo = Dns.GetHostEntry(args[0]); //using IPHostEntry support both host name and host IPAddress inputs
			IPAddress[] serverIPaddr = serverInfo.AddressList; //addresslist may contain both IPv4 and IPv6 addresses

			int recv;
			byte[] data = new byte[1024];
			string stringData;
			Socket server;
			server = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

			try {
				server.Connect(serverIPaddr, Int32.Parse(args[2]));
			} catch (SocketException e) {
				Console.WriteLine("Unable to connect to server.");
				Console.WriteLine(e.ToString());
				return;
			}

			Console.WriteLine("Connected to " + server.ToString());

			Console.WriteLine("Requesting " + args[1] + " from the server.");
			string GETstr = "GET " + args[1] + " HTTP/1.0\r\n\r\n";
			server.Send(Encoding.ASCII.GetBytes(GETstr));
			bool whileVar = true;

			while (whileVar) {
				data = new byte[1024];
				recv = server.Receive(data);
				stringData = Encoding.ASCII.GetString(data, 0, recv);
				if (stringData.Contains("\r\n\r\n"))
					whileVar = false;
				Console.WriteLine(stringData);
			}

			Console.WriteLine("Disconnecting from server...");
			server.Shutdown(SocketShutdown.Both);
			server.Close();
		}
	}
}
