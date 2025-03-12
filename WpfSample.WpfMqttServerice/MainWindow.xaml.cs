using MQTTnet;
using MQTTnet.Server;
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

namespace WpfSample.WpfMqttServerice
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private MqttServer _mqttServer;

        public MainWindow()
        {
            InitializeComponent();
            StartMqttServer().ConfigureAwait(false);
        }

        private async Task StartMqttServer()
        {
            // 创建 MQTT 服务器
            var options = new MqttServerOptionsBuilder()
                .WithDefaultEndpoint() // 默认端口 1883
                .WithDefaultEndpointPort(1883).Build();

            _mqttServer = new MqttFactory().CreateMqttServer(options);



            // 处理客户端连接事件
            _mqttServer.ClientConnectedAsync += e =>
            {
                Dispatcher.Invoke(() =>
                {
                    LogTextBox.AppendText($"客户端已连接: {e.ClientId}\n");
                });
                return Task.CompletedTask;
            };

            // 处理客户端断开连接事件
            _mqttServer.ClientDisconnectedAsync += e =>
            {
                Dispatcher.Invoke(() =>
                {
                    LogTextBox.AppendText($"客户端已断开: {e.ClientId}\n");
                });
                return Task.CompletedTask;
            };

            // 处理消息接收事件
            _mqttServer.ApplicationMessageEnqueuedOrDroppedAsync += e =>
            {
                var message = Encoding.UTF8.GetString(e.ApplicationMessage.Payload);
                Dispatcher.Invoke(() =>
                {
                    LogTextBox.AppendText($"收到消息: {message}\n");
                });
                return Task.CompletedTask;
            };

            // 启动 MQTT 服务器
            await _mqttServer.StartAsync();
            LogTextBox.AppendText("MQTT 服务器已启动\n");
        }

        private async void StopButton_Click(object sender, RoutedEventArgs e)
        {
            if (_mqttServer != null)
            {
                await _mqttServer.StopAsync();
                LogTextBox.AppendText("MQTT 服务器已停止\n");
            }
        }
    }
}