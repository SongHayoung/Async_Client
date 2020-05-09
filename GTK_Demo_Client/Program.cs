using System;
using System.Threading;

using Gtk;
using GTK_Demo_Client.DataHandler;
using GTK_Demo_Client.Network;
using GTK_Demo_Client.Popup;

namespace GTK_Demo_Client
{
	public enum Actions : int{
		Error = 0,
		Donothing,
		CloseWindow,
	}

	class MainClass
	{
		private static bool Running = true;
		private static string USERID = null;
		public static void Main(string[] args)
		{
			Thread NWManager = new Thread(new ThreadStart(CNetworkManager.Run));
			//Thread PopupManager = new Thread(new ThreadStart(CPopupHandler.Run));
			Thread HandlingManager = new Thread(new ThreadStart(CDataHandler.Run));

			NWManager.Start();
			//PopupManager.Start();
			HandlingManager.Start();

			Application.Init();
			MainWindow win = new MainWindow();
			win.Show();
			Application.Run();

			while (IsRunning()){ }
			CDataFactory DataFactory = CDataFactory.GetDataFactory();
			DataFactory.freeLock();

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
			Console.WriteLine("Running end");
		}
		public static void setUserID(string userid)
		{
			USERID = userid;
		}
		public static string getUserID()
		{
			return USERID;
		}
	}
}
