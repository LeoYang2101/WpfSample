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
using System.Xml.Linq;

namespace WpfSample.WpfAdorner
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }


        private void Button_Loaded(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn) 
            { 
                //获取AdornerLayer，并添加自定义Adorner
                AdornerLayer adornerLayer=AdornerLayer.GetAdornerLayer(btn);
                if (adornerLayer != null)
                {
                    adornerLayer.Add(new BorderAdorner(btn));
                }
            }
        }

        private void Border_Loaded(object sender, RoutedEventArgs e)
        {
            if (sender is Border border)
            {
                //获取AdornerLayer，并添加自定义Adorner
                AdornerLayer adornerLayer = AdornerLayer.GetAdornerLayer(border);
                if (adornerLayer != null)
                {
                    adornerLayer.Add(new BorderAdorner(border)); 
                    adornerLayer.Add(new AnchorAdorner(border));
                }
            }
        }



        private void border_MouseEnter(object sender, MouseEventArgs e)
        {
            var element = (UIElement)sender;
            var adornerLayer = AdornerLayer.GetAdornerLayer(element);
            Adorner[] adorners = adornerLayer.GetAdorners(element);
            if (adornerLayer != null && adorners == null)
            {
                adornerLayer.Add(new AnchorAdorner(element));
            }
        }

        private void border_MouseLeave(object sender, MouseEventArgs e)
        {
            var element = (UIElement)sender;
            var adornerLayer = AdornerLayer.GetAdornerLayer(element);
            Adorner[] adorners = adornerLayer.GetAdorners(element);
            if (adorners != null)
            {
                foreach (var adorner in adorners)
                {
                    adornerLayer.Remove(adorner);
                }
            }
        }
    }
}