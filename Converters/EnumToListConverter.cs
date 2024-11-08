using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace Labb_3.Converters
{
    public class EnumToListConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null) return null;

            Type enumType = value.GetType();

            if (!enumType.IsEnum)
            {
                MessageBox.Show("Value must be an enum!");
            }

            return Enum.GetValues(enumType).Cast<Enum>().ToList();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
