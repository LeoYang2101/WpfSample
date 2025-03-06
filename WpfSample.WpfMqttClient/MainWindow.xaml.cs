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
                .WithTcpServer("broker.hivemq.com", 1883) // MQTT broker地址和端口
                .WithClientId("WpfMqttClient")
                .WithCleanSession()
                .Build();

            // 连接MQTT broker
            await _mqttClient.ConnectAsync(options);

            // 订阅主题
            await _mqttClient.SubscribeAsync(new MqttTopicFilterBuilder().WithTopic("test/topic").Build());

            // 接收消息
            //_mqttClient.UseApplicationMessageReceivedHandler(e =>
            //{
            //    var message = Encoding.UTF8.GetString(e.ApplicationMessage.Payload);
            //    Dispatcher.Invoke(() =>
            //    {
            //        ReceivedMessageTextBox.Text = message;
            //    });
            //});
        }

        private async void PublishButton_Click(object sender, RoutedEventArgs e)
        {
            //var message = new MqttApplicationMessageBuilder()
            //    .WithTopic("test/topic")
            //    .WithPayload(MessageTextBox.Text)
            //    .WithExactlyOnceQoS()
            //    .WithRetainFlag()
            //    .Build();

            //await _mqttClient.PublishAsync(message);
        }
    }
}