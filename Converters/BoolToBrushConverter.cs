using System.Windows.Media;
using System.Globalization;
using System.Windows.Data;

namespace Labb_3.Converters
{
    public class BoolToBrushConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool isCorrect)
            {
                return isCorrect ? Brushes.LightGreen : Brushes.OrangeRed;
            }
            return Brushes.Transparent;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
