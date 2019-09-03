using System;
using Gtk;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.Windows;
using GTK_Demo_Packet;

public partial class MainWindow : Gtk.Window
{
	string Server_IP = "172.16.38.199";
	public MainWindow() : base(Gtk.WindowType.Toplevel)
	{
		Build();
	}

	protected void OnDeleteEvent(object sender, DeleteEventArgs a)
	{
		Application.Quit();
		a.RetVal = true;
	}

	protected void OnButton1Clicked(object sender, EventArgs e)
	{
		try
		{
			byte[] buffer = new byte[1024 * 4];

			TcpClient client = new TcpClient(Server_IP, 5011);
			NetworkStream stream = client.GetStream();

			Login login = new Login();
			login.packet_Type = (int)PacketType.Login;
			login.id_str = entry1.Text.Trim();
			login.pw_str = entry2.Text.Trim();

			Console.WriteLine("user id : {0}", login.id_str);
			Console.WriteLine("user pass : {0}", login.pw_str);

			Packet.Serialize(login).CopyTo(buffer, 0);
			stream.Write(buffer, 0, buffer.Length);
			Array.Clear(buffer, 0, buffer.Length);
			int bytesRead = stream.Read(buffer, 0, buffer.Length);
			LoginResult loginResult = (LoginResult)Packet.Deserialize(buffer);

			if (loginResult.result)
			{
				var dialog = new Gtk.MessageDialog(null, Gtk.DialogFlags.Modal, Gtk.MessageType.Question,
											   Gtk.ButtonsType.Ok, "Success");
				dialog.Show();
				int r = dialog.Run();
				dialog.Destroy();
				if (r != (int)Gtk.ResponseType.Yes)
				{
					GTK_Demo_Client.GameWindow GameWindow = new GTK_Demo_Client.GameWindow();
					GameWindow.Show();
					this.Hide();
					stream.Close();
					client.Close();

					return; 
				}
			}
			else
			{
				var dialog = new Gtk.MessageDialog(null, Gtk.DialogFlags.Modal, Gtk.MessageType.Question,
											   Gtk.ButtonsType.Ok, "fail");
				dialog.Show();
				int r = dialog.Run();
				dialog.Destroy();
				if (r != (int)Gtk.ResponseType.Yes)
				{ 
					stream.Close();
					client.Close();
					return; 
				}
			}
		}
		catch (Exception error)
		{
			var dialog = new Gtk.MessageDialog(null, Gtk.DialogFlags.Modal, Gtk.MessageType.Question,
											   Gtk.ButtonsType.Ok, "error");
			dialog.Show();
			int r = dialog.Run();
			dialog.Destroy();
			if (r != (int)Gtk.ResponseType.Yes)
			{ return; }
		}
	}

	protected void OnButton2Clicked(object sender, EventArgs e)
	{
		try
		{
			byte[] buffer = new byte[1024 * 4];

			TcpClient client = new TcpClient(Server_IP, 5011);
			NetworkStream stream = client.GetStream();

			MemberRegister mr = new MemberRegister();
			mr.packet_Type = (int)PacketType.Member_REGISTER;
			mr.id_str = entry1.Text.Trim();
			mr.pw_str = entry2.Text.Trim();

			Console.WriteLine("user id : {0}", mr.id_str);
			Console.WriteLine("user pass : {0}", mr.pw_str);

			Packet.Serialize(mr).CopyTo(buffer, 0);
			stream.Write(buffer, 0, buffer.Length);
			Array.Clear(buffer, 0, buffer.Length);
			int bytesRead = stream.Read(buffer, 0, buffer.Length);
			MemberRegisterResult mrResult = (MemberRegisterResult)Packet.Deserialize(buffer);

			if (mrResult.result)
			{
				var dialog = new Gtk.MessageDialog(null, Gtk.DialogFlags.Modal, Gtk.MessageType.Question,
											   Gtk.ButtonsType.Ok, "Success");
				dialog.Show();
				int r = dialog.Run();
				dialog.Destroy();
				if (r != (int)Gtk.ResponseType.Yes)
				{
					stream.Close();
					client.Close();
					return;
				}
			}
			else
			{
				var dialog = new Gtk.MessageDialog(null, Gtk.DialogFlags.Modal, Gtk.MessageType.Question,
											   Gtk.ButtonsType.Ok, "fail");
				dialog.Show();
				int r = dialog.Run();
				dialog.Destroy();
				if (r != (int)Gtk.ResponseType.Yes)
				{
					stream.Close();
					client.Close();
					return;
				}
			}
		}
		catch (Exception error)
		{
			var dialog = new Gtk.MessageDialog(null, Gtk.DialogFlags.Modal, Gtk.MessageType.Question,
											   Gtk.ButtonsType.Ok, "error");
			dialog.Show();
			int r = dialog.Run();
			dialog.Destroy();
			if (r != (int)Gtk.ResponseType.Yes)
			{ return; }
		}
	}
}
