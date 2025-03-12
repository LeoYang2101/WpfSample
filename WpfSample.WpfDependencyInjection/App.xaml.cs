using Microsoft.Extensions.DependencyInjection;
using System.Configuration;
using System.Data;
using System.Windows;

namespace WpfSample.WpfDependencyInjection
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private IServiceProvider _serviceProvider;


        private void ConfigureServices(IServiceCollection services)
        {
            services.RegisterServiceFromAssembly();

            // 注册服务
            //services.AddSingleton<IMyService, MyService>();

            // 注册窗口和页面
            //services.AddTransient<MainWindow>();
            //services.AddTransient<OtherWindow>();
        }

        private void Application_Startup(object sender, StartupEventArgs e)
        {

            // 配置服务
            var serviceCollection = new ServiceCollection();
            //serviceCollection.RegisterServiceFromAssembly();

            ConfigureServices(serviceCollection);

            // 构建服务提供程序
            _serviceProvider = serviceCollection.BuildServiceProvider();

            // 显示主窗口
            var mainWindow = _serviceProvider.GetRequiredService<MainWindow>();
            mainWindow.Show();
        }
    }


    public interface IMyService
    {
        void DoSomething();
    }

    public class MyService : IMyService
    {
        public void DoSomething()
        {
            MessageBox.Show("Service is doing something!");
        }
    }



}
