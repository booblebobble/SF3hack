using System;
using System.Collections.Generic;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;


public partial class Form1 : Form
{
	List<Image> Arrows;

	List<PictureBox> PictureBoxes;
	Queue<Image> QueuedPics;
	bool DoTick = true;

	public Form1()
	{
		InitializeComponent();

		QueuedPics = new Queue<Image>(10);
		PictureBoxes = new List<PictureBox>(20) {pb1, pb2, pb3, pb4, pb5, pb6, pb7, pb8, pb9, pb10,
											pb11, pb12, pb13, pb14, pb15, pb16, pb17, pb18, pb19, pb20};
		Arrows = new List<Image>{
					SF3hack.Properties.Resources.u,
					SF3hack.Properties.Resources.ur,
					SF3hack.Properties.Resources.r,
					SF3hack.Properties.Resources.dr,
					SF3hack.Properties.Resources.d,
					SF3hack.Properties.Resources.dl,
					SF3hack.Properties.Resources.l,
					SF3hack.Properties.Resources.ul,};

		Game.Init();
		Game.Tick += Tick;
		Game.P2HealthChanged += delegate {LastHealthChanged = DateTime.Now; };
//		Game.RoundStarting +=
//				delegate
//				{
//				};
//		Game.KeysPressed += OnKeysPressed;
		Game.GameClosed += delegate {this.Invoke((MethodInvoker)delegate {Close(); }); };

		KeyboardManager.Init();
		KeyboardManager.RecordingStarted += delegate { lblStatus.Text = "Recording inputs..."; };
		KeyboardManager.RecordingEnded +=
				delegate(int e)
				{
					lblStatus.Text = e.ToString() + " events captured.  Press space to replay.";
				};

		if (!Game.Found)
		{
			DoTick = false;
			this.Visible = false;
			MessageBox.Show(this, "Couldn't find game window. Please open 3S with ggpofba.exe and try again.", "Error");
			Load += delegate(object s, EventArgs e) { Close(); };
		}
		else if (!Game.CalibrationSucceeded)
		{
			DoTick = false;
			this.Visible = false;
			MessageBox.Show(this, "Unable to calculate memory addresses for game variables.  " + 
									"This can happen if you open sf3hack.exe at the same moment as the game.  " + 
									"If so, letting the game initialize for a couple seconds may fix this.\r\n\r\n" + 
									"(Sorry about that...)", "Error");
			Load += delegate(object s, EventArgs e) { Close(); };
		}
	}

	private void Form1_FormClosing(object sender, FormClosingEventArgs e)
	{
		KeyboardManager.Close();
	}

	DateTime LastHealthChanged = DateTime.Now.AddDays(99);
	int LastDamage = 0;

	void Tick()
	{
		if (!DoTick) return;

		if (Game.IsRoundActive & cbTrainingMode.Checked)
		{
			Game.Timer = 99;
			Game.P1Health = 160;
			Game.P1Stun = 0;
            Game.P1Super = Game.P2Super = 0;
// 			Game.P1MetersA = Game.P1MetersB = Game.P1MaxMeters;
//    		Game.P2MetersA = Game.P2MetersB = Game.P2MaxMeters;
			Game.P1MetersA = Game.P1MetersB = 3;
			Game.P2MetersA = Game.P2MetersB = 3;

			if ((DateTime.Now - LastHealthChanged).TotalMilliseconds >= 2000)
			{
				LastHealthChanged = DateTime.Now.AddDays(99);
				LastDamage = 160 - Game.P2Health;
				Game.P2Health = 160;
				Game.P2Stun = 0;
			}
		}

		try
		{
			this.BeginInvoke((MethodInvoker)delegate
			{
				lblLastDamage.Text = String.Format("{0} ({1}%)", LastDamage, (100 * LastDamage) / 160);

//				lblDebug.Text = Game.Timer.ToString();
				SetWindowPos(this.Handle,
					Game.Window,
					Game.WindowCoords.Left,
					Game.WindowCoords.Bottom,
					Game.WindowCoords.Right - Game.WindowCoords.Left,
					90,
					SWP_SHOWWINDOW);
			});
		}
		catch (Exception) { }
	}

	//int LastDirection = -1;
	//void OnKeysPressed(int dir, List<Game.Keys> keys, DateTime time)
	//{
	//    int newpics = 0;

	//    bool l = Game.PollKey(Game.Keys.LEFT);
	//    bool r = Game.PollKey(Game.Keys.RIGHT);
	//    bool u = Game.PollKey(Game.Keys.UP);
	//    bool d = Game.PollKey(Game.Keys.DOWN);
	//    bool l,u,r,d;

	//    if (u && d)
	//    {
	//        d = false;
	//    }

	//    if (l && r)
	//    {
	//        l = Game.P1Pos > Game.P2Pos;
	//        r = !l;
	//    }

	//    int Direction = (!l && !r && !u && !d) ? -1 :
	//                    (u && !l && !r) ? 0 :
	//                    (u && r) ? 1 :
	//                    (r && !d && !u) ? 2 :
	//                    (r && d) ? 3 :
	//                    (d && !r && !l) ? 4 :
	//                    (d && l) ? 5 :
	//                    (l && !d && !u) ? 6 :
	//                    (l && u) ? 7 : -1;

	//    if (Direction != LastDirection && Direction != -1)
	//    {
	//        lock (QueuedPics) QueuedPics.Enqueue(Arrows[Direction]);
	//        newpics++;
	//    }

	//    LastDirection = Direction;

	//    while (keys.Count > 0)
	//    {
	//        Game.Keys key = keys[0];
	//        keys.RemoveAt(0);

	//        if (key == Game.Keys.UP || key == Game.Keys.DOWN || key == Game.Keys.RIGHT || key == Game.Keys.LEFT)
	//            continue;

	//        Image pic = null;

	//        switch (key)
	//        {
	//            case (Game.Keys.LP): pic = SF3hack.Properties.Resources.lp; break;
	//            case (Game.Keys.MP): pic = SF3hack.Properties.Resources.mp; break;
	//            case (Game.Keys.HP): pic = SF3hack.Properties.Resources.hp; break;
	//            case (Game.Keys.LK): pic = SF3hack.Properties.Resources.lk; break;
	//            case (Game.Keys.MK): pic = SF3hack.Properties.Resources.mk; break;
	//            case (Game.Keys.HK): pic = SF3hack.Properties.Resources.hk; break;
	//        }

	//        lock (QueuedPics) QueuedPics.Enqueue(pic);
	//        newpics++;
	//    }

	//    try
	//    {
	//        this.BeginInvoke(
	//                (MethodInvoker)delegate
	//                {
	//                    while(newpics-- > 0)
	//                    {
	//                        Image newpic;
	//                        lock (QueuedPics)
	//                            newpic = QueuedPics.Dequeue();

	//                        for (int i = 0; i < PictureBoxes.Count - 1; i++)
	//                            PictureBoxes[i].Image = PictureBoxes[i + 1].Image;

	//                        PictureBoxes[PictureBoxes.Count - 1].Image = newpic;
	//                    }
	//                });
	//    }
	//    catch (Exception) { }
	//}

	private void buttonClose_Click(object sender, EventArgs e)
	{
		Close();
	}

	private void cbTrainingMode_CheckedChanged(object sender, EventArgs e)
	{
	}

	#region pinvoke shit

	[DllImport("user32.dll")]
	static extern int SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int x, int Y, int cx, int cy, int wFlags);

	const int SWP_SHOWWINDOW = 0x0040;
	const int SWP_NOSIZE = 0x0001;
	const int SWP_NOMOVE = 0x0002;

	#endregion


}