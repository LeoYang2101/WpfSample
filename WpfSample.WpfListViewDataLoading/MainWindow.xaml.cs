using System.Diagnostics;
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

namespace WpfSample.WpfListViewDataLoading
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = new MainWindowViewModel();
        }

        private void ListView_ScrollChanged(object sender, ScrollChangedEventArgs e)
        {
            //listView
            GetVisibleItems();
        }

        private void GetVisibleItems()
        {
            // 获取 ScrollViewer 控件
            ScrollViewer scrollViewer = FindVisualChild<ScrollViewer>(listView);

            // 获取可见的项
            int firstVisibleIndex = (int)scrollViewer.VerticalOffset;
            int visibleItemCount = (int)scrollViewer.ViewportHeight;

            // 确定可见项的范围
            int lastVisibleIndex = firstVisibleIndex + visibleItemCount;

            // 输出可见项的索引范围
            Debug.WriteLine("可见项范围：{0} - {1}", firstVisibleIndex, lastVisibleIndex);

            for (int i = firstVisibleIndex; i < lastVisibleIndex; i++)
            {
                if (listView.Items[i] is Student stu)
                {
                    stu.Update();
                }
            }
        }

        // 递归查找指定类型的子元素
        private T FindVisualChild<T>(DependencyObject parent) where T : DependencyObject
        {
            int childCount = VisualTreeHelper.GetChildrenCount(parent);
            for (int i = 0; i < childCount; i++)
            {
                DependencyObject child = VisualTreeHelper.GetChild(parent, i);
                if (child is T typedChild)
                    return typedChild;

                T childItem = FindVisualChild<T>(child);
                if (childItem != null)
                    return childItem;
            }

            return null;
        }

        private void listView_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {

        }

        private void listView_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            GetVisibleItems();
        }
    }
}