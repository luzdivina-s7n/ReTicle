using System;
using System.Runtime.InteropServices;
using System.Text;

namespace RTC
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            bool createdNew;
            using (var mutex = new System.Threading.Mutex(true, "Local\\rtc_app", out createdNew))
            {
                if (!createdNew) return;
                try { Native.SetProcessDPIAware(); } catch { }
                new RTCApp().Run();
            }
        }
    }

    static class Native
    {
        public const uint WS_POPUP = 0x80000000;
        public const uint WS_EX_LAYERED = 0x00080000;
        public const uint WS_EX_TRANSPARENT = 0x00000020;
        public const uint WS_EX_TOPMOST = 0x00000008;
        public const uint WS_EX_TOOLWINDOW = 0x00000080;
        public const uint WS_EX_NOACTIVATE = 0x08000000;

        public const int WM_DESTROY = 0x0002;
        public const int WM_CONTEXTMENU = 0x007B;
        public const int WM_DISPLAYCHANGE = 0x007E;
        public const int WM_RBUTTONUP = 0x0205;

        public const int SW_SHOWNOACTIVATE = 4;

        public const uint NIM_ADD = 0;
        public const uint NIM_DELETE = 2;

        public const uint NIF_MESSAGE = 0x1;
        public const uint NIF_ICON = 0x2;
        public const uint NIF_TIP = 0x4;

        public const int SM_CXSCREEN = 0;
        public const int SM_CYSCREEN = 1;

        public const uint MF_STRING = 0x0;
        public const uint MF_CHECKED = 0x8;
        public const uint MF_POPUP = 0x10;
        public const uint MF_SEPARATOR = 0x800;

        public const uint TPM_RIGHTBUTTON = 0x2;
        public const uint TPM_RETURNCMD = 0x100;

        public const uint AC_SRC_OVER = 0;
        public const uint AC_SRC_ALPHA = 1;
        public const uint ULW_ALPHA = 2;

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate IntPtr WndProcDelegate(IntPtr hWnd, uint msg, IntPtr wParam, IntPtr lParam);

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        public struct WNDCLASS
        {
            public uint style;
            public IntPtr lpfnWndProc;
            public int cbClsExtra;
            public int cbWndExtra;
            public IntPtr hInstance;
            public IntPtr hIcon;
            public IntPtr hCursor;
            public IntPtr hbrBackground;
            public string lpszMenuName;
            public string lpszClassName;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct POINT { public int X, Y; }

        [StructLayout(LayoutKind.Sequential)]
        public struct SIZE { public int X, Y; }

        [StructLayout(LayoutKind.Sequential)]
        public struct MSG
        {
            public IntPtr hwnd;
            public uint message;
            public IntPtr wParam;
            public IntPtr lParam;
            public uint time;
            public int ptX, ptY;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct BLENDFUNCTION
        {
            public byte BlendOp;
            public byte BlendFlags;
            public byte SourceConstantAlpha;
            public byte AlphaFormat;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct BITMAPINFOHEADER
        {
            public uint biSize;
            public int biWidth;
            public int biHeight;
            public ushort biPlanes;
            public ushort biBitCount;
            public uint biCompression;
            public uint biSizeImage;
            public int biXPelsPerMeter;
            public int biYPelsPerMeter;
            public uint biClrUsed;
            public uint biClrImportant;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct BITMAPINFO
        {
            public BITMAPINFOHEADER bmiHeader;
            public uint bmiColors;
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        public struct NOTIFYICONDATA
        {
            public int cbSize;
            public IntPtr hWnd;
            public uint uID;
            public uint uFlags;
            public uint uCallbackMessage;
            public IntPtr hIcon;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)] public string szTip;
            public uint dwState;
            public uint dwStateMask;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)] public string szInfo;
            public uint uTimeoutOrVersion;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)] public string szInfoTitle;
            public uint dwInfoFlags;
            public Guid guidItem;
            public IntPtr hBalloonIcon;
        }

        [DllImport("user32.dll")] public static extern bool SetProcessDPIAware();
        [DllImport("kernel32.dll")] public static extern IntPtr GetModuleHandle(string lpModuleName);
        [DllImport("user32.dll", CharSet = CharSet.Unicode)] public static extern ushort RegisterClassW(ref WNDCLASS lpWndClass);
        [DllImport("user32.dll", CharSet = CharSet.Unicode)] public static extern IntPtr CreateWindowExW(uint dwExStyle, string lpClassName, string lpWindowName, uint dwStyle, int x, int y, int nWidth, int nHeight, IntPtr hWndParent, IntPtr hMenu, IntPtr hInstance, IntPtr lpParam);
        [DllImport("user32.dll")] public static extern IntPtr DefWindowProcW(IntPtr hWnd, uint msg, IntPtr wParam, IntPtr lParam);
        [DllImport("user32.dll")] public static extern bool DestroyWindow(IntPtr hWnd);
        [DllImport("user32.dll")] public static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);
        [DllImport("user32.dll")] public static extern int GetMessageW(out MSG lpMsg, IntPtr hWnd, uint wMsgFilterMin, uint wMsgFilterMax);
        [DllImport("user32.dll")] public static extern bool TranslateMessage(ref MSG lpMsg);
        [DllImport("user32.dll")] public static extern IntPtr DispatchMessageW(ref MSG lpMsg);
        [DllImport("user32.dll")] public static extern void PostQuitMessage(int nExitCode);
        [DllImport("user32.dll")] public static extern uint RegisterWindowMessageW(string lpString);
        [DllImport("user32.dll")] public static extern IntPtr GetDC(IntPtr hWnd);
        [DllImport("user32.dll")] public static extern int ReleaseDC(IntPtr hWnd, IntPtr hDC);
        [DllImport("user32.dll")] public static extern int GetSystemMetrics(int nIndex);
        [DllImport("user32.dll")] public static extern bool GetCursorPos(out POINT lpPoint);
        [DllImport("user32.dll")] public static extern bool SetForegroundWindow(IntPtr hWnd);
        [DllImport("user32.dll")] public static extern bool PostMessageW(IntPtr hWnd, uint msg, IntPtr wParam, IntPtr lParam);
        [DllImport("shell32.dll", CharSet = CharSet.Unicode)] public static extern uint ExtractIconExW(string lpszFile, int nIconIndex, out IntPtr phiconLarge, out IntPtr phiconSmall, uint nIcons);
        [DllImport("user32.dll")] public static extern bool DestroyIcon(IntPtr hIcon);
        [DllImport("user32.dll")] public static extern IntPtr LoadIconW(IntPtr hInstance, IntPtr lpIconName);
        [DllImport("user32.dll")] public static extern IntPtr CreatePopupMenu();
        [DllImport("user32.dll", CharSet = CharSet.Unicode)] public static extern bool AppendMenuW(IntPtr hMenu, uint uFlags, UIntPtr uIDNewItem, string lpNewItem);
        [DllImport("user32.dll")] public static extern int TrackPopupMenu(IntPtr hMenu, uint uFlags, int x, int y, int nReserved, IntPtr hWnd, IntPtr prcRect);
        [DllImport("user32.dll")] public static extern bool DestroyMenu(IntPtr hMenu);
        [DllImport("gdi32.dll")] public static extern IntPtr CreateCompatibleDC(IntPtr hdc);
        [DllImport("gdi32.dll")] public static extern bool DeleteDC(IntPtr hdc);
        [DllImport("gdi32.dll")] public static extern IntPtr SelectObject(IntPtr hdc, IntPtr hgdiobj);
        [DllImport("gdi32.dll")] public static extern bool DeleteObject(IntPtr hObject);
        [DllImport("gdi32.dll")] public static extern IntPtr CreateDIBSection(IntPtr hdc, ref BITMAPINFO pbmi, uint usage, out IntPtr ppvBits, IntPtr hSection, uint offset);
        [DllImport("user32.dll")] public static extern bool UpdateLayeredWindow(IntPtr hwnd, IntPtr hdcDst, ref POINT pptDst, ref SIZE psize, IntPtr hdcSrc, ref POINT pptSrc, uint crKey, ref BLENDFUNCTION pblend, uint dwFlags);
        [DllImport("shell32.dll", CharSet = CharSet.Unicode)] public static extern bool Shell_NotifyIconW(uint dwMessage, ref NOTIFYICONDATA lpData);
        [DllImport("kernel32.dll", CharSet = CharSet.Unicode)] public static extern uint GetModuleFileNameW(IntPtr hModule, StringBuilder lpFilename, uint nSize);
        [DllImport("kernel32.dll")] public static extern IntPtr GetCurrentProcess();
        [DllImport("kernel32.dll")] public static extern bool SetProcessWorkingSetSize(IntPtr hProcess, IntPtr dwMinimumWorkingSetSize, IntPtr dwMaximumWorkingSetSize);
    }

    class RTCApp
    {
        private const int WinSize = 32;
        private const uint TrayMessage = 0x8001;

        private static RTCApp _self;

        private static readonly int[] Sizes = { 3, 4, 5 };
        private static readonly byte[][] Colors =
        {
            new byte[] { 255, 255, 255 },
            new byte[] { 255, 130, 130 },
            new byte[] { 140, 190, 255 },
            new byte[] { 140, 240, 160 }
        };

        private Native.WndProcDelegate _wndProc;
        private string _className = "rtc_overlay";
        private IntPtr _hwnd = IntPtr.Zero;
        private IntPtr _memDc = IntPtr.Zero;
        private IntPtr _hBitmap = IntPtr.Zero;
        private IntPtr _oldBitmap = IntPtr.Zero;
        private IntPtr _hIcon = IntPtr.Zero;
        private IntPtr _hIconLarge = IntPtr.Zero;
        private IntPtr _hIconSmall = IntPtr.Zero;
        private uint _taskbarCreated;

        private int _sizeIndex = 1;
        private int _colorIndex = 0;
        private int _shapeIndex = 0;
        private int _x, _y;

        public void Run()
        {
            _self = this;
            _wndProc = WndProc;

            _hIcon = LoadTrayIcon();

            var wc = new Native.WNDCLASS();
            wc.lpfnWndProc = Marshal.GetFunctionPointerForDelegate(_wndProc);
            wc.hInstance = Native.GetModuleHandle(null);
            wc.lpszClassName = _className;
            wc.hIcon = _hIconLarge != IntPtr.Zero ? _hIconLarge : _hIcon;
            Native.RegisterClassW(ref wc);

            _taskbarCreated = Native.RegisterWindowMessageW("TaskbarCreated");
            Center();

            _hwnd = Native.CreateWindowExW(
                Native.WS_EX_LAYERED | Native.WS_EX_TRANSPARENT | Native.WS_EX_TOPMOST |
                Native.WS_EX_TOOLWINDOW | Native.WS_EX_NOACTIVATE,
                _className, "rtc", Native.WS_POPUP, _x, _y, WinSize, WinSize,
                IntPtr.Zero, IntPtr.Zero, wc.hInstance, IntPtr.Zero);

            Native.ShowWindow(_hwnd, Native.SW_SHOWNOACTIVATE);

            BuildSurface();
            Show();
            AddTray();
            Trim();

            Native.MSG msg;
            while (Native.GetMessageW(out msg, IntPtr.Zero, 0, 0) > 0)
            {
                Native.TranslateMessage(ref msg);
                Native.DispatchMessageW(ref msg);
            }

            RemoveTray();
            ReleaseSurface();
            if (_hIconLarge != IntPtr.Zero) Native.DestroyIcon(_hIconLarge);
            if (_hIconSmall != IntPtr.Zero) Native.DestroyIcon(_hIconSmall);
        }

        private IntPtr LoadTrayIcon()
        {
            var sb = new StringBuilder(260);
            Native.GetModuleFileNameW(IntPtr.Zero, sb, (uint)sb.Capacity);
            IntPtr large, small;
            Native.ExtractIconExW(sb.ToString(), 0, out large, out small, 1);
            _hIconLarge = large;
            _hIconSmall = small;
            if (small != IntPtr.Zero) return small;
            if (large != IntPtr.Zero) return large;
            return Native.LoadIconW(IntPtr.Zero, (IntPtr)32512);
        }

        private void Center()
        {
            _x = (Native.GetSystemMetrics(Native.SM_CXSCREEN) - WinSize) / 2;
            _y = (Native.GetSystemMetrics(Native.SM_CYSCREEN) - WinSize) / 2;
        }

        private static IntPtr WndProc(IntPtr hwnd, uint msg, IntPtr wParam, IntPtr lParam)
        {
            RTCApp self = _self;
            if (self != null)
            {
                if (msg == TrayMessage)
                {
                    int ev = (int)((long)lParam & 0xFFFF);
                    if (ev == Native.WM_RBUTTONUP || ev == Native.WM_CONTEXTMENU) self.ShowMenu();
                    return IntPtr.Zero;
                }
                if (msg == Native.WM_DISPLAYCHANGE)
                {
                    self.Center();
                    self.Show();
                    return IntPtr.Zero;
                }
                if (msg == Native.WM_DESTROY)
                {
                    Native.PostQuitMessage(0);
                    return IntPtr.Zero;
                }
                if (self._taskbarCreated != 0 && msg == self._taskbarCreated)
                {
                    self.RemoveTray();
                    self.AddTray();
                    return IntPtr.Zero;
                }
            }
            return Native.DefWindowProcW(hwnd, msg, wParam, lParam);
        }

        private void ShowMenu()
        {
            Native.POINT pt;
            Native.GetCursorPos(out pt);

            IntPtr menu = Native.CreatePopupMenu();
            IntPtr sizeMenu = Native.CreatePopupMenu();
            IntPtr colorMenu = Native.CreatePopupMenu();
            IntPtr shapeMenu = Native.CreatePopupMenu();

            AddItem(sizeMenu, 1, "Tiny", _sizeIndex == 0);
            AddItem(sizeMenu, 2, "Small", _sizeIndex == 1);
            AddItem(sizeMenu, 3, "Medium", _sizeIndex == 2);
            Native.AppendMenuW(menu, Native.MF_POPUP | Native.MF_STRING, (UIntPtr)(ulong)(long)sizeMenu, "Size");

            AddItem(shapeMenu, 21, "Dot", _shapeIndex == 0);
            AddItem(shapeMenu, 22, "Cross", _shapeIndex == 1);
            AddItem(shapeMenu, 23, "Ring", _shapeIndex == 2);
            AddItem(shapeMenu, 24, "Square", _shapeIndex == 3);
            Native.AppendMenuW(menu, Native.MF_POPUP | Native.MF_STRING, (UIntPtr)(ulong)(long)shapeMenu, "Shape");

            AddItem(colorMenu, 11, "White", _colorIndex == 0);
            AddItem(colorMenu, 12, "Red", _colorIndex == 1);
            AddItem(colorMenu, 13, "Blue", _colorIndex == 2);
            AddItem(colorMenu, 14, "Green", _colorIndex == 3);
            Native.AppendMenuW(menu, Native.MF_POPUP | Native.MF_STRING, (UIntPtr)(ulong)(long)colorMenu, "Color");

            Native.AppendMenuW(menu, Native.MF_SEPARATOR, UIntPtr.Zero, null);
            AddItem(menu, 99, "Exit", false);

            Native.SetForegroundWindow(_hwnd);
            int cmd = Native.TrackPopupMenu(menu, Native.TPM_RIGHTBUTTON | Native.TPM_RETURNCMD, pt.X, pt.Y, 0, _hwnd, IntPtr.Zero);
            Native.PostMessageW(_hwnd, 0, IntPtr.Zero, IntPtr.Zero);
            Native.DestroyMenu(menu);

            switch (cmd)
            {
                case 1: SetSize(0); break;
                case 2: SetSize(1); break;
                case 3: SetSize(2); break;
                case 11: SetColor(0); break;
                case 12: SetColor(1); break;
                case 13: SetColor(2); break;
                case 14: SetColor(3); break;
                case 21: SetShape(0); break;
                case 22: SetShape(1); break;
                case 23: SetShape(2); break;
                case 24: SetShape(3); break;
                case 99: Exit(); break;
            }
        }

        private static void AddItem(IntPtr menu, int id, string text, bool check)
        {
            uint flags = Native.MF_STRING | (check ? Native.MF_CHECKED : 0);
            Native.AppendMenuW(menu, flags, (UIntPtr)(ulong)id, text);
        }

        private void SetSize(int index)
        {
            _sizeIndex = index;
            BuildSurface();
            Show();
        }

        private void SetColor(int index)
        {
            _colorIndex = index;
            BuildSurface();
            Show();
        }

        private void SetShape(int index)
        {
            _shapeIndex = index;
            BuildSurface();
            Show();
        }

        private void BuildSurface()
        {
            ReleaseSurface();

            IntPtr screenDc = Native.GetDC(IntPtr.Zero);
            _memDc = Native.CreateCompatibleDC(screenDc);
            Native.ReleaseDC(IntPtr.Zero, screenDc);

            var bmi = new Native.BITMAPINFO();
            bmi.bmiHeader.biSize = (uint)Marshal.SizeOf(typeof(Native.BITMAPINFOHEADER));
            bmi.bmiHeader.biWidth = WinSize;
            bmi.bmiHeader.biHeight = -WinSize;
            bmi.bmiHeader.biPlanes = 1;
            bmi.bmiHeader.biBitCount = 32;
            bmi.bmiHeader.biCompression = 0;

            IntPtr bits;
            _hBitmap = Native.CreateDIBSection(_memDc, ref bmi, 0, out bits, IntPtr.Zero, 0);
            _oldBitmap = Native.SelectObject(_memDc, _hBitmap);

            byte[] px = new byte[WinSize * WinSize * 4];
            Render(px);
            Marshal.Copy(px, 0, bits, px.Length);
        }

        private static bool InShape(int shape, float x, float y, float c, float s)
        {
            float dx = x - c, dy = y - c;
            float ax = Math.Abs(dx), ay = Math.Abs(dy);
            float d = (float)Math.Sqrt(dx * dx + dy * dy);
            switch (shape)
            {
                case 0:
                    return d <= s * 0.5f;
                case 1:
                {
                    float half = s, th = Math.Max(1.2f, s * 0.45f) * 0.5f;
                    return (ax <= half && ay <= th) || (ay <= half && ax <= th);
                }
                case 2:
                {
                    float r = s, t = Math.Max(1.2f, s * 0.45f);
                    return d <= r && d >= r - t;
                }
                case 3:
                {
                    float half = s, t = Math.Max(1.2f, s * 0.45f);
                    float m = Math.Max(ax, ay);
                    return m <= half && m >= half - t;
                }
            }
            return false;
        }

        private void Render(byte[] px)
        {
            byte[] c = Colors[_colorIndex];
            float s = Sizes[_sizeIndex];
            float cx = WinSize / 2f;
            float cy = WinSize / 2f;
            float dr = c[0] * 0.7f, dg = c[1] * 0.7f, db = c[2] * 0.7f;
            float ext = s + 1f;
            const int ss = 4;
            const float inv = 1f / (ss * ss);
            int shape = _shapeIndex;

            int i = 0;
            for (int y = 0; y < WinSize; y++)
            {
                for (int x = 0; x < WinSize; x++)
                {
                    float ar = 0, ag = 0, ab = 0, aa = 0;
                    for (int sy = 0; sy < ss; sy++)
                    {
                        float fy = y + (sy + 0.5f) / ss;
                        for (int sx = 0; sx < ss; sx++)
                        {
                            float fx = x + (sx + 0.5f) / ss;
                            if (!InShape(shape, fx, fy, cx, s)) continue;
                            float dx = fx - cx, dy = fy - cy;
                            float t = (float)Math.Sqrt(dx * dx + dy * dy) / ext;
                            if (t > 1) t = 1;
                            ar += c[0] + (dr - c[0]) * t;
                            ag += c[1] + (dg - c[1]) * t;
                            ab += c[2] + (db - c[2]) * t;
                            aa += 1f;
                        }
                    }
                    if (aa <= 0)
                    {
                        px[i] = 0; px[i + 1] = 0; px[i + 2] = 0; px[i + 3] = 0;
                    }
                    else
                    {
                        float a = aa * inv;
                        px[i] = (byte)(ab * inv);
                        px[i + 1] = (byte)(ag * inv);
                        px[i + 2] = (byte)(ar * inv);
                        px[i + 3] = (byte)(a * 255f);
                    }
                    i += 4;
                }
            }
        }

        private void Show()
        {
            if (_memDc == IntPtr.Zero || _hwnd == IntPtr.Zero) return;

            IntPtr screenDc = Native.GetDC(IntPtr.Zero);
            var dst = new Native.POINT { X = _x, Y = _y };
            var size = new Native.SIZE { X = WinSize, Y = WinSize };
            var src = new Native.POINT { X = 0, Y = 0 };
            var blend = new Native.BLENDFUNCTION
            {
                BlendOp = (byte)Native.AC_SRC_OVER,
                SourceConstantAlpha = 255,
                AlphaFormat = (byte)Native.AC_SRC_ALPHA
            };
            Native.UpdateLayeredWindow(_hwnd, screenDc, ref dst, ref size, _memDc, ref src, 0, ref blend, Native.ULW_ALPHA);
            Native.ReleaseDC(IntPtr.Zero, screenDc);
        }

        private void ReleaseSurface()
        {
            if (_memDc == IntPtr.Zero) return;
            if (_oldBitmap != IntPtr.Zero) Native.SelectObject(_memDc, _oldBitmap);
            if (_hBitmap != IntPtr.Zero) Native.DeleteObject(_hBitmap);
            Native.DeleteDC(_memDc);
            _memDc = IntPtr.Zero;
            _hBitmap = IntPtr.Zero;
            _oldBitmap = IntPtr.Zero;
        }

        private void AddTray()
        {
            var nid = new Native.NOTIFYICONDATA();
            nid.cbSize = Marshal.SizeOf(typeof(Native.NOTIFYICONDATA));
            nid.hWnd = _hwnd;
            nid.uID = 1;
            nid.uFlags = Native.NIF_MESSAGE | Native.NIF_ICON | Native.NIF_TIP;
            nid.uCallbackMessage = TrayMessage;
            nid.hIcon = _hIcon;
            nid.szTip = "rtc";
            Native.Shell_NotifyIconW(Native.NIM_ADD, ref nid);
        }

        private void RemoveTray()
        {
            var nid = new Native.NOTIFYICONDATA();
            nid.cbSize = Marshal.SizeOf(typeof(Native.NOTIFYICONDATA));
            nid.hWnd = _hwnd;
            nid.uID = 1;
            Native.Shell_NotifyIconW(Native.NIM_DELETE, ref nid);
        }

        private void Exit()
        {
            Native.DestroyWindow(_hwnd);
        }

        private void Trim()
        {
            Native.SetProcessWorkingSetSize(Native.GetCurrentProcess(), (IntPtr)(-1), (IntPtr)(-1));
        }
    }
}
