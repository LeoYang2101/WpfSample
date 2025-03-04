using System.Diagnostics;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using WpfSample.KeyboardShortcut.Common;

namespace WpfSample.KeyboardShortcut
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        GlobalHotkeys? globalHotkeys=null;
        public MainWindow()
        {
            InitializeComponent();
            this.Loaded += MainWindow_Loaded;
            this.Closed += MainWindow_Closed;

            return;

            // 设置为无界面运行
            //ConsoleWindow.Hide();

            // 初始化键盘钩子
            using (var keyboardHook = new KeyboardHook())
            {
                keyboardHook.OnKeyCombinationPressed += (combination) =>
                {
                    if (combination == "Ctrl+Alt+T")
                    {
                        MicrophoneController.Mute();
                        Debug.WriteLine("麦克风已关闭");
                    }
                    else if (combination == "Ctrl+Alt+S")
                    {
                        MicrophoneController.Unmute();
                        Debug.WriteLine("麦克风已开启");
                    }
                };

                // 设置开机自启动
                SetStartup();

                // 保持程序运行
                //while (true)
                //{
                //    Thread.Sleep(1000);
                //}
            }
        }

        private void MainWindow_Closed(object? sender, EventArgs e)
        {
            globalHotkeys?.Close(new System.Windows.Interop.WindowInteropHelper(this).Handle);
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            globalHotkeys = new GlobalHotkeys(new System.Windows.Interop.WindowInteropHelper(this).Handle);
            SetStartup();

            SetWindowProperties();
        }

        // 设置开机自启动
        private static void SetStartup()
        {
            //启动目录方式
            StartUpAutomatically startUpAutomatically = new StartUpAutomatically();
            startUpAutomatically.SetMeAutoStart(true);
            //startUpAutomatically.SetMeAutoStart(false);

            //using (var key = Microsoft.Win32.Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true))
            //{
            //    key.SetValue("MicControlApp", Process.GetCurrentProcess().MainModule.FileName);
            //}
        }


        private void SetWindowProperties()
        {
            // 设置窗口初始状态
            this.Visibility = Visibility.Hidden;

            // 使用Win32 API修改扩展样式
            var handle = new WindowInteropHelper(this).Handle;
            if (handle != IntPtr.Zero)
            {
                int extendedStyle = Win32.GetWindowLong(handle, Win32.GWL_EXSTYLE);
                Win32.SetWindowLong(handle, Win32.GWL_EXSTYLE,
                    extendedStyle | Win32.WS_EX_TOOLWINDOW | Win32.WS_EX_NOACTIVATE);
            }
        }

        internal static class Win32
        {
            public const int GWL_EXSTYLE = -20;
            public const int WS_EX_TOOLWINDOW = 0x00000080;
            public const int WS_EX_NOACTIVATE = 0x08000000;

            [System.Runtime.InteropServices.DllImport("user32.dll")]
            public static extern int GetWindowLong(IntPtr hWnd, int nIndex);

            [System.Runtime.InteropServices.DllImport("user32.dll")]
            public static extern int SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);
        }

    }
}