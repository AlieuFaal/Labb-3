using System.Globalization;
using System.Windows.Data;
using System.Windows.Media.Imaging;

namespace Labb_3.Converters
{
    public class AnswerStateToImageConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var state = value as string;
            if (state == "Correct")
            {
                return new BitmapImage(new Uri("pack://application:,,,/Assets/correct.png"));
            }
            else if (state == "Incorrect")
            {
                return new BitmapImage(new Uri("pack://application:,,,/Assets/incorrect.png"));
            }
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
