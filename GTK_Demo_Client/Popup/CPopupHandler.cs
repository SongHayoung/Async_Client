using System;

/*
 * Do nothing
 */
using GTK_Demo_Client.DataHandler;
namespace GTK_Demo_Client.Popup
{
	public class CPopupHandler
	{
		public CPopupHandler(){ }

        /*
         * Do nothing
         */
		public static void Run()
		{
			Console.WriteLine("Popup Manager on Active");
			Handling();
			Console.WriteLine("Popup Manager Join");
		}

        /*
         * Do nothing
         */
		private static void Handling()
		{
			string Popup_Message;
			while (MainClass.IsRunning())
			{
				Popup_Message = CDataHandler.Handling_PopupMessage();
				if (Popup_Message == null)
					continue;
				PM_log("working");
				PM_log(Popup_Message);
			}
		}

        /*
         * Do nothing
         */
		private static void PM_log(string str)
		{
			Console.WriteLine("Popup Manager : " + str);
		}

        /*
         * Do nothing
         */
		public static string GetMessageFromServer()
		{
			string Message;
			while (true)
			{
				Message = CDataHandler.Handling_PopupMessage();
				if (Message == null)
					continue;

				PM_log(Message);
				break;
			}
			return Message;
		}
	}
}
