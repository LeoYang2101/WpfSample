using MQTTnet.Client;
using MQTTnet;
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
using MQTTnet.Server;
using System.Diagnostics;

namespace WpfSample.WpfMqttClient
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private IMqttClient _mqttClient;


        public MainWindow()
        {
            InitializeComponent();
            ConnectToMqttBroker().ConfigureAwait(false);
        }

        private async Task ConnectToMqttBroker()
        {
            // 创建MQTT客户端
            var factory = new MqttFactory();
            _mqttClient = factory.CreateMqttClient();

            // 配置MQTT客户端选项
            var options = new MqttClientOptionsBuilder()
                //.WithTcpServer("broker.hivemq.com", 1883) // MQTT broker地址和端口,免费MQTT服务器用于测试，这个会收到一些其他的消息
                //.WithTcpServer("broker.emqx.io", 1883) // MQTT broker地址和端口
                .WithTcpServer("localhost", 1883) // MQTT broker地址和端口
                .WithCredentials("username", "password")//Account,Password
                .WithClientId("windows_" +DateTime.Now.ToString("hh:mm:ss fff"))//"windows_" + EnvironmentHelper.GetDeviceIdentification()
                .WithTlsOptions(new MqttClientTlsOptions() { UseTls=false})// 是否使用 tls加密
                .WithCleanSession()
                .WithKeepAlivePeriod(TimeSpan.FromSeconds(60))
                .Build();            

            // 注册消息接收事件
            //_mqttClient.ApplicationMessageReceivedAsync += async e =>
            //{
            //    var message = Encoding.UTF8.GetString(e.ApplicationMessage.Payload);
            //    Dispatcher.Invoke(() =>
            //    {
            //        ReceivedMessageTextBox.Text = message;
            //    });
            //};
            _mqttClient.ConnectedAsync += async e =>
            {
                Debug.WriteLine("Connected to MQTT broker.");
            };
            _mqttClient.DisconnectedAsync += async e =>
            {
                Debug.WriteLine("Disconnected from MQTT broker.");
            };
            _mqttClient.ApplicationMessageReceivedAsync += e =>
            {

                var message = Encoding.UTF8.GetString(e.ApplicationMessage.Payload);
                Debug.WriteLine($"{DateTime.Now.ToString("HH:mm:ss fff")} Received application message,Topic:{e.ApplicationMessage.Topic},Message:{message},Qos:{e.ApplicationMessage.QualityOfServiceLevel}");
                Dispatcher.Invoke(() =>
                {
                    ReceivedMessageTextBox.Text += message;
                });
                return Task.CompletedTask;
            };

            // 连接MQTT broker
            await _mqttClient.ConnectAsync(options,CancellationToken.None);

            // 订阅主题
            await _mqttClient.SubscribeAsync(new MqttTopicFilterBuilder().WithTopic("test/topic").Build());
            //await _mqttClient.SubscribeAsync(new MqttTopicFilterBuilder().WithTopic("test/topic/bbb").Build());
        }

        private async void PublishButton_Click(object sender, RoutedEventArgs e)
        {
            if (!_mqttClient.IsConnected)
            {
                MessageBox.Show("未连接到服务器");
                return;
            }
            var message = new MqttApplicationMessageBuilder()
                .WithTopic("test/topic")
                .WithPayload(MessageTextBox.Text)
                //.WithExactlyOnceQoS()
                .WithRetainFlag()
                .Build();

            var resp= await _mqttClient.PublishAsync(message);
        }
    }
}
