using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace ArticleWpfApp.Converters
{
    class ErrorToVisibilityConv : IValueConverter
    {
        public object Convert(object value, System.Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            Visibility ReturnValue = Visibility.Hidden;
            if (value != null)
            {
                var str = (string)value;
                if (str.Length>0)
                ReturnValue = Visibility.Visible;                
            }
            return ReturnValue;
        }

        public object ConvertBack(object value, System.Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
           return DependencyProperty.UnsetValue;
        }
    }
}
