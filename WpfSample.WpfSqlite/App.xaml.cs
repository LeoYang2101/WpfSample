using System.Configuration;
using System.Data;
using System.Windows;
using WpfSample.WpfSqlite.Views;

namespace WpfSample.WpfSqlite
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            new MainWindow().Show();
        }
    }

}
