using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace ArticleWpfApp.Converters
{
    class LogonConv : IValueConverter
    {
        public object Convert(object value, System.Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            bool ReturnValue = false;
            if (value != null)
            {
                switch ((bool)value)
                {
                    case true: ReturnValue = false; break;
                    case false: ReturnValue = true; break;
                }
            }
            return ReturnValue;
        }

        public object ConvertBack(object value, System.Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            bool ReturnValue = false;

            switch ((bool)value)
            {
                case true: ReturnValue = false; break;
                case false: ReturnValue = true; break;
            }
            return ReturnValue;
        }
    }
}
