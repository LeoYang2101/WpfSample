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

        private void border_MouseEnter(object sender, MouseEventArgs e)
        {
            var element = (UIElement)sender;
            var adornerLayer = AdornerLayer.GetAdornerLayer(element);
            Adorner[] adorners = adornerLayer.GetAdorners(element);
            if (adornerLayer != null && adorners == null)
            {
                adornerLayer.Add(new AnchorAdorner(element));
                adornerLayer.Add(new RubberAdorner(element));
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