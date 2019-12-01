using System;
using System.ComponentModel;

using Gtk;
using GTK_Demo_Client.DataHandler;
using GTK_Demo_Client.Popup;


public partial class MainWindow : Gtk.Window
{
	private BackgroundWorker PopupWorker;
	public MainWindow() : base(Gtk.WindowType.Toplevel)
	{
		Build();
		InitializeBackgroundWorker();
	}

    /*
     * initializing BackgroundWorker Thread
     */
	private void InitializeBackgroundWorker()
	{
		PopupWorker = new BackgroundWorker();
		PopupWorker.DoWork += new DoWorkEventHandler(Popup_Dowork);
		PopupWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(Popup_Completed);

	}

    /*
     * BackgroundWorker Dowork
     */
	private void Popup_Dowork(object sender, DoWorkEventArgs e)
	{
		BackgroundWorker worker = (BackgroundWorker)sender;
		string Popup_Message = "";
		Console.WriteLine("Background Worker Do Process");
		while (true)
		{
			Popup_Message = CDataHandler.Handling_PopupMessage();
			if (Popup_Message == null)
				continue;
			break;
		}
		e.Result = Popup_Message;
	}

    /*
     * BackgroundWorker RunWorkerCompleted
     */
	private void Popup_Completed(object sender, RunWorkerCompletedEventArgs e)
	{
		Console.WriteLine("Background Worker Job Done : "+e.Result.ToString());
		Gtk.Application.Invoke(delegate {
			ShowPopup(e.Result.ToString());
		});
	}

    /*
     * this function show popup message came from server
     */
	public void ShowPopup(string msg)
	{
		var Popup = new Gtk.MessageDialog(null, Gtk.DialogFlags.Modal, Gtk.MessageType.Question,
										   Gtk.ButtonsType.Ok, msg);
		Popup.Show();
		Popup.Run();
		Popup.Destroy();

		if (msg.CompareTo("로그인 성공") == 0)
		{
			GTK_Demo_Client.GameWindow GameWindow = new GTK_Demo_Client.GameWindow();
			GameWindow.Show();
			this.Hide();
		}
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
		if(!PopupWorker.IsBusy)
			PopupWorker.RunWorkerAsync();
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
		if(!PopupWorker.IsBusy)
			PopupWorker.RunWorkerAsync();
	}

	/*
	 * Not use
	 */
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

    /*
     * send ID and Pass to DataHandler
     */
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

    /*
     * send ID and Pass to DataHandler
     */
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
