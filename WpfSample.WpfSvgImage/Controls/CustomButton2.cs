using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace WpfSample.WpfSvgImage.Controls
{
    public class CustomButton2 : Button,INotifyCollectionChanged
    {
        public Uri SvgPath {
            get { return (Uri)GetValue(SvgPathProperty); }
            set
            {
                SetValue(SvgPathProperty, value);
                //OnPropertyChanged("IsChecked");
            }
        }

        public static readonly DependencyProperty SvgPathProperty =
            DependencyProperty.Register(nameof(SvgPath), typeof(Uri), typeof(CustomButton2), new PropertyMetadata(null));

        public ImageSource IconImage
        {
            get { return (ImageSource)GetValue(IconImageProperty); }
            set { SetValue(IconImageProperty, value); }
        }
        public static readonly DependencyProperty IconImageProperty =
            DependencyProperty.Register(nameof(IconImage), typeof(ImageSource), typeof(CustomButton2), new PropertyMetadata(null));


        public string ButtonText
        {
            get { return (string)GetValue(ButtonTextProperty); }
            set { SetValue(ButtonTextProperty, value); }
        }
        // 按钮文本依赖属性
        public static readonly DependencyProperty ButtonTextProperty =
            DependencyProperty.RegisterAttached(
                "ButtonText",
                typeof(string),
                typeof(CustomButton2),
                new PropertyMetadata(string.Empty));


        public event NotifyCollectionChangedEventHandler? CollectionChanged;
    }
}
