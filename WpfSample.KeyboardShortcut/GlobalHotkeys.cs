using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace WpfSample.KeyboardShortcut
{
    public class GlobalHotkeys
    {
        public GlobalHotkeys(IntPtr hwnd)
        {
            RegisterHotKey(hwnd, 1, MOD_CONTROL | MOD_ALT, (int)KeyInterop.VirtualKeyFromKey(Key.T));            
            //RegisterHotKey(hwnd, 1, MOD_CONTROL | MOD_ALT, (int)61);

            var source = System.Windows.Interop.HwndSource.FromHwnd(hwnd);
            source.AddHook(HwndHook);
        }

        [DllImport("user32.dll")]
        private static extern bool RegisterHotKey(IntPtr hWnd, int id, int fsModifiers, int vk);

        [DllImport("user32.dll")]
        private static extern bool UnregisterHotKey(IntPtr hWnd, int id);

        private const int MOD_ALT = 0x0001;
        private const int MOD_CONTROL = 0x0002;
        private const int MOD_SHIFT = 0x0004;
        private const int WM_HOTKEY = 0x0312;
        private bool hotkeyPressed = false;

        //protected override void OnSourceInitialized(EventArgs e)
        //{
        //    base.OnSourceInitialized(e);
        //    var hwnd = new System.Windows.Interop.WindowInteropHelper(this).Handle;
        //    var source = System.Windows.Interop.HwndSource.FromHwnd(hwnd);
        //    source.AddHook(HwndHook);
        //}

        private IntPtr HwndHook(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            Debug.WriteLine($"触发HwndHook,msg:{msg},wParam:{wParam}");
            if (msg == WM_HOTKEY && (int)wParam == 1)
            {
                if (!hotkeyPressed)
                {
                    hotkeyPressed = true;

                    Debug.WriteLine("触发全局快捷键 Ctrl+Alt+T");
                    //直播中重启摄像头
                    Task.Run(async () =>
                    {
                        
                            MicrophoneController.Mute();
                            Debug.WriteLine("麦克风已关闭");
                        Thread.Sleep(2000);
                            MicrophoneController.Unmute();
                            Debug.WriteLine("麦克风已开启");
                        
                    }).ContinueWith(o =>
                    {
                        hotkeyPressed = false;
                        Console.WriteLine("触发复原");
                    });
                }
            }
            return IntPtr.Zero;
        }
    }
}
