using WebSocketSharp;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using Newtonsoft.Json.Linq;
using static System.Net.Mime.MediaTypeNames;
using System.Diagnostics;

namespace WpfSample.WpfWebSocket
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private WebSocket _webSocket;

        public MainWindow()
        {
            InitializeComponent();

            InitTimer();
        }

        // 连接按钮点击事件
        private void ConnectButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // 创建 WebSocket 客户端
                _webSocket = new WebSocket("ws://localhost:5000/chat"); // WebSocket 服务端地址
                
                // 注册事件处理程序
                _webSocket.OnOpen += (s, ev) =>
                {
                    Dispatcher.Invoke(() =>
                    {
                        MessageBox.Show("Connected to WebSocket server!");
                    });
                };

                _webSocket.OnMessage += (s, ev) =>
                {
                    Dispatcher.Invoke(() =>
                    {
                        MessageBox.Show($"Received: {ev.Data}");
                    });
                };

                _webSocket.OnError += (s, ev) =>
                {
                    Dispatcher.Invoke(() =>
                    {
                        MessageBox.Show($"Error: {ev.Message}");
                    });
                };

                _webSocket.OnClose += (s, ev) =>
                {
                    Dispatcher.Invoke(() =>
                    {
                        MessageBox.Show("Disconnected from WebSocket server.");
                    });
                };

                // 连接到 WebSocket 服务端
                _webSocket.Connect();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }
        }

        // 发送消息按钮点击事件
        private void SendButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (_webSocket != null && _webSocket.IsAlive)
                {
                    string message = "Hello, WebSocket Server!";
                    _webSocket.Send(message);
                    MessageBox.Show("Message sent!");
                }
                else
                {
                    MessageBox.Show("WebSocket is not connected.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }
        }

        //发送二进制消息按钮点击事件
        private void SendBinaryButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (_webSocket != null && _webSocket.IsAlive)
                {
                    byte[] binaryData = { 0x01, 0x02, 0x03, 0x04 };
                    _webSocket.Send(binaryData);
                    MessageBox.Show("Binary message sent!");                    
                }
                else
                {
                    MessageBox.Show("WebSocket is not connected.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }
        }

        // 断开连接按钮点击事件
        private void DisconnectButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (_webSocket != null && _webSocket.IsAlive)
                {
                    _webSocket.Close();
                    MessageBox.Show("Disconnected from WebSocket server.");
                }
                else
                {
                    MessageBox.Show("WebSocket is not connected.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }
        }


        private void SubscribeButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (_webSocket != null && _webSocket.IsAlive)
                {
                    _webSocket.Send("SUBSCRIBE:");
                    MessageBox.Show("Message sent!");
                }
                else
                {
                    MessageBox.Show("WebSocket is not connected.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }
        }



        #region 心跳包

        private DispatcherTimer _timer;

        private void InitTimer()
        {
            _timer = new DispatcherTimer();
            _timer.Tick += _timer_Tick;
            _timer.Interval = new TimeSpan(0, 0, 5);
            _timer.Start();
        }

        private void _timer_Tick(object sender, EventArgs e)
        {
            try
            {
                //只有在已经连接成功WebSocket的时候，才需要发送心跳包
                if (_webSocket != null && _webSocket.IsAlive)
                {                    
                    _webSocket.Send("ping");
                }
            }
            catch (Exception exception)
            {
                Debug.WriteLine(exception);
            }
        }

        #endregion

    }
}