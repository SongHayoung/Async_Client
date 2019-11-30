using System;
using System.Threading;

using Gtk;
using Gdk;
using GTK_Demo_Client.DataHandler;
using GTK_Demo_Client.Popup;


public partial class MainWindow : Gtk.Window
{
	private static MainWindow mainwindow = new MainWindow();
	public MainWindow() : base(Gtk.WindowType.Toplevel)
	{
		Build();
	}
	public static MainWindow GetMainWindow()
	{
		return mainwindow;
	}

	protected void OnDeleteEvent(object sender, DeleteEventArgs a)
	{
		Application.Quit();
		a.RetVal = true;
	}

	protected void OnButton1Clicked(object sender, EventArgs e)
	{   //LoginButton
		string ID = entry1.Text.Trim();
		string Pass = entry2.Text.Trim();

		if (!LoginRequest(ID, Pass))
		{
			var request_fail = new Gtk.MessageDialog(null, Gtk.DialogFlags.Modal, Gtk.MessageType.Question,
										   Gtk.ButtonsType.Ok, "Fail");
			request_fail.Show();
			request_fail.Run();
			request_fail.Destroy();
			return;
		}

		GTK_Demo_Client.PopupWindow popup = new GTK_Demo_Client.PopupWindow();
		popup.Show();
		//ShowPopup();
	}


	protected void OnButton2Clicked(object sender, EventArgs e)
	{   //RegisterButton
		string ID = entry1.Text.Trim();
		string Pass = entry2.Text.Trim();

		if (!RegistRequest(ID, Pass))
		{
			var request_fail = new Gtk.MessageDialog(null, Gtk.DialogFlags.Modal, Gtk.MessageType.Question,
										   Gtk.ButtonsType.Ok, "Fail");
			request_fail.Show();
			request_fail.Run();
			request_fail.Destroy();
			return;
		}

		GTK_Demo_Client.PopupWindow popup = new GTK_Demo_Client.PopupWindow();
		popup.Show();
		//ShowPopup();
	}

	private void ShowPopup()
	{
		string result_msg = CPopupHandler.GetMessageFromServer();
		var result = new Gtk.MessageDialog(null, Gtk.DialogFlags.Modal, Gtk.MessageType.Question,
		                                   Gtk.ButtonsType.Ok, result_msg);
		result.Show();
		result.Run();
		result.Destroy();

		if (result_msg.CompareTo("로그인 성공") == 0)
		{
			GTK_Demo_Client.GameWindow GameWindow = new GTK_Demo_Client.GameWindow();
			GameWindow.Show();
			this.Hide();
			return;
		}

		if (result_msg.CompareTo("서버와의 연결이 끊겼습니다") == 0)
		{
			Gtk.Main.Quit();
			return;
		}

	}

	public bool LoginRequest(string ID, string Pass)
	{
		try
		{
			return CDataHandler.Handling_LoginRequest(ID, Pass);
		}
		catch (Exception e)
		{
			return false;
		}
	}

	public bool RegistRequest(string ID, string Pass)
	{
		try
		{
			return CDataHandler.Handling_RegisterRequest(ID, Pass);
		}
		catch (Exception e)
		{
			return false;
		}
	}
}
