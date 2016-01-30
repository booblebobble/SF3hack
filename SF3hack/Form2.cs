using System;
using System.Collections.Generic;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;
using SF3hack.Properties;

public partial class Form2 : Form
{
	int formWidth;

	public Form2()
	{
//		Settings.Default.InputDisplay = 0;
//		Settings.Default.Save();
		InitializeComponent();

		lblStatus.Text = "Press R to record";
		lblComboDamage.Text = "";

		Game.Init();

		if (!Game.Found)
		{
			this.Visible = false;
			MessageBox.Show(this, "Run this after starting the emulator.", "Game not found");
			Load += (s,e) => Close();
			return;
		}
		else if (!Game.CalibrationSucceeded)
		{
			this.Visible = false;
			MessageBox.Show(this, "Error, please restart this program. " + 
				"If that doesn't work, restart the emulator too.", "Initialization failure");
			Load += (s,e) => Close();
			return;
		}

		Settings.Default.LoadStateOnPlayback = false;
		Settings.Default.Save();
		ReadSettings();

		new Thread(PrepareImages).Start();
		Game.Tick += Tick;
		Game.P2HealthChanged += () => {LastHealthChanged = DateTime.Now; autoblockFlag = true;};
		Game.GameClosed += () => this.Invoke((MethodInvoker)delegate {Close();});
		Game.ComboEnded += () => LastComboEndedTime = DateTime.Now;

		SetWindowPos(this.Handle, Game.Window, Game.WindowCoords.Left - 150, Game.WindowCoords.Top,
			150, Game.WindowCoords.Height, SWP_SHOWWINDOW);

		KeyboardManager.Init();
		KeyboardManager.UpdateInputs();
		KeyboardManager.RecordingStarted += () => lblStatus.Text = "Recording...";

		KeyboardManager.RecordingEnded += (e) => lblStatus.Text = e.ToString() + " events recorded" + 
																	Environment.NewLine  + 
																	"Press P2 Start to replay";

		KeyboardManager.PlaybackStarted += () => this.BeginInvoke((MethodInvoker) delegate {lblStatus.Text = "Playing.";});
		KeyboardManager.PlaybackEnded += () => this.BeginInvoke((MethodInvoker) delegate {lblStatus.Text = "Press P2 Start to replay.";});
	}

	List<Image> ButtonCombinations;

	void PrepareImages()
	{
		List<Image> Arrows = new List<Image>{Resources.u, Resources.ur, Resources.r, Resources.dr, 
								Resources.d, Resources.dl, Resources.l, Resources.ul,};

		List<Image> Buttons = new List<Image>{Resources.lp, Resources.mp, Resources.hp,
									Resources.lk, Resources.mk, Resources.hk,};

		ButtonCombinations = new List<Image>(64*9);

//		int imgSize = Buttons[0].Width < Arrows[0].Width ? Buttons[0].Width : Arrows[0].Width;
		int imgSize = 80;

		ButtonCombinations.Add(new Bitmap(1, 1));
		for (int i = 1; i < 64; i++)
		{
			List<int> pos = new List<int>();
			for (int j = 5; j >= 0; j--) 
				if ((i & (1 << j)) != 0) pos.Add(5-j);

			Bitmap tmp = new Bitmap(pos.Count * imgSize, imgSize);
			using (Graphics g = Graphics.FromImage(tmp))
				for (int j = 0; j < pos.Count; j++)
					g.DrawImage(Buttons[pos[j]], j*imgSize, 0, imgSize, imgSize); 

			ButtonCombinations.Add(tmp);
		}

		for (int d = 0; d < 8; d++)
		{
			for (int j = 0; j < 64; j++)
			{
				Bitmap tmp = new Bitmap(ButtonCombinations[j].Width + imgSize, imgSize);
				using (Graphics g = Graphics.FromImage(tmp))
				{
					g.DrawImage(Arrows[d], 0, 0, imgSize, imgSize);
					g.DrawImage(ButtonCombinations[j], imgSize, 0, ButtonCombinations[j].Width, imgSize); 
				}

				ButtonCombinations.Add(tmp);
			}
		}

		Game.StartInputMonitor();
	}

	private void Form1_FormClosing(object sender, FormClosingEventArgs e)
	{
		Game.Tick -= Tick;
		KeyboardManager.Close();
	}

	DataGridViewCellStyle dgvStyleRed = new DataGridViewCellStyle{ForeColor = Color.Red};
	DataGridViewCellStyle dgvStyleBlue = new DataGridViewCellStyle{ForeColor = Color.Blue};

	int LastButtonPressFrame = 0, LastInputFrame = 0;

	DateTime LastHealthChanged = DateTime.Now.AddDays(99);
	int LastDamage = 0;

	DateTime LastAttackTime = DateTime.Now.AddDays(99);

	DateTime LastSAChanged = DateTime.Now.AddDays(99);
	int LastSuper = 0;
	int LastMeters = 0;

	DateTime LastComboEndedTime = DateTime.Now.AddDays(99);
	int MaxComboLength = 0;
	int P2HealthBeforeCurrentCombo = 160;
	int MaxDamage = 0;

	bool oldP1Attacking = false, blocking = false, autoblockFlag = false;

	Random rnd = new Random();
	int test = 0;
//	bool newtest = false;

	void Tick()
	{
//		this.BeginInvoke((MethodInvoker) delegate {lblStatus.Text = Game.test.ToString();});
//		this.BeginInvoke((MethodInvoker) delegate {lblStatus.Text = Game.P1Attacking.ToString();});
//		this.BeginInvoke((MethodInvoker) delegate {lblStatus.Text = autoblockFlag.ToString();});
		int x = Game.P2Height;
		if (x > test) test = x;
		if (Game.P2Height == 0)
		{
		    test = 0;
		}
//		if (test > 0) this.BeginInvoke((MethodInvoker) delegate {lblStatus.Text = "P2 Height = " + test.ToString();});

		if (!this.IsDisposed && !this.optionsFormOpen)
		{
			this.BeginInvoke((MethodInvoker) delegate {
				SetWindowPos(this.Handle, Game.Window, Game.WindowCoords.Left - formWidth, Game.WindowCoords.Top,
					formWidth, Game.WindowCoords.Height, SWP_SHOWWINDOW);
			});
		}

//		if (!Game.IsRoundActive) return;

		if (InputDisplay == InputDisplayOptions.Dual)
			goto InputDisplayOnly;

		Game.Timer = 99;
		Game.P1Health = 160;
		Game.P1Stun = 0;
		Game.P2MetersA = Game.P2MetersB = 3;
		if (Stun == StunOptions.None) Game.P2Stun = 0;

		if (Game.P2StartKeyDown && Game.IsRoundActive) KeyboardManager.TogglePlayback();

		if (Game.P1Combo > MaxComboLength) MaxComboLength = Game.P1Combo;
		int ComboDamage = Game.P2Health == 0 ? 0 : P2HealthBeforeCurrentCombo - Game.P2Health;
//		int ComboDamage = P2HealthBeforeCurrentCombo - Game.P2Health;

		if ((DateTime.Now - LastHealthChanged).TotalMilliseconds >= 1500)
		{
			LastHealthChanged = DateTime.Now.AddDays(99);
			LastDamage = 160 - Game.P2Health;
			Game.P2Health = 160;
			Game.P2Stun = 0;
		}

		this.BeginInvoke((MethodInvoker)delegate
		{
			if (ComboDamage > 0)
			{
				lblComboDamage.Text = String.Format("{0} ({1}%)", ComboDamage, (100 * ComboDamage) / 160);
				if (ComboDamage > MaxDamage)
				{
					MaxDamage = ComboDamage;
					lblMaxDamage.Text = String.Format("{0} ({1}%)", MaxDamage, (100 * MaxDamage) / 160);
				}
			}
			lblMaxCombo.Text = MaxComboLength.ToString();
			lblLastDamage.Text = String.Format("{0} ({1}%)", LastDamage, (100 * LastDamage) / 160);
		});

		if (Game.P1Combo == 0)
			P2HealthBeforeCurrentCombo = Game.P2Health;




		if (!KeyboardManager.RecordingOrPlayback && Game.IsRoundActive)
		{
			switch (Action)
			{
				case ActionOptions.Stand:
					break;

					for (int i = 0; i < 10; i++)
						if (Game.PollKey(2, (Game.Keys)i))
							KeyboardManager.SendP2Input((Game.Keys)i, false);
					break;

				case ActionOptions.Crouch:
					for (int i = 0; i < 10; i++)
						if (i == (int)Game.Keys.DOWN)
						{
							if (!Game.PollKey(2, Game.Keys.DOWN))
								KeyboardManager.SendP2Input(Game.Keys.DOWN, true);
						}
						else
						{
							if (Game.PollKey(2, (Game.Keys)i))
								KeyboardManager.SendP2Input((Game.Keys)i, false);
						}
					break;

				case ActionOptions.Jump:
					for (int i = 0; i < 10; i++)
						if (i == (int)Game.Keys.UP)
						{
							if (!Game.PollKey(2, Game.Keys.UP))
								KeyboardManager.SendP2Input(Game.Keys.UP, true);
						}
						else
						{
							if (Game.PollKey(2, (Game.Keys)i))
								KeyboardManager.SendP2Input((Game.Keys)i, false);
						}
					break;
			}
		}





		if (Game.P1Attacking && (
						 (Guard == GuardOptions.AllBlock) || 
						 (Guard == GuardOptions.AutoBlock && autoblockFlag) || 
						 (Guard == GuardOptions.Random && (blocking || (!oldP1Attacking && rnd.Next(0,2) == 1)))))
		{
			blocking = true;

			if (!Game.PollKey(2, Game.Keys.DOWN))
				KeyboardManager.SendP2Input(Game.Keys.DOWN, true);
			if (!Game.PollKey(2, Game.P2BackKey))
				KeyboardManager.SendP2Input(Game.P2BackKey, true);
			if (Game.PollKey(2, Game.P2TowardKey))
				KeyboardManager.SendP2Input(Game.P2TowardKey, false);
		}
		else if (blocking)
		{
			blocking = false;

			KeyboardManager.SendP2Input(Game.Keys.LEFT, false);
			KeyboardManager.SendP2Input(Game.Keys.RIGHT, false);
			KeyboardManager.SendP2Input(Game.Keys.DOWN, false);
		}

		oldP1Attacking = Game.P1Attacking;

		if (Game.P1Attacking) LastAttackTime = DateTime.Now;
		if ((DateTime.Now - LastAttackTime).TotalMilliseconds > 1000)
		{
			autoblockFlag = false;
			LastAttackTime = DateTime.Now.AddDays(99);
		}


		if (Game.P2Landing && (Stand == StandOptions.Quick ||
								(Stand == StandOptions.Random && rnd.Next(0, 2) == 1)))
			KeyboardManager.TapP2Down();


		switch (SAGauge)
		{
			case SAGaugeOptions.Infinite:
				Game.P1Super = Game.P2Super = 0;
				Game.P1MetersA = Game.P1MetersB = 3;
				break;

			case SAGaugeOptions.Refill:
				if (Game.P1Super != LastSuper || Game.P1MetersA != LastMeters)
				{
					LastSuper = Game.P1Super;
					LastMeters = Game.P1MetersA;
					LastSAChanged = DateTime.Now;
				}
				if ((DateTime.Now - LastSAChanged).TotalMilliseconds >= 1500)
				{
					Game.P1Super = Game.P2Super = 0;
					Game.P1MetersA = Game.P1MetersB = 3;
					LastSAChanged = DateTime.Now.AddDays(99);
				}
				break;
		}

InputDisplayOnly:

		switch (InputDisplay)
		{
			case InputDisplayOptions.On:
			case InputDisplayOptions.Dual:
				if (Game.P1NewImageFlag || Game.P2NewImageFlag)
				{
//					Debug.Assert(Game.P1ImageCode > 0 && Game.P1ImageCode < 9*64);

					//ui code might not run until Game values have been overwritten.  save local copies.
					int Frame = Game.Frame;
					int P1ImageCode = Game.P1ImageCode;
					int P2ImageCode = Game.P2ImageCode;
					bool P1NewImageFlag = Game.P1NewImageFlag;
					bool P2NewImageFlag = Game.P2NewImageFlag;
					int P1NewButtons = Game.P1NewButtons;
					bool FrameMissed = Game.FrameMissed;

					this.BeginInvoke((MethodInvoker) delegate {
						if (P1NewImageFlag)
//						if (Game.P1NewImageFlag)
						{
							if (P1NewButtons != 0)
							{
								dgvPlayer1.Rows.Insert(0, new Object[] {(Frame - LastButtonPressFrame).ToString(), ButtonCombinations[P1ImageCode]});
								LastButtonPressFrame = LastInputFrame = Frame;
								dgvPlayer1.Rows[0].Cells[0].Style = dgvStyleBlue;
							}
							else
							{
								dgvPlayer1.Rows.Insert(0, new Object[] {(Frame - LastInputFrame).ToString(), ButtonCombinations[P1ImageCode]});
								LastInputFrame = Frame;
							}

//							if (FrameMissed)
//								dgvPlayer1.Rows[0].Cells[0].Style = dgvStyleRed;
						}

//						if (Game.P2NewImageFlag && InputDisplay == InputDisplayOptions.Dual)
//							dgvPlayer2.Rows.Insert(0, new Image[] {ButtonCombinations[Game.P2ImageCode]});
						if (P2NewImageFlag && InputDisplay == InputDisplayOptions.Dual)
							dgvPlayer2.Rows.Insert(0, new Image[] {ButtonCombinations[P2ImageCode]});

						int displayedRows = dgvPlayer1.DisplayedRowCount(false);
						for (int i = displayedRows; i < dgvPlayer1.Rows.Count; i++)
							try {dgvPlayer1.Rows.RemoveAt(displayedRows);} catch (Exception) {}

						displayedRows = dgvPlayer2.DisplayedRowCount(false);
						for (int i = displayedRows; i < dgvPlayer2.Rows.Count; i++)
							try {dgvPlayer2.Rows.RemoveAt(displayedRows);} catch (Exception) {}

						dgvPlayer1.ClearSelection();
						dgvPlayer2.ClearSelection();
					});
				}
				break;
		}
	}

	enum ActionOptions {Stand=0, Crouch=1, Jump=2, Human=3, Record=4, Playback=5};
	ActionOptions Action;
	enum GuardOptions {NoBlock=0, AutoBlock=1, AllBlock=2, Random=3};
	GuardOptions Guard;
	enum StandOptions {Normal=0, Quick=1, Random=2};
	StandOptions Stand;
	enum CounterHitOptions {Off=0, On=1, Random=2};
	CounterHitOptions CounterHit;
	enum StunOptions {Normal=0, /*Constant=1,*/ None=1};
	StunOptions Stun;
	enum SAGaugeOptions {Normal=0, Infinite=1, Refill=2};
	SAGaugeOptions SAGauge;
	enum AttackDataOptions {Off=0, On=1};
	AttackDataOptions AttackData;
	enum InputDisplayOptions {Off=0, On=1, Dual=2};
	InputDisplayOptions InputDisplay;
	int LagSimulation;

	bool optionsFormOpen = false;
	private void btnConfig_Click(object sender, EventArgs e)
	{
		KeyboardManager.PlayRecording(false);
		optionsFormOpen = true;
		(new OptionsForm()).ShowDialog(this);
		optionsFormOpen = false;
		ReadSettings();
	}

	private void btnClose_Click(object sender, EventArgs e)
	{
		Close();
	}

	void ReadSettings()
	{
		Action = (ActionOptions)Settings.Default.Action;
		Guard = (GuardOptions)Settings.Default.Guard;
		Stand = (StandOptions)Settings.Default.Stand;
		CounterHit = (CounterHitOptions)Settings.Default.CounterHit;
		Stun = (StunOptions)Settings.Default.Stun;
		SAGauge = (SAGaugeOptions)Settings.Default.SAGauge;
		AttackData = (AttackDataOptions)Settings.Default.AttackData;
		InputDisplay = (InputDisplayOptions)Settings.Default.InputDisplay;
		LagSimulation = Settings.Default.LagSimulation;

		dgvPlayer1.Visible = (InputDisplay != InputDisplayOptions.Off);

		lblComboDamage.Visible = lblComboDamageT.Visible = lblLastDamage.Visible = lblLastDamageT.Visible =
			lblMaxCombo.Visible = lblMaxComboT.Visible /*= lblMaxDamage.Visible = lblMaxDamageT.Visible */
					= (AttackData == AttackDataOptions.On);

		lblComboDamage.Text = "0 (0%)";

		switch (InputDisplay)
		{
			case InputDisplayOptions.Off:
				dgvPlayer1.Rows.Clear();
				dgvPlayer2.Rows.Clear();
				formWidth = 180;
				break;
			case InputDisplayOptions.On:
				formWidth = 180;
				dgvPlayer2.Rows.Clear();
				break;
			case InputDisplayOptions.Dual:
				formWidth = 360;
				break;
		}

		SetWindowPos(this.Handle, Game.Window, Game.WindowCoords.Left - formWidth, Game.WindowCoords.Top,
			formWidth, Game.WindowCoords.Height, SWP_SHOWWINDOW);

		if (AttackData == AttackDataOptions.On)
		{
			MaxDamage = 0;
			MaxComboLength = 0;
			P2HealthBeforeCurrentCombo = 160;
			DateTime LastComboEndedTime = DateTime.Now.AddDays(99);
		}

		KeyboardManager.LoadStateOnPlayback = Settings.Default.LoadStateOnPlayback;
	}


	#region pinvoke shit

	[DllImport("user32.dll")]
	static extern int SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int x, int Y, int cx, int cy, int wFlags);
	[DllImport("user32.dll", SetLastError = true)]
	static extern IntPtr GetWindow(IntPtr hWnd, GetWindow_Cmd uCmd);

	enum GetWindow_Cmd : uint {
    GW_HWNDFIRST = 0,
    GW_HWNDLAST = 1,
    GW_HWNDNEXT = 2,
    GW_HWNDPREV = 3,
    GW_OWNER = 4,
    GW_CHILD = 5,
    GW_ENABLEDPOPUP = 6
}
	const int SWP_SHOWWINDOW = 0x0040;
	const int SWP_NOSIZE = 0x0001;
	const int SWP_NOMOVE = 0x0002;

	#endregion


}