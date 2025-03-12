using Autofac;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfSample.WpfAutofac
{
    public class AutofacModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            // 注册服务
            builder.RegisterType<MyService>().As<IMyService>().SingleInstance();

            // 注册视图模型
            builder.RegisterType<MainViewModel>();

            // 注册视图
            builder.RegisterType<MainWindow>();
        }
    }
}
