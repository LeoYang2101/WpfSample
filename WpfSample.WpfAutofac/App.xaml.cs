using Autofac;
using Autofac.Features.Indexed;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Net.NetworkInformation;
using System.Windows;

namespace WpfSample.WpfAutofac
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private IContainer _container;

        //protected override void OnStartup(StartupEventArgs e)
        //{
        //    base.OnStartup(e);

        //    // 配置 Autofac 容器
        //    var builder = new ContainerBuilder();
        //    ConfigureServices(builder);

        //    _container = builder.Build();

        //    // 从容器中解析 MainWindow 并显示
        //    var mainWindow = _container.Resolve<MainWindow>();
        //    mainWindow.Show();
        //}

        private void Application_Startup(object sender, StartupEventArgs e)
        {
            // 配置 Autofac 容器
            var builder = new ContainerBuilder();            
            ConfigureServices(builder);

            _container = builder.Build();

            // 从容器中解析 MainWindow 并显示
            var mainWindow = _container.Resolve<MainWindow>();
            mainWindow.Show();

            //--------------2.接口类被多个实现类实现的使用--------------

            //字符串方式获取
            //var dog = _container.ResolveNamed<IAnimal>("Doge");//通过IOC容器创建对象
            //dog.SayHello();

            //枚举类型获取
            var animal = _container.Resolve<IIndex<AnumalType, IAnimal>>();
            var doge = animal[AnumalType.Doge];
            doge.SayHello();
        }

        private void ConfigureServices(ContainerBuilder builder)
        {
            //SingleInstance：单例模式，整个容器中只有一个实例
            //InstancePerDependency：每次请求都会创建一个新的实例（默认行为）
            //InstancePerLifetimeScope：在同一个生命周期作用域内使用同一个实例

            //// 注册服务
            //builder.RegisterType<MyService>().As<IMyService>().SingleInstance();

            //// 注册视图模型
            //builder.RegisterType<MainViewModel>();

            //// 注册视图
            //builder.RegisterType<MainWindow>();

            //将注册逻辑拆分到模块中
            builder.RegisterModule<AutofacModule>();

            //--------------1.接口类被多个实现类实现的注册--------------

            //Named方式，注入的时候使用字符串进行区分
            //builder.RegisterType<Doge>().Named<IAnimal>("Doge");//映射对象
            //builder.RegisterType<Pig>().Named<IAnimal>("Pig");//映射对象

            //Keyed方式，注入的时候使用枚举类型进行区分
            builder.RegisterType<Doge>().Keyed<IAnimal>(AnumalType.Doge);//映射对象
            builder.RegisterType<Pig>().Keyed<IAnimal>(AnumalType.Pig);//映射对象
        }

        public enum AnumalType
        {
            Doge, Pig
        }
        public interface IAnimal
        {
            void SayHello();
        }
        public class Doge : IAnimal
        {
            public void SayHello()
            {
                MessageBox.Show("我是小狗，汪汪汪~");
            }
        }
        public class Pig : IAnimal
        {
            public void SayHello()
            {
                MessageBox.Show("我是小猪，呼呼呼~");
            }
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
