using System;
using Gtk;
using GTK_Demo_Client.DataHandler;

public partial class MainWindow : Gtk.Window
{
	public MainWindow() : base(Gtk.WindowType.Toplevel)
	{
		Build();
		PopupHandling();

	}

	private void PopupHandling()
	{
		while (true)
		{
			string Popup_Message = CDataHandler.Handling_PopupMessage();
			if (Popup_Message == null)
				continue;

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

	protected void OnDeleteEvent(object sender, DeleteEventArgs a)
	{
		Application.Quit();
		a.RetVal = true;
	}

	protected void OnButton1Clicked(object sender, EventArgs e)
	{   //LoginButton
		string ID = entry1.Text.Trim();
		string Pass = entry2.Text.Trim();

		CDataFactory DataFactory = CDataFactory.GetDataFactory();
		if (!LoginRequest(ID, Pass))
		{
			var dialog = new Gtk.MessageDialog(null, Gtk.DialogFlags.Modal, Gtk.MessageType.Question,
										   Gtk.ButtonsType.Ok, "Fail");
		}
	}


	protected void OnButton2Clicked(object sender, EventArgs e)
	{   //RegisterButton
		string ID = entry1.Text.Trim();
		string Pass = entry2.Text.Trim();

		if (!RegistRequest(ID, Pass))
		{
			var dialog = new Gtk.MessageDialog(null, Gtk.DialogFlags.Modal, Gtk.MessageType.Question,
										   Gtk.ButtonsType.Ok, "Fail");
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
