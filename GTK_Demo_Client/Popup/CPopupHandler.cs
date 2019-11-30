using System;
using Gtk;

using GTK_Demo_Client.DataHandler;
namespace GTK_Demo_Client.Popup
{
	public class CPopupHandler
	{
		public CPopupHandler() { }

		public static void Run()
		{
			Console.WriteLine("Popup Manager on Active");
			Handling();
			Console.WriteLine("Popup Manager Join");
		}

		private static void Handling()
		{
			while (true)
			{
				string Popup_Message = CDataHandler.Handling_PopupMessage();
				if (Popup_Message == null)
					continue;
				PM_log("working");

				Gtk.MessageDialog dialog = new Gtk.MessageDialog(null, Gtk.DialogFlags.Modal, Gtk.MessageType.Question,
				                                   Gtk.ButtonsType.Ok, Popup_Message);
				dialog.Show();
				int status = dialog.Run();
				dialog.Destroy();
				if (status != (int)Gtk.ResponseType.Yes)
				{
					return;
				}
			}
		}

		private static void PM_log(string str)
		{
			Console.WriteLine("Popup Manager : " + str);
		}
	}
}
