using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Interop;

namespace WpfSample.KeyboardShortcut
{
    public class GlobalHotkeys
    {
        // 定义热键 ID
        private const int HOTKEY_ID_1 = 9000;
        private const int HOTKEY_ID_2 = 9001;

        public GlobalHotkeys(IntPtr hwnd)
        {
            // 注册 Ctrl + Alt + C
            RegisterHotKey(hwnd, HOTKEY_ID_1, MOD_CONTROL | MOD_ALT, (int)Keys.T);

            // 注册 Ctrl + Alt + O
            RegisterHotKey(hwnd, HOTKEY_ID_2, MOD_CONTROL | MOD_ALT, (int)Keys.O);


            // 添加消息钩子
            HwndSource source = HwndSource.FromHwnd(hwnd);
            source.AddHook(HwndHook);


            //RegisterHotKey(hwnd, 1, MOD_CONTROL | MOD_ALT, (int)KeyInterop.VirtualKeyFromKey(Key.C));            

            //var source = System.Windows.Interop.HwndSource.FromHwnd(hwnd);
            //source.AddHook(HwndHook);
        }

        public void Close(IntPtr hwnd)
        {
            // 注销热键
            UnregisterHotKey(hwnd, HOTKEY_ID_1);
            UnregisterHotKey(hwnd, HOTKEY_ID_2);
            Debug.WriteLine("热键已注销");
        }

        [DllImport("user32.dll")]
        private static extern bool RegisterHotKey(IntPtr hWnd, int id, int fsModifiers, int vk);

        [DllImport("user32.dll")]
        private static extern bool UnregisterHotKey(IntPtr hWnd, int id);

        private const int MOD_ALT = 0x0001;
        private const int MOD_CONTROL = 0x0002;
        private const int MOD_SHIFT = 0x0004;
        private const int WM_HOTKEY = 0x0312;
        private bool hotkey1Pressed = false;
        private bool hotkey2Pressed = false;

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
            if (msg != WM_HOTKEY)
            {
                Debug.WriteLine("未知热键");
                return IntPtr.Zero;
            }
            switch (wParam.ToInt32())
            {
                case HOTKEY_ID_1:
                    HotKey1Event();
                    break;
                case HOTKEY_ID_2:
                    HotKey2Event();
                    break;
                default:
                    Debug.WriteLine($"未知wParam:{wParam}");
                    break;
            }
            return IntPtr.Zero;
        }


        private void HotKey1Event()
        {
            if (!hotkey1Pressed)
            {
                hotkey1Pressed = true;
                Debug.WriteLine("麦克风关闭中");
                Task.Run(() =>
                {
                    MicrophoneController.Mute();
                    Debug.WriteLine("麦克风已关闭");
                }).ContinueWith(o =>
                {
                    hotkey1Pressed = false;
                    Debug.WriteLine("释放");
                });
            }
        }

        private void HotKey2Event()
        {

            if (!hotkey2Pressed)
            {
                hotkey2Pressed = true;
                Debug.WriteLine("麦克风开启中");
                Task.Run(() =>
                {
                    MicrophoneController.Unmute();
                    Debug.WriteLine("麦克风已开启");
                }).ContinueWith(o =>
                {
                    hotkey2Pressed = false;
                    Debug.WriteLine("释放");
                });
            }
        }
    }
}
