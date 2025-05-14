using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows.Data;

namespace WpfSample.WpfSqlite.Resource
{
    internal class ValueConverter
    {
    }
    public class PriorityToTextConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is int priority)
            {
                return priority switch
                {
                    1 => "高",
                    2 => "中",
                    3 => "低",
                    _ => "中"
                };
            }
            return "中";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class StatusToTextConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is int status)
            {
                return status switch
                {
                    0 => "未开始",
                    1 => "进行中",
                    2 => "已完成",
                    3 => "已取消",
                    _ => "未开始"
                };
            }
            return "未开始";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
