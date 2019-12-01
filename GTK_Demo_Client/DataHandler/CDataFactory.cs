using System.Collections.Generic;
using System.Threading;

using GTK_Demo_Packet;

namespace GTK_Demo_Client.DataHandler
{
	public class CDataFactory
	{
		private static CDataFactory DataFactory = new CDataFactory();
		private Stack<byte[]> Send_buffer = new Stack<byte[]>();
		private Stack<byte[]> Recv_buffer = new Stack<byte[]>();
		private Stack<string> Popup_buffer = new Stack<string>();

		private static object Recv_Lock = new object();
		private static object Send_Lock = new object();
		private static object Popup_Lock = new object();

		public CDataFactory() { }

        /*
         * return DataFactory
         */
		public static CDataFactory GetDataFactory()
		{
			return DataFactory;
		}

        /*
         * set buffer at SendBuffer
         */
		public bool SetSendBuffer(byte[] buffer)
		{
			lock(Send_Lock)
			{
				Send_buffer.Push(buffer);
			}
			return true;
		}

        /*
         * get buffer from SendBuffer
         */
		public byte[] GetSendBuffer()
		{
			byte[] buffer;
			lock(Send_Lock)
			{
				buffer = Send_buffer.Count > 0 ? Send_buffer.Pop() : null;
			}
			return buffer;
		}

        /*
         * set buffer at RecvBuffer
         */
		public bool SetRecvBuffer(byte[] buffer)
		{
			lock (Recv_Lock)
			{
				Recv_buffer.Push(buffer);
			}
			return true;
		}

        /*
         * get buffer from RecvBuffer
         */
		public byte[] GetRecvBuffer()
		{
			byte[] buffer;
			lock(Recv_Lock)
			{
				buffer = Recv_buffer.Count > 0 ? Recv_buffer.Pop() : null;
			}
			return buffer;
		}

        /*
         * set message at PopupBuffer
         */
		public bool SetPopupBuffer(string buffer)
		{
			lock(Popup_Lock)
			{
				Popup_buffer.Push(buffer);
			}
			return true;
		}

        /*
         * get message from PopupBuffer
         */
		public string GetPopupBuffer()
		{
			string buffer;
			lock(Popup_Lock)
			{
				buffer = Popup_buffer.Count > 0 ? Popup_buffer.Pop() : null;
			}
			return buffer;
		}

	}
}
