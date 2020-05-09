using System;
using GTK_Demo_Packet;
using GTK_Demo_Client.DataHandler;

namespace GTK_Demo_Client
{
	public partial class GameWindow : Gtk.Window
	{
		public GameWindow() :
				base(Gtk.WindowType.Toplevel)
		{
			this.Build();
		}
		protected void OnDeleteEvent(object o, Gtk.DeleteEventArgs args)
		{
			this.Destroy();
			Gtk.Application.Quit();
			CDataFactory DataFactory = CDataFactory.GetDataFactory();
			ClosePacket close = new ClosePacket(MainClass.getUserID());

			DataFactory.SetSendBuffer(Packet.Serialize(close));

			MainClass.SetRunning(false);
		}
	}
}
