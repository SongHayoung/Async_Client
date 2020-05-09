using System;
using System.Net;
using System.Net.Sockets;
using System.Linq;

using GTK_Demo_Client.DataHandler;
using GTK_Demo_Packet;
namespace GTK_Demo_Client.Network
{
	public class CNetworkManager
	{
		private static Socket Client;
		private static string Server_IP = "127.0.0.1";
		private static int Server_Port = 5011;
		private static int Buf_Size = 1024 * 4;
		private static string Local_IP;

        /*
         * Set option for TCP KeepAlive
         */
		public class CTcpKeepAlive
		{
			public uint OnOff { get; set; }
			public uint KeepAliveTime { get; set; }
			public uint KeepAliveInterval { get; set; }

			public byte[] GetBytes()
			{
				return BitConverter.GetBytes(OnOff).Concat(BitConverter.GetBytes(KeepAliveTime))
					               .Concat(BitConverter.GetBytes(KeepAliveInterval)).ToArray();
			}
		}

		public CNetworkManager() { }

        /*
         * Set Socket options Connect to Server
         */
		public static void Run()
		{
			Console.WriteLine("Network Manager on Active");
			Local_IP = GetLocalIPAddress();

			Client = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
			var option = new CTcpKeepAlive
			{
				OnOff = 1,
				KeepAliveTime = 1000,
				KeepAliveInterval = 1000
			};
			Client.IOControl(IOControlCode.KeepAliveValues, option.GetBytes(), null);
			IPEndPoint IPEP = new IPEndPoint(IPAddress.Parse(Server_IP), Server_Port);
			SocketAsyncEventArgs Asynce = new SocketAsyncEventArgs();
			Asynce.RemoteEndPoint = IPEP;
			Asynce.Completed += new EventHandler<SocketAsyncEventArgs>(Connect);
			Client.ConnectAsync(Asynce);
			Handling();
			Client.Close();
			Console.WriteLine("Network Manager Join");
		}

        /*
         * Sending Packet to Server
         */
		public static void Handling()
		{
			
			while (MainClass.IsRunning())
			{
				byte[] buffer = CDataHandler.Handling_SendPacket();
				if (buffer == null)
					continue;
				
				NWM_log("Sending Packet");
				SocketAsyncEventArgs Asynce = new SocketAsyncEventArgs();
				Asynce.SetBuffer(buffer, 0, buffer.Length);
				Asynce.Completed += new EventHandler<SocketAsyncEventArgs>(Send);
				if (Client.Connected)
				{
					try
					{
						Client.SendAsync(Asynce);
					}
					catch (SocketException se)
					{
						Console.WriteLine("Socket Exception : " + se.ErrorCode + "Message : " + se.Message);
					}
				}
				else
				{
					CDataFactory DataFactory = CDataFactory.GetDataFactory();
					DataFactory.SetPopupBuffer("서버와의 연결이 끊겼습니다");
					MainClass.SetRunning(false);
				}
			}

		}

        /*
         * Get Local IP
         */
		public static string GetLocalIPAddress()
		{
			var host = Dns.GetHostEntry(Dns.GetHostName());
			foreach (var ip in host.AddressList)
			{
				if (ip.AddressFamily == AddressFamily.InterNetwork)
				{
					return ip.ToString();
				}
			}
			throw new Exception("No network adapters with an IPv4 address in the system");
		}

        /*
         * Connect Server Async
         */
		private static void Connect(object sender, SocketAsyncEventArgs e)
		{
			Socket client = (Socket)sender;
			if (client.Connected)
			{
				NWM_log("Connected with Server");
				byte[] buffer = new byte[Buf_Size];

				SocketAsyncEventArgs Asynce = new SocketAsyncEventArgs();
				Asynce.SetBuffer(buffer, 0, Buf_Size);
				Asynce.Completed += new EventHandler<SocketAsyncEventArgs>(Recieve);
				Client.ReceiveAsync(Asynce);
			}
		}

        /*
         * Do nothing
         */
		private static void Send(object sender, SocketAsyncEventArgs e)
		{
			Socket client = (Socket)sender;
			if (Client.Connected)
			{
				
			}
		}

        /*
         * Recieve Packet Async
         */
        private static void Recieve(object sender, SocketAsyncEventArgs e)
		{
			Socket client = (Socket)sender;
			if (Client.Connected && e.BytesTransferred > 0)
			{
				NWM_log("Receving Packet");
				byte[] buffer = e.Buffer;
				CDataHandler.Handling_RecvPacket(buffer);

				if (!client.ReceiveAsync(e))
				{
					client.ReceiveAsync(e);
				}
			}
			else
			{
				client.Close();
				Console.WriteLine("Connection Losted");
				CDataFactory DataFactory = CDataFactory.GetDataFactory();
				DataFactory.SetPopupBuffer("서버와의 연결이 끊겼습니다");
				MainClass.SetRunning(false);
			}
		}

		private static void NWM_log(string str)
		{
			Console.WriteLine("Network Manager : " + str);
		}
	}
}
