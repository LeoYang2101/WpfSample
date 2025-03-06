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
using WebSocketSharp.Server;
using WebSocketSharp;
using System.Diagnostics;

namespace WpfSample.WpfWebSocketSharpServerice
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private WebSocketServer _webSocketServer;        

        public MainWindow()
        {
            InitializeComponent();
        }

        // 启动 WebSocket 服务端
        private void StartServerButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // 创建 WebSocket 服务器，监听指定的地址和端口
                _webSocketServer = new WebSocketServer("ws://localhost:5000");

                // 添加 WebSocket 服务
                //_webSocketServer.AddWebSocketService<Chat>("/chat");
                _webSocketServer.AddWebSocketService<SubscriptionServer>("/chat");

                SubscriptionServer.SetServiceManager(_webSocketServer);

                // 启动服务器
                _webSocketServer.Start();
                MessageBox.Show("WebSocket server started on ws://localhost:5000/chat");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error starting server: {ex.Message}");
            }
        }

        // 停止 WebSocket 服务端
        private void StopServerButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (_webSocketServer != null)
                {
                    _webSocketServer.Stop();
                    MessageBox.Show("WebSocket server stopped.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error stopping server: {ex.Message}");
            }
        }

        // 向订阅的客户端广播消息
        private void BoardMessageButton_Click(object sender, RoutedEventArgs e)
        {
            if (_webSocketServer != null)
            {
                SubscriptionServer.BroadcastToSubscribedClients("Broadcast message to all subscribed clients!");
                MessageBox.Show("Message broadcasted to subscribed clients.");
            }
            else
            {
                MessageBox.Show("Server is not running.");
            }
        }

        // 自定义 WebSocket 服务
        public class Chat : WebSocketBehavior
        {
            protected override void OnMessage(MessageEventArgs e)
            {
                // 当收到客户端消息时触发
                if (e.IsBinary)
                {
                    // 接收二进制数据
                    byte[] binaryData = e.RawData;

                    // 解析二进制数据（示例：将二进制数据转换为字符串）
                    string receivedMessage = Encoding.UTF8.GetString(binaryData);
                    Debug.WriteLine($"Received binary data: {BitConverter.ToString(binaryData)}");
                    Debug.WriteLine($"Converted to string: {receivedMessage}");

                    // 向客户端发送响应
                    Send($"Echo: {receivedMessage}");
                }
                else if (e.IsText)
                {
                    // 处理文本数据
                    Debug.WriteLine($"Received text data: {e.Data}");
                    // 向客户端发送响应
                    Send($"Echo: {e.Data}");
                }
            }

            protected override void OnOpen()
            {
                // 当客户端连接时触发
                Debug.WriteLine("Client connected.");
            }

            protected override void OnClose(CloseEventArgs e)
            {
                // 当客户端断开连接时触发
                Debug.WriteLine("Client disconnected.");
            }
        }


        public class SubscriptionServer : WebSocketBehavior
        {
            private static HashSet<string> _subscribedClients = new HashSet<string>();
            private static WebSocketServer _serviceManager;

            // 设置服务管理器（在服务器启动时调用）
            public static void SetServiceManager(WebSocketServer manager)
            {
                _serviceManager = manager;
            }

            protected override void OnMessage(MessageEventArgs e)
            {
                if (e.IsText)
                {
                    string message = e.Data;
                    if (e.Data == "ping")
                    {
                        Send("pong"); // 服务器回复 pong
                        Debug.WriteLine("收到ping，回复pong");
                    }
                    // 处理客户端订阅请求
                    else if(message.StartsWith("SUBSCRIBE:"))
                    {
                        string clientId = ID; // 获取客户端唯一 ID
                        _subscribedClients.Add(clientId);
                        Send($"Subscribed successfully! Your ID: {clientId}");
                        Console.WriteLine($"Client {clientId} subscribed.");
                    }
                    // 处理客户端取消订阅请求
                    else if (message.StartsWith("UNSUBSCRIBE:"))
                    {
                        string clientId = ID;
                        _subscribedClients.Remove(clientId);
                        Send("Unsubscribed successfully!");
                        Console.WriteLine($"Client {clientId} unsubscribed.");
                    }
                    // 处理其他消息
                    else
                    {
                        Console.WriteLine($"Received from client {ID}: {message}");
                        Send($"Echo: {message}");
                    }
                }
            }

            protected override void OnOpen()
            {
                Console.WriteLine($"Client connected: {ID}");
            }

            protected override void OnClose(CloseEventArgs e)
            {
                _subscribedClients.Remove(ID);
                Console.WriteLine($"Client disconnected: {ID}");
            }

            // 向所有订阅的客户端广播消息
            public static void BroadcastToSubscribedClients(string message)
            {
                if (_serviceManager == null)
                {
                    Console.WriteLine("Error: ServiceManager is not initialized.");
                    return;
                }

                foreach (var clientId in _subscribedClients.ToList())
                {
                    // 通过 WebSocketServiceManager 获取会话
                    if (_serviceManager.WebSocketServices["/chat"].Sessions.TryGetSession(clientId, out var session))
                    {
                        session.Context.WebSocket.Send(message);
                    }
                }
            }
        }
    }
}