using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;

public static class Game
{
	public enum Keys { UP=9, DOWN=8, LEFT=7, RIGHT=6, LP=5, MP=4, HP=3, LK=2, MK=1, HK=0, };

	static Process process;
	static IntPtr processHandle;
	public static IntPtr Window { get { return process.MainWindowHandle; } }
	public static bool Found { get { return process != null; } }
	public static RECT WindowCoords = new RECT(0,0,0,0);
	public static bool CalibrationSucceeded { get; set; }

	public static event MethodInvoker P2HealthChanged, RoundStarting, Tick, GameClosed, ComboEnded;

	public delegate void KeyChangedEventDelegate(Keys key, bool pressed, DateTime time);
	public static event KeyChangedEventDelegate KeyChanged;

	public static bool IsFocused { get { return GetForegroundWindow() == process.MainWindowHandle; } }
	public static bool IsRoundActive { get { return Timer > 0 && P1Health != 255 && P2Health != 255; } }

	public static string ConfigFile;
	public static void Init()
	{
		process = null;
		foreach (Process p in Process.GetProcessesByName("ggpofba-ng").Union<Process>(Process.GetProcessesByName("ggpofba")))
		{
			if (p.MainWindowTitle.IndexOf("Street Fighter III") != -1)
			{
				process = p;
				break;
			}
		}

		if (process == null) return;

		processHandle = OpenProcess(ProcessAccessFlags.QueryInformation | ProcessAccessFlags.VMRead | ProcessAccessFlags.VMWrite | ProcessAccessFlags.VMOperation, false, process.Id);

		GetWindowRect(process.MainWindowHandle, out WindowCoords);

		if (!(CalibrationSucceeded = Calibrate())) return;

		ConfigFile = Path.GetDirectoryName(process.MainModule.FileName) + "\\config\\games\\sfiii3n.ini";
	}


	public static void StartInputMonitor()
	{
		Thread thd = new Thread(Spin);
		thd.Priority = ThreadPriority.AboveNormal;
		thd.IsBackground = true;
		thd.Start();

	}

	static void Spin()
	{
		while(true)
		{
			if (process.HasExited)
			{
				if (GameClosed != null)	GameClosed();
				return;
			}
			else
			{
//				if (GetVariable(pFrame, 2) % 20 != 0) {
					int o_Frame = m_Frame;
					if (o_Frame != (m_Frame = GetVariable(pFrame, 2)))
					{
						if (o_Frame > m_Frame) 
						{
							FrameOffset += 65536;
							if (o_Frame != 65535 || m_Frame != 0)
							FrameMissed = true;
						}
						else
						{
							if (m_Frame - o_Frame > 1)
								FrameMissed = true;
						}

						OnTick();
					}
//				}
			}

			Thread.Sleep(5);
		}
	}

	static public bool FrameMissed = false;

	static IntPtr pTest = (IntPtr)0x160C8E84;
	static public int test;

	static public bool P1Landing = false, P2Landing = false;
	static public bool WindowMoved;
	static public bool P1Start, P2Start;
	static public bool P2StartKeyDown = false;

	static void OnTick()
	{
		test = GetVariable(pTest, 1);

		IntPtr bytesRead;

		RECT oldCoords = WindowCoords;
		GetWindowRect(process.MainWindowHandle, out WindowCoords);
		WindowMoved = (WindowCoords != oldCoords);

		int o_P1Height = m_P1Height, o_P2Height = m_P2Height;
		int o_P1Combo = P1Combo;

		m_P1Pos = GetVariable(pP1Pos, 2);
		m_P2Pos = GetVariable(pP2Pos, 2);
		m_P1Height = GetVariable(pP1Height, 2);
		m_P2Height = GetVariable(pP2Height, 2);
		m_P1Stun = GetVariable(pP1Stun, 1);
		m_P2Stun = GetVariable(pP2Stun, 1);
		m_P1Super = GetVariable(pP1Super, 1);
		m_P2Super = GetVariable(pP2Super, 1);
		m_P1MetersA = GetVariable(pP1MetersA, 1);
		m_P1MetersB = GetVariable(pP1MetersB, 1);
		m_P2MetersA = GetVariable(pP2MetersA, 1);
		m_P2MetersB = GetVariable(pP2MetersB, 1);
		P1Combo = GetVariable(pP1Combo, 1);
		P2Combo = GetVariable(pP2Combo, 1);
		P1Attacking = (bool)(GetVariable(pP1Attacking, 1) == 1);

		bool o_P1Start = P1Start, o_P2Start = P2Start;
		P1Start = (GetVariable(pP1Start, 1) == 1);
		P2Start = (GetVariable(pP2Start, 1) == 1);

		P2StartKeyDown = P2Start && !o_P2Start;

		if (o_P1Combo > 0 && P1Combo == 0)
			if (ComboEnded != null) ComboEnded();

		P1Landing = o_P1Height > 0 && m_P1Height == 0;
		P2Landing = o_P2Height > 0 && m_P2Height == 0;

		int t = GetVariable(pTimer, 1);
		int h1 = GetVariable(pP1Health, 1);
		int h2 = GetVariable(pP2Health, 1);

		if ((t == 99 && h1 == 160 && h2 == 160) && (m_Timer != 99 || m_P1Health != 160 || m_P2Health != 160))
			if (RoundStarting != null) RoundStarting();

		m_Timer = t;
		m_P1Health = h1;
		if (m_P2Health > (m_P2Health = h2))
			if (P2HealthChanged != null) P2HealthChanged();


		byte[] inp = new byte[(long)pP2HK - (long)pP1Up + 1];
		ReadProcessMemory(process.Handle, pP1Up, inp, inp.Length, out bytesRead);

		int o_P1Inputs = P1Inputs, o_P2Inputs = P2Inputs;

		P1Inputs = (inp[(long)pP1Up - (long)pP1Up] << 9) + 
					(inp[(long)pP1Down - (long)pP1Up] << 8) +
					(inp[(long)pP1Left - (long)pP1Up] << 7) +
					(inp[(long)pP1Right - (long)pP1Up] << 6) +
					(inp[(long)pP1LP - (long)pP1Up] << 5) +
					(inp[(long)pP1MP - (long)pP1Up] << 4) +
					(inp[(long)pP1HP - (long)pP1Up] << 3) +
					(inp[(long)pP1LK - (long)pP1Up] << 2) +
					(inp[(long)pP1MK - (long)pP1Up] << 1) +
					(inp[(long)pP1HK - (long)pP1Up]);

		P2Inputs = (inp[(long)pP2Up - (long)pP1Up] << 9) + 
					(inp[(long)pP2Down - (long)pP1Up] << 8) +
					(inp[(long)pP2Left - (long)pP1Up] << 7) +
					(inp[(long)pP2Right - (long)pP1Up] << 6) +
					(inp[(long)pP2LP - (long)pP1Up] << 5) +
					(inp[(long)pP2MP - (long)pP1Up] << 4) +
					(inp[(long)pP2HP - (long)pP1Up] << 3) +
					(inp[(long)pP2LK - (long)pP1Up] << 2) +
					(inp[(long)pP2MK - (long)pP1Up] << 1) +
					(inp[(long)pP2HK - (long)pP1Up]);


		int P1Changes = P1Inputs ^ o_P1Inputs;
		DateTime dt = DateTime.Now;

		for (int i = 0; i < 10; i++)
			if ((P1Changes >> i) % 2 != 0)
				if (KeyChanged != null) KeyChanged((Keys)i, PollKey(1, (Keys)i), dt);


		int o_P1NewButtons = P1NewButtons, o_P2NewButtons = P2NewButtons;
		P1NewButtons = (P1Inputs & ~o_P1Inputs) & 63;
		P2NewButtons = (P2Inputs & ~o_P2Inputs) & 63;


		int o_P1Direction = P1Direction, o_P2Direction = P2Direction;
		bool u,d,l,r;

		u = PollKey(1, Keys.UP);
		d = PollKey(1, Keys.DOWN);
		l = PollKey(1, Keys.LEFT);
		r = PollKey(1, Keys.RIGHT);
		d = d && !u;
		l = l && (!r || P1Pos > P2Pos);
		r = r && (!l || P1Pos <= P2Pos);
		P1Direction = (!l && !r && !u && !d) ? 0 : (u && !l && !r) ? 1 : (u && r) ? 2 : (r && !d && !u) ? 3 :
				(r && d) ? 4 : (d && !r && !l) ? 5 : (d && l) ? 6 : (l && !d && !u) ? 7 : 8;

		u = PollKey(2, Keys.UP);
		d = PollKey(2, Keys.DOWN);
		l = PollKey(2, Keys.LEFT);
		r = PollKey(2, Keys.RIGHT);
		d = d && !u;
		l = l && (!r || P2Pos > P1Pos);
		r = r && (!l || P2Pos <= P1Pos);
		P2Direction = (!l && !r && !u && !d) ? 0 : (u && !l && !r) ? 1 : (u && r) ? 2 : (r && !d && !u) ? 3 :
				(r && d) ? 4 : (d && !r && !l) ? 5 : (d && l) ? 6 : (l && !d && !u) ? 7 : 8;

		P1ImageCode = (P1Direction << 6) + (P1NewButtons != 0 ? (P1NewButtons | o_P1NewButtons) : 0);
		P2ImageCode = (P2Direction << 6) + (P2NewButtons != 0 ? (P2NewButtons | o_P2NewButtons) : 0);

		P1NewImageFlag = ((P1Direction != o_P1Direction) && (P1Direction != 0)) || (P1NewButtons != 0);
		P2NewImageFlag = ((P2Direction != o_P2Direction) && (P2Direction != 0)) || (P2NewButtons != 0);

		if (Tick != null) Tick();

		if (P1NewImageFlag) FrameMissed = false;
	}

	static void SetVariable(IntPtr addr, int val, int field_length)
	{
		IntPtr bytesWritten;
		byte [] data = new byte[2];

		data[0] = (byte)(val % 256);
		data[1] = (byte)(val / 256);

		WriteProcessMemory(process.Handle, addr, data, field_length, out bytesWritten);
	}

	static int GetVariable(IntPtr addr, int field_length)
	{
		IntPtr bytesRead;
		byte [] data = new byte[2];

		ReadProcessMemory(process.Handle, addr, data, field_length, out bytesRead);

		return data[0] + (field_length == 2 ? 256 * data[1] : 0);
	}

	public static bool PollKey(int player, Game.Keys key)
	{
		if (player == 1)
			return (P1Inputs >> (int)key) % 2 == 1;
		else
			return (P2Inputs >> (int)key) % 2 == 1;
	}

	static bool Calibrate()
	{
		int calibrationPointsNeeded = 1;
		long offset = 0;
//		long offset2 = 0;
//		int CalibrationLength = Math.Min(CalibrationArray.Length, CalibrationArray2.Length);
		int CalibrationLength = CalibrationArray.Length;

		SYSTEM_INFO si = new SYSTEM_INFO();
		GetSystemInfo(out si);

		long min_address = (long)si.minimumApplicationAddress;
		long max_address = (long)si.maximumApplicationAddress;

		MEMORY_BASIC_INFORMATION mbi = new MEMORY_BASIC_INFORMATION();

		IntPtr bytesRead;

		byte[] segment = new byte[CalibrationLength];

		while (min_address < max_address)
		{
			VirtualQueryEx(processHandle, (IntPtr)min_address, out mbi, (uint)Marshal.SizeOf(typeof(MEMORY_BASIC_INFORMATION)));
//			Console.WriteLine(min_address.ToString() + " " + (min_address+(int)mbi.RegionSize).ToString());

			if (mbi.Protect == PAGE_READWRITE && mbi.State == MEM_COMMIT)
			{
				byte[] buffer = new byte[(int)mbi.RegionSize];
				ReadProcessMemory(processHandle, (IntPtr)mbi.BaseAddress, buffer, (int)mbi.RegionSize, out bytesRead);

//				int i = Array.FindIndex<byte>(buffer, 0, (b) => (b == CalibrationArray[0] || b == CalibrationArray2[0]));
				int i = Array.FindIndex<byte>(buffer, 0, (b) => (b == CalibrationArray[0]));

				while (i >= 0 && i <= buffer.Length - CalibrationLength)
				{
					Buffer.BlockCopy(buffer, i, segment, 0, CalibrationLength);

					if (segment.SequenceEqual<byte>(CalibrationArray))
					{
//						Console.WriteLine(i);
						offset = (long)mbi.BaseAddress + i;
						if (--calibrationPointsNeeded == 0) goto ExitLoop;
					}

					//if (segment.SequenceEqual<byte>(CalibrationArray2))
					//{
					//    offset2 = (long)mbi.BaseAddress + i;
					//    if (--calibrationPointsNeeded == 0) goto ExitLoop;
					//}

//					i = Array.FindIndex<byte>(buffer, i+1, (b) => (b == CalibrationArray[0] || b == CalibrationArray2[0]));
					i = Array.FindIndex<byte>(buffer, i+1, (b) => (b == CalibrationArray[0]));
				}
			}

			if ((long)mbi.RegionSize == 0) break;

			min_address = ((long)mbi.RegionSize + min_address);
		}

		return false;

	ExitLoop:

		pFrame = (IntPtr)((long)pFrame + offset);
		pTimer = (IntPtr)((long)pTimer + offset);
		pP1Health = (IntPtr)((long)pP1Health + offset);
		pP2Health = (IntPtr)((long)pP2Health + offset);
		pP1Stun = (IntPtr)((long)pP1Stun + offset);
		pP2Stun = (IntPtr)((long)pP2Stun + offset);
		pP1Pos = (IntPtr)((long)pP1Pos + offset);
		pP2Pos = (IntPtr)((long)pP2Pos + offset);
		pP1Super = (IntPtr)((long)pP1Super + offset);
		pP2Super = (IntPtr)((long)pP2Super + offset);
		pP1MetersA = (IntPtr)((long)pP1MetersA + offset);
		pP1MetersB = (IntPtr)((long)pP1MetersB + offset);
		pP2MetersA = (IntPtr)((long)pP2MetersA + offset);
		pP2MetersB = (IntPtr)((long)pP2MetersB + offset);
		pP1MaxMeters = (IntPtr)((long)pP1MaxMeters + offset);
		pP2MaxMeters = (IntPtr)((long)pP2MaxMeters + offset);
		pP1Height = (IntPtr)((long)pP1Height + offset);
		pP2Height = (IntPtr)((long)pP2Height + offset);
		pP1Combo = (IntPtr)((long)pP1Combo + offset);
		pP2Combo = (IntPtr)((long)pP2Combo + offset);
		pP1Attacking = (IntPtr)((long)pP1Attacking + offset);

//		pP1WinStreak = (IntPtr)((long)pP1WinStreak + offset2);
//		pP2WinStreak = (IntPtr)((long)pP2WinStreak + offset2);

		return true;
	}


	//78 00 79 00 76 00 77 00 74 00 75 00 72 00 73 00 70 00
	static byte[] CalibrationArray = new byte[] {0x78, 0x00, 0x79, 0x00, 0x76, 0x00};
	static IntPtr pFrame = (IntPtr)(0x160A8720 - 0x161091C0);
	static IntPtr pTimer = (IntPtr)(0x160B1B94 - 0x161091C0);
	static IntPtr pP1Health = (IntPtr)(0x16109528 - 0x161091C0);
	static IntPtr pP2Health = (IntPtr)(0x161099C0 - 0x161091C0);
	static IntPtr pP1Stun = (IntPtr)(0x16109E1E - 0x161091C0);
	static IntPtr pP2Stun = (IntPtr)(0x16109E32 - 0x161091C0);
	static IntPtr pP1Pos = (IntPtr)(0x161094F2 - 0x161091C0);
	static IntPtr pP2Pos = (IntPtr)(0x1610998A - 0x161091C0);
	static IntPtr pP1Super = (IntPtr)(0x16109DD6 - 0x161091C0);
	static IntPtr pP2Super = (IntPtr)(0x16109E02 - 0x161091C0);
	static IntPtr pP1MetersA = (IntPtr)(0x160C8EC8 - 0x161091C0);
	static IntPtr pP1MetersB = (IntPtr)(0x16109DDC - 0x161091C0);
	static IntPtr pP2MetersA = (IntPtr)(0x160C8EFC - 0x161091C0);
	static IntPtr pP2MetersB = (IntPtr)(0x16109E08 - 0x161091C0);
	static IntPtr pP1MaxMeters = (IntPtr)(0x16109DDE - 0x161091C0);
	static IntPtr pP2MaxMeters = (IntPtr)(0x16109E0A - 0x161091C0);
	static IntPtr pP1Height = (IntPtr)(0x161160DC - 0x161151C0);
	static IntPtr pP2Height = (IntPtr)(0x161163DC - 0x161151C0);
	static IntPtr pP1Combo = (IntPtr)(0x16115EE6 - 0x161151C0);
	static IntPtr pP2Combo = (IntPtr)(0x16115E3E - 0x161151C0);
//	static IntPtr pP1Attacking = (IntPtr)(0x1611569D - 0x161151C0);
	static IntPtr pP1Attacking = (IntPtr)(0x161188B7 - 0x161181C0);
	static IntPtr pP1Rounds = (IntPtr)(0x160B9BA0 - 0x161111C0);
	static IntPtr pP2Rounds = (IntPtr)(0x160B9BA6 - 0x161111C0);


	//A0 2A 90 2A C0 2A B0 2A E0 2A
	static byte[] CalibrationArray2 = new byte[] {0xA0, 0x2A, 0x90, 0x2A, 0xC0, 0x2A};
	static IntPtr pP1WinStreak = (IntPtr)(0x0ECED4F6 - 0x0ECECC14);
	static IntPtr pP2WinStreak = (IntPtr)(0x0ECED4F4 - 0x0ECECC14);


	//these don't seem to need calibration
	static IntPtr pP1Up = (IntPtr)0x00A871AC;
	static IntPtr pP1Down = (IntPtr)0x00A871AD;
	static IntPtr pP1Left = (IntPtr)0x00A871AE;
	static IntPtr pP1Right = (IntPtr)0x00A871AF;
	static IntPtr pP1LP = (IntPtr)0x00A871B0;
	static IntPtr pP1MP = (IntPtr)0x00A871B1;
	static IntPtr pP1HP = (IntPtr)0x00A871B2;
	static IntPtr pP1HK = (IntPtr)0x00A87219;
	static IntPtr pP1MK = (IntPtr)0x00A8721A;
	static IntPtr pP1LK = (IntPtr)0x00A8721B;
	static IntPtr pP1Start = (IntPtr)0x00A87234;

	static IntPtr pP2Up = (IntPtr)0x00A871B4;
	static IntPtr pP2Down = (IntPtr)0x00A871B5;
	static IntPtr pP2Left = (IntPtr)0x00A871B6;
	static IntPtr pP2Right = (IntPtr)0x00A871B7;
	static IntPtr pP2LP = (IntPtr)0x00A871B8;
	static IntPtr pP2MP = (IntPtr)0x00A871B9;
	static IntPtr pP2HP = (IntPtr)0x00A871BA;
	static IntPtr pP2LK = (IntPtr)0x00A8721C;
	static IntPtr pP2MK = (IntPtr)0x00A8721D;
	static IntPtr pP2HK = (IntPtr)0x00A87232;
	static IntPtr pP2Start = (IntPtr)0x00A87235;


	public static int P1Direction, P2Direction;
	public static bool P1DirectionChanged, P2DirectionChanged;

	public static int P1Inputs = 0, P2Inputs = 0;
	public static int P1NewButtons = 0, P2NewButtons = 0;

	public static int P1ImageCode, P2ImageCode;
	public static bool P1NewImageFlag, P2NewImageFlag;

	public static bool P1Attacking;
	public static int P1Combo, P2Combo;

	static int FrameOffset = 0;
	static int m_Frame = 0, m_Timer, m_P1Health, m_P2Health, m_P1Stun, m_P2Stun, m_P1Pos, m_P2Pos, m_P1Super, m_P2Super,
				m_P1MetersA, m_P1MetersB, m_P2MetersA, m_P2MetersB, m_P1MaxMeters, m_P2MaxMeters, m_P1Height, m_P2Height;
	public static int Frame { get { return m_Frame + FrameOffset; } }
	public static int Timer { set { SetVariable(pTimer, value, 1); m_Timer = value; } get { return m_Timer; } }
	public static int P1Health { set { SetVariable(pP1Health, value, 1); m_P1Health = value; } get { return m_P1Health; } }
	public static int P2Health { set { SetVariable(pP2Health, value, 1); m_P2Health = value; } get { return m_P2Health; } }
	public static int P1Stun { set { SetVariable(pP1Stun, value, 1); m_P1Stun = value; } get { return m_P1Stun; } }
	public static int P2Stun { set { SetVariable(pP2Stun, value, 1); m_P2Stun = value; } get { return m_P2Stun; } }
	public static int P1Pos { set { SetVariable(pP1Pos, value, 2); m_P1Pos = value; } get { return m_P1Pos; } }
	public static int P2Pos { set { SetVariable(pP2Pos, value, 2); m_P2Pos = value; } get { return m_P2Pos; } }
	public static int P1Super { set { SetVariable(pP1Super, value, 1); m_P1Super = value; } get { return m_P1Super; } }
	public static int P2Super { set { SetVariable(pP2Super, value, 1); m_P2Super = value; } get { return m_P2Super; } }
	public static int P1MetersA { set { SetVariable(pP1MetersA, value, 1); m_P1MetersA = value; } get { return m_P1MetersA; } }
	public static int P1MetersB { set { SetVariable(pP1MetersB, value, 1); m_P1MetersB = value; } get { return m_P1MetersB; } }
	public static int P2MetersA { set { SetVariable(pP2MetersA, value, 1); m_P2MetersA = value; } get { return m_P2MetersA; } }
	public static int P2MetersB { set { SetVariable(pP2MetersB, value, 1); m_P2MetersB = value; } get { return m_P2MetersB; } }
	public static int P1MaxMeters { set { SetVariable(pP1MaxMeters, value, 1); m_P1MaxMeters = value; } get { return m_P1MaxMeters; } }
	public static int P2MaxMeters { set { SetVariable(pP2MaxMeters, value, 1); m_P2MaxMeters = value; } get { return m_P2MaxMeters; } }
	public static int P1Height { set { SetVariable(pP1Height, value, 1); m_P1Height = value; } get { return m_P1Height; } }
	public static int P2Height { set { SetVariable(pP2Height, value, 1); m_P2Height = value; } get { return m_P2Height; } }

	public static bool Orientation {get {return P1Pos < P2Pos;}}
	public static Game.Keys P1TowardKey { get {return Game.P1Pos > Game.P2Pos ? Game.Keys.LEFT : Game.Keys.RIGHT;}}
	public static Game.Keys P1BackKey { get {return Game.P1Pos > Game.P2Pos ? Game.Keys.RIGHT : Game.Keys.LEFT;}}
	public static Game.Keys P2TowardKey { get {return Game.P1Pos > Game.P2Pos ? Game.Keys.RIGHT : Game.Keys.LEFT;}}
	public static Game.Keys P2BackKey { get {return Game.P1Pos > Game.P2Pos ? Game.Keys.LEFT : Game.Keys.RIGHT;}}

	#region pinvoke shit

	[DllImport("kernel32.dll")]
	static extern IntPtr OpenProcess(ProcessAccessFlags dwDesiredAccess, [MarshalAs(UnmanagedType.Bool)] bool bInheritHandle, int dwProcessId);
	[DllImport("kernel32.dll")]
	static extern bool ReadProcessMemory(IntPtr hProcess, IntPtr lpBaseAddress, [Out] byte[] lpBuffer, int dwSize, out IntPtr lpNumberOfBytesRead);
	[DllImport("kernel32.dll")]
	static extern bool WriteProcessMemory(IntPtr hProcess, IntPtr lpBaseAddress, byte[] lpBuffer, int nSize, out IntPtr lpNumberOfBytesWritten);
	[DllImport("user32.dll")]
	private static extern IntPtr GetForegroundWindow();
	[DllImport("user32.dll")]
	static extern bool GetWindowRect(IntPtr hwnd, out RECT lpRect);
	[DllImport("kernel32.dll")]
	static extern void GetSystemInfo(out SYSTEM_INFO lpSystemInfo);
	[DllImport("kernel32.dll")]
	static extern int VirtualQueryEx(IntPtr hProcess, IntPtr lpAddress, out MEMORY_BASIC_INFORMATION lpBuffer, uint dwLength);
	[DllImport("user32.dll", CharSet = CharSet.Auto)]
	static extern IntPtr SendMessage(IntPtr hWnd, UInt32 Msg, IntPtr wParam, IntPtr lParam);

	const int MEM_COMMIT = 0x00001000;
	const int PAGE_READWRITE = 0x04;
	const int WM_COMMAND = 0x0111;

	[Flags]
	enum ProcessAccessFlags : uint
	{
		All = 0x001F0FFF,
		Terminate = 0x00000001,
		CreateThread = 0x00000002,
		VMOperation = 0x00000008,
		VMRead = 0x00000010,
		VMWrite = 0x00000020,
		DupHandle = 0x00000040,
		SetInformation = 0x00000200,
		QueryInformation = 0x00000400,
		Synchronize = 0x00100000
	}

	[StructLayout(LayoutKind.Sequential)]
	public struct RECT
	{
		public int Left, Top, Right, Bottom;

		public RECT(int left, int top, int right, int bottom)
		{
			Left = left;
			Top = top;
			Right = right;
			Bottom = bottom;
		}

		public RECT(System.Drawing.Rectangle r) : this(r.Left, r.Top, r.Right, r.Bottom) { }

		public int X
		{
			get { return Left; }
			set { Right -= (Left - value); Left = value; }
		}

		public int Y
		{
			get { return Top; }
			set { Bottom -= (Top - value); Top = value; }
		}

		public int Height
		{
			get { return Bottom - Top; }
			set { Bottom = value + Top; }
		}

		public int Width
		{
			get { return Right - Left; }
			set { Right = value + Left; }
		}

		public System.Drawing.Point Location
		{
			get { return new System.Drawing.Point(Left, Top); }
			set { X = value.X; Y = value.Y; }
		}

		public System.Drawing.Size Size
		{
			get { return new System.Drawing.Size(Width, Height); }
			set { Width = value.Width; Height = value.Height; }
		}

		public static implicit operator System.Drawing.Rectangle(RECT r)
		{
			return new System.Drawing.Rectangle(r.Left, r.Top, r.Width, r.Height);
		}

		public static implicit operator RECT(System.Drawing.Rectangle r)
		{
			return new RECT(r);
		}

		public static bool operator ==(RECT r1, RECT r2)
		{
			return r1.Equals(r2);
		}

		public static bool operator !=(RECT r1, RECT r2)
		{
			return !r1.Equals(r2);
		}

		public bool Equals(RECT r)
		{
			return r.Left == Left && r.Top == Top && r.Right == Right && r.Bottom == Bottom;
		}

		public override bool Equals(object obj)
		{
			if (obj is RECT)
				return Equals((RECT)obj);
			else if (obj is System.Drawing.Rectangle)
				return Equals(new RECT((System.Drawing.Rectangle)obj));
			return false;
		}

		public override int GetHashCode()
		{
			return ((System.Drawing.Rectangle)this).GetHashCode();
		}

		public override string ToString()
		{
			return string.Format(System.Globalization.CultureInfo.CurrentCulture, "{{Left={0},Top={1},Right={2},Bottom={3}}}", Left, Top, Right, Bottom);
		}
	}

	[StructLayout(LayoutKind.Sequential)]
	public struct MEMORY_BASIC_INFORMATION
	{
		public IntPtr BaseAddress;
		public IntPtr AllocationBase;
		public uint AllocationProtect;
		public IntPtr RegionSize;
		public uint State;
		public uint Protect;
		public uint Type;
	}

	[StructLayout(LayoutKind.Sequential)]
	public struct SYSTEM_INFO
	{
		public ushort processorArchitecture;
		ushort reserved;
		public uint pageSize;
		public IntPtr minimumApplicationAddress;
		public IntPtr maximumApplicationAddress;
		public IntPtr activeProcessorMask;
		public uint numberOfProcessors;
		public uint processorType;
		public uint allocationGranularity;
		public ushort processorLevel;
		public ushort processorRevision;
	}


	#endregion
}