using System;
using System.Threading;

using Gtk;
using GTK_Demo_Client.DataHandler;
using GTK_Demo_Client.Network;
using GTK_Demo_Client.Popup;

namespace GTK_Demo_Client
{
	public enum Actions : int
	{
		Error = 0,
		Donothing,
		CloseWindow,
	}

	class MainClass
	{
		private static bool Running = true;

		public static void Main(string[] args)
		{
			Thread NWManager = new Thread(new ThreadStart(CNetworkManager.Run));
			//Thread PopupManager = new Thread(new ThreadStart(CPopupHandler.Run));
			Thread HandlingManager = new Thread(new ThreadStart(CDataHandler.Run));

			NWManager.Start();
			//PopupManager.Start();
			HandlingManager.Start();

			Application.Init();
			MainWindow win = MainWindow.GetMainWindow();
			win.Show();
			Application.Run();

			while (IsRunning())
			{ }

			NWManager.Join();
			HandlingManager.Join();
		}

		public static bool IsRunning()
		{
			return Running;
		}

		public static void SetRunning(bool status)
		{
			Running = status;
		}
	}
}
