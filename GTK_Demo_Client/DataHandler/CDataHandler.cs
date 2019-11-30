using System;

using GTK_Demo_Packet;
namespace GTK_Demo_Client.DataHandler
{
	public class CDataHandler
	{
		public CDataHandler() { }

		public static void Run()
		{
			while (true)
			{
				Console.WriteLine("Handling Manager on Active");
				Handling();
				Console.WriteLine("Handling Manager Join");
			}
		}

		public static void Handling()
		{
			CDataFactory DataFactory = CDataFactory.GetDataFactory();
			while (true)
			{
				byte[] buffer = DataFactory.GetRecvBuffer();
				if (buffer == null)
					continue;
				DH_log("working");

				PacketType packettype = Packet.GetPacketType(buffer);
				if (packettype == PacketType.Member_REGISTER_RESULT)
				{
					MemberRegisterResult result = (MemberRegisterResult)Packet.Deserialize(buffer);
					DataFactory.SetPopupBuffer(result.msg);
				}

				if (packettype == PacketType.Login_RESULT)
				{
					LoginResult result = (LoginResult)Packet.Deserialize(buffer);
					DataFactory.SetPopupBuffer(result.msg);
				}

			}
		}

		public static byte[] Handling_SendPacket()
		{
			CDataFactory DataFactory = CDataFactory.GetDataFactory();
			return DataFactory.GetSendBuffer();
		}

		public static bool Handling_RecvPacket(byte[] buffer)
		{
			CDataFactory DataFactory = CDataFactory.GetDataFactory();
			if (!DataFactory.SetRecvBuffer(buffer))
			{
				Console.WriteLine("DataHandler : RecvPacket Handling Fail");
				return false;
			}
			return true;
		}

		public static string Handling_PopupMessage()
		{
			CDataFactory DataFactory = CDataFactory.GetDataFactory();
			return DataFactory.GetPopupBuffer();
		}

		public static bool Handling_LoginRequest(string ID, string Pass)
		{
			CDataFactory DataFactory = CDataFactory.GetDataFactory();
			Login request = new Login();
			request.packet_Type = PacketType.Login;
			request.id_str = ID;
			request.pw_str = Pass;
			request.port_str = "20000";
			request.ip_str = "none";

			return DataFactory.SetSendBuffer(Packet.Serialize(request));
		}

		public static bool Handling_RegisterRequest(string ID, string Pass)
		{
			CDataFactory DataFactory = CDataFactory.GetDataFactory();
			MemberRegister request = new MemberRegister();
			request.packet_Type = PacketType.Member_REGISTER;
			request.id_str = ID;
			request.pw_str = Pass;
			return DataFactory.SetSendBuffer(Packet.Serialize(request));
		}

		private static void DH_log(string str)
		{
			Console.WriteLine("Handling Manager : " + str);
		}
	}
}
