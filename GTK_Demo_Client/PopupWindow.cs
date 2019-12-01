using System;


using GTK_Demo_Client.DataHandler;
namespace GTK_Demo_Client
{
	public partial class PopupWindow : Gtk.Window
	{

		string Popup_Message = "";
		public PopupWindow() :
				base(Gtk.WindowType.Toplevel)
		{
			this.Build();
			this.Hide();
			Run();
		}

		public PopupWindow(string msg) :
				base(Gtk.WindowType.Toplevel)
		{
			this.Build();
			Message.Text = msg;
		}

		private void Run()
		{
			while (MainClass.IsRunning())
			{
				Popup_Message = CDataHandler.Handling_PopupMessage();
				if (Popup_Message != null)
					break;
			}
			Message.Text = Popup_Message;
			this.Show();
		}

		protected void clicked(object sender, EventArgs e)
		{
			if(Popup_Message.CompareTo("로그인 성공") == 0)
			{
				GTK_Demo_Client.GameWindow GameWindow = new GTK_Demo_Client.GameWindow();
				GameWindow.Show();
				MainWindow mainwindow = new MainWindow();
				mainwindow.Destroy();
			}
			this.Destroy();
		}
	}
}
