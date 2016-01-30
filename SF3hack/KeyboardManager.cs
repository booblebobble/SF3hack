using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;

static class KeyboardManager
{
	public delegate void RecordingEndedDelegate(int EventsRecorded);
	public static event MethodInvoker RecordingStarted, PlaybackStarted, PlaybackEnded;
	public static event RecordingEndedDelegate RecordingEnded;

	static Dictionary<Game.Keys, int> KeyDictionary = new Dictionary<Game.Keys, int>();
	static IntPtr kHook;
	static HookProcDelegate _LLKP = LowLevelKeyboardProc;

	static INPUT[] F9Down = new INPUT[1], F9Up = new INPUT[1];
	static INPUT[] F10Down = new INPUT[1], F10Up = new INPUT[1];
//	static EventWaitHandle PlaybackActive = new EventWaitHandle(false, EventResetMode.ManualReset);
	static EventWaitHandle PlaybackActive = new EventWaitHandle(false, EventResetMode.AutoReset);

	public static void Init()
	{
		Game.KeyChanged += OnKeyChanged;
		kHook = SetWindowsHookEx(WH_KEYBOARD_LL, _LLKP, LoadLibrary("User32"), 0);

		F9Down[0].Type = F9Up[0].Type = 1;
		F9Down[0].Data.Keyboard.Vk = F9Up[0].Data.Keyboard.Vk = VK_F9;
		F9Down[0].Data.Keyboard.Flags = 0;
		F9Up[0].Data.Keyboard.Flags = KEYEVENTF_KEYUP;

		F10Down[0].Type = F10Up[0].Type = 1;
		F10Down[0].Data.Keyboard.Vk = F10Up[0].Data.Keyboard.Vk = VK_F10;
		F10Down[0].Data.Keyboard.Flags = 0;
		F10Up[0].Data.Keyboard.Flags = KEYEVENTF_KEYUP;

		Thread PlaybackThread = new Thread(DoPlayback);
		PlaybackThread.IsBackground = true;
		PlaybackThread.Start();
	}

	public static void Close()
	{
		UnhookWindowsHookEx(kHook);
	}

	public static void UpdateInputs()
	{
		Dictionary<string, ushort> dict = new Dictionary<string, ushort>();

		foreach (string s in File.ReadAllLines(Game.ConfigFile))
		{
			Match m = Regex.Match(s, "input\\s*\"(?<name>[^\"]*)\"\\s*switch\\s*0x(?<sc>.*)");
			if (m.Success)
				dict.Add(m.Groups["name"].Value, (ushort)Int32.Parse(m.Groups["sc"].Value, System.Globalization.NumberStyles.HexNumber));
		}

		KeyDictionary.Clear();
		KeyDictionary.Add(Game.Keys.LP, dict["P2 Weak punch"]);
		KeyDictionary.Add(Game.Keys.MP, dict["P2 Medium punch"]);
		KeyDictionary.Add(Game.Keys.HP, dict["P2 Strong punch"]);
		KeyDictionary.Add(Game.Keys.LK, dict["P2 Weak kick"]);
		KeyDictionary.Add(Game.Keys.MK, dict["P2 Medium kick"]);
		KeyDictionary.Add(Game.Keys.HK, dict["P2 Strong kick"]);
		KeyDictionary.Add(Game.Keys.UP, dict["P2 Up"]);
		KeyDictionary.Add(Game.Keys.DOWN, dict["P2 Down"]);
		KeyDictionary.Add(Game.Keys.LEFT, dict["P2 Left"]);
		KeyDictionary.Add(Game.Keys.RIGHT, dict["P2 Right"]);
//		KeyDictionary.Add(Game.Keys.RIGHT, dict["P2 Left"]);
//		KeyDictionary.Add(Game.Keys.LEFT, dict["P2 Right"]);
	}

	static IntPtr LowLevelKeyboardProc(int nCode, IntPtr wParam, IntPtr lParam)
	{

		if (nCode == HC_ACTION && Game.IsFocused)
		{
			switch ((Keys)Marshal.ReadInt32(lParam))
			{
				case Keys.R:
					if ((int)wParam == WM_KEYDOWN)
						Record();
					break;

				case Keys.Space:
					if ((int)wParam == WM_KEYDOWN && Recording == false)
						TogglePlayback();
					break;

				case Keys.Home:
					SendInput(1, F10Down, Marshal.SizeOf(typeof(INPUT)));
					Thread.Sleep(30);
					SendInput(1, F10Up, Marshal.SizeOf(typeof(INPUT)));
					break;

				case Keys.Enter:
					SendInput(1, F9Down, Marshal.SizeOf(typeof(INPUT)));
					Thread.Sleep(30);
					SendInput(1, F9Up, Marshal.SizeOf(typeof(INPUT)));
					break;
			}
		}

		return CallNextHookEx(IntPtr.Zero, nCode, wParam, lParam);
	}

	static void OnKeyChanged(Game.Keys key, bool pressed, DateTime time)
	{
		if (Recording)
		{
			SendP2Input(key, pressed);

			if (!Orientation)
			{
				if (key == Game.Keys.RIGHT)
					key = Game.Keys.LEFT;
				else if (key == Game.Keys.LEFT)
					key = Game.Keys.RIGHT;
			}

			RecordedEvents.Add(new KbdEvent(key, pressed, time));
		}
	}

	static bool Recording = false;

	static public bool RecordingOrPlayback
	{
//		get {return Recording || PlaybackActive.WaitOne(0);}
		get {return Recording;}
	}

	class KbdEvent
	{
		public DateTime tm;
		public TimeSpan offset;
		System.Threading.Timer timer;

		public INPUT inp = new INPUT();
		INPUT[] inputarray;
		ushort regScan = 0, altScan = 0;
		public bool orientation;

		public KbdEvent(Game.Keys key, bool down, DateTime? dt)
		{
			if (dt.HasValue) tm = (DateTime)dt;

			regScan = (ushort)KeyDictionary[key];
			
			if (key == Game.Keys.LEFT)
				altScan = (ushort)KeyDictionary[Game.Keys.RIGHT];
			else if (key == Game.Keys.RIGHT)
				altScan = (ushort)KeyDictionary[Game.Keys.LEFT];
			else
				altScan = regScan;

			inp.Type = 1;
//			inp.Data.Keyboard.Scan = (ushort)KeyDictionary[key];
			inp.Data.Keyboard.Flags = (uint)(down ? 0 : KEYEVENTF_KEYUP) | KEYEVENTF_SCANCODE;
			timer = new System.Threading.Timer(Play);
			inputarray = new INPUT[] {inp};
		}

		public KbdEvent(Game.Keys key, bool down, TimeSpan off) : this(key, down, null)
		{
			offset = off;
		}

		public void Execute() { timer.Change((int)offset.TotalMilliseconds, Timeout.Infinite); }
		void Play(Object state) 
		{
			inputarray[0].Data.Keyboard.Scan = orientation ? regScan : altScan;
			SendInput(1, inputarray, Marshal.SizeOf(typeof(INPUT)));
		}
	}

	static List<KbdEvent> RecordedEvents = new List<KbdEvent>(100);
	static bool Orientation;

	public static void Record()
	{
		Recording = !Recording;

		if (Recording)
		{
			Orientation = Game.P2Pos > Game.P1Pos;
			RecordedEvents.Clear();
			if (RecordingStarted != null) RecordingStarted();
		}
		else
		{
			DateTime tmp = DateTime.Now;
			foreach (KbdEvent ke in RecordedEvents)
				if (ke.tm < tmp) tmp = ke.tm;

			tmp = tmp.AddMilliseconds(-100);
			foreach (KbdEvent ke in RecordedEvents)
				ke.offset = ke.tm - tmp;

			if (RecordingEnded != null) RecordingEnded(RecordedEvents.Count);
		}
	}

	static public void TogglePlayback()
	{
		if (RecordedEvents.Count > 0)
			PlayRecording(!PlaybackActive.WaitOne(0));
	}

	static public void PlayRecording(bool state)
	{
		if (state)
			PlaybackActive.Set();
		else
			PlaybackActive.Reset();
	}

	public static bool LoadStateOnPlayback;
	static void DoPlayback()
	{
		Random rnd = new Random();

		while(true)
		{
			PlaybackActive.WaitOne();

			if (!Game.IsRoundActive) continue;

			if (PlaybackStarted != null) PlaybackStarted();

			if (LoadStateOnPlayback)
			{
				SendInput(1, F9Down, Marshal.SizeOf(typeof(INPUT)));
				Thread.Sleep(30);
				SendInput(1, F9Up, Marshal.SizeOf(typeof(INPUT)));
			}

			Thread.Sleep(rnd.Next(500, 1000));

			TimeSpan longestOffset = new TimeSpan(0);
			foreach (KbdEvent ke in RecordedEvents)
			{
				ke.orientation = Game.Orientation;
				ke.Execute();
				if (ke.offset > longestOffset)
					longestOffset = ke.offset;
			}

			Thread.Sleep(longestOffset);

			if (PlaybackEnded != null) PlaybackEnded();
		}
	}

	public static void SendP2Input(Game.Keys key, bool pressed)
	{
//		Console.WriteLine(((int)key).ToString() + " " + pressed.ToString());
		INPUT[] inp = new INPUT[1];
		inp[0].Type = 1;
		inp[0].Data.Keyboard.Scan = (ushort)KeyDictionary[key];
		inp[0].Data.Keyboard.Flags = (uint)(pressed ? 0 : KEYEVENTF_KEYUP) | KEYEVENTF_SCANCODE;
		SendInput(1, inp, Marshal.SizeOf(typeof(INPUT)));
	}

	public static void TapP2Down()
	{
		(new KbdEvent(Game.Keys.DOWN, true, new TimeSpan(0,0,0,0,0))).Execute();
		(new KbdEvent(Game.Keys.DOWN, false, new TimeSpan(0,0,0,0,30))).Execute();
	}

	public static void TapP2Toward()
	{
		(new KbdEvent(Game.Keys.LEFT, true, new TimeSpan(0,0,0,0,0))).Execute();
		(new KbdEvent(Game.Keys.LEFT, false, new TimeSpan(0,0,0,0,30))).Execute();
	}

	#region pinvoke shit

	const int HC_ACTION = 0;
	const int WM_KEYUP = 0x0101;
	const int WM_KEYDOWN = 0x0100;
	const int INPUT_KEYBOARD = 1;
	const int KEYEVENTF_KEYUP = 2;
	const int KEYEVENTF_SCANCODE = 8;
	const int WH_KEYBOARD_LL = 13;

	delegate IntPtr HookProcDelegate(int nCode, IntPtr wParam, IntPtr lParam);
	[DllImport("user32.dll")]
	static extern IntPtr SetWindowsHookEx(int idHook, HookProcDelegate lpfn, IntPtr hMod, uint dwThreadId);
	[DllImport("user32.dll")]
	static extern IntPtr CallNextHookEx(IntPtr hhk, int nCode, IntPtr wParam, IntPtr lParam);
	[DllImport("user32.dll")]
	static extern bool UnhookWindowsHookEx(IntPtr hhk);
	[DllImport("kernel32.dll")]
	static extern IntPtr LoadLibrary(string lpFileName);
	[DllImport("user32.dll")]
	private static extern uint SendInput(uint numberOfInputs, INPUT[] inputs, int sizeOfInputStructure);

	[StructLayout(LayoutKind.Sequential)]
	internal struct INPUT
	{
		public uint Type;
		public MOUSEKEYBDHARDWAREINPUT Data;
	}
	[StructLayout(LayoutKind.Explicit)]
	internal struct MOUSEKEYBDHARDWAREINPUT
	{
		[FieldOffset(0)]
		public HARDWAREINPUT Hardware;
		[FieldOffset(0)]
		public KEYBDINPUT Keyboard;
		[FieldOffset(0)]
		public MOUSEINPUT Mouse;
	}
	[StructLayout(LayoutKind.Sequential)]
	internal struct HARDWAREINPUT
	{
		public uint Msg;
		public ushort ParamL;
		public ushort ParamH;
	}
	[StructLayout(LayoutKind.Sequential)]
	internal struct KEYBDINPUT
	{
		public ushort Vk;
		public ushort Scan;
		public uint Flags;
		public uint Time;
		public IntPtr ExtraInfo;
	}
	[StructLayout(LayoutKind.Sequential)]
	internal struct MOUSEINPUT
	{
		public int X;
		public int Y;
		public uint MouseData;
		public uint Flags;
		public uint Time;
		public IntPtr ExtraInfo;
	}

	const int DIK_UP = 0xC8;
	const int DIK_LEFT = 0xCB;
	const int DIK_RIGHT = 0xCD;
	const int DIK_DOWN = 0xD0;
	const int DIK_A = 0x1E;
	const int DIK_S = 0x1F;
	const int DIK_D = 0x20;
	const int DIK_Z = 0x2C;
	const int DIK_X = 0x2D;
	const int DIK_C = 0x2E;
	const int DIK_I = 0x17;
	const int DIK_O = 0x18;
	const int DIK_P = 0x19;
	const int DIK_LBRACKET = 0x1A;
	const int DIK_RBRACKET = 0x1B;
	const int DIK_J = 0x24;
	const int DIK_K = 0x25;
	const int DIK_L = 0x26;
	const int DIK_SEMICOLON = 0x27;
	const int DIK_APOSTROPHE = 0x28;
	const int VK_CONTROL = 0x11;
	const int VK_MENU = 0x12;
	const int VK_OEM_PLUS = 0xBB;
	const int VK_OEM_MINUS = 0xBD;
	const int VK_F9 = 0x78;
	const int VK_F10 = 0x79;

	#endregion
}

