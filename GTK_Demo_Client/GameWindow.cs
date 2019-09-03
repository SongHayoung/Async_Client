using System;
namespace GTK_Demo_Client
{
	public partial class GameWindow : Gtk.Window
	{
		public GameWindow() :
				base(Gtk.WindowType.Toplevel)
		{
			this.Build();
		}
	}
}
