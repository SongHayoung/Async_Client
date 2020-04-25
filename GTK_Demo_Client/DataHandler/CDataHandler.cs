using System;

using GTK_Demo_Packet;
namespace GTK_Demo_Client.DataHandler
{
	public class CDataHandler
	{
		public CDataHandler() { }

        /*
         * Running DataHandler
         */
		public static void Run()
		{
			Console.WriteLine("Handling Manager on Active");
			Handling();
			Console.WriteLine("Handling Manager Join");
		}

        /*
         * this function helps moving Recv Sessions to right Queue at DataFactory
         */
        public static void Handling()
		{
			CDataFactory DataFactory = CDataFactory.GetDataFactory();
			while (MainClass.IsRunning())
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
				if (packettype == PacketType.Heart_Beat)
				{
					DH_log("Received HB packet");
					DataFactory.SetSendBuffer(buffer);
					DH_log("Hearbeat received");
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

        /*
         * Initilize Login Packet send to DataHandler
         */
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

        /*
         * Initilize MemberRegister Packet send to DataHandler
         */
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
