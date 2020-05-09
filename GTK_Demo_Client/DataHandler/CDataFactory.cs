using System.Collections.Generic;
using System.Threading;

using GTK_Demo_Packet;

namespace GTK_Demo_Client.DataHandler
{
	public class CDataFactory
	{
		private static CDataFactory DataFactory = new CDataFactory();
		private Queue<byte[]> Send_buffer = new Queue<byte[]>();
		private Queue<byte[]> Recv_buffer = new Queue<byte[]>();
		private Queue<string> Popup_buffer = new Queue<string>();

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
				Send_buffer.Enqueue(buffer);
				System.Threading.Monitor.Pulse(Send_Lock);
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
				buffer = Send_buffer.Count > 0 ? Send_buffer.Dequeue() : null;
				if(buffer==null)
                    System.Threading.Monitor.Wait(Send_Lock);
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
				Recv_buffer.Enqueue(buffer);
				System.Threading.Monitor.Pulse(Recv_Lock);
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
				buffer = Recv_buffer.Count > 0 ? Recv_buffer.Dequeue() : null;
				if(buffer==null)
					System.Threading.Monitor.Wait(Recv_Lock);
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
				Popup_buffer.Enqueue(buffer);
				System.Threading.Monitor.Pulse(Popup_Lock);
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
				buffer = Popup_buffer.Count > 0 ? Popup_buffer.Dequeue() : null;
				if(buffer==null)
					System.Threading.Monitor.Wait(Popup_Lock);
			}
			return buffer;
		}

		public void freeLock()
		{
			lock (Popup_Lock)
				System.Threading.Monitor.Pulse(Popup_Lock);
			lock (Recv_Lock)
				System.Threading.Monitor.Pulse(Recv_Lock);
			lock (Send_Lock)
				System.Threading.Monitor.Pulse(Send_Lock);
		}

	}
}
