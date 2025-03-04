using System.Globalization;
using System.Windows.Data;

namespace Exchange.Presentation.Converters
{
    public class StringToIntConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is not int integer)
                return string.Empty;

            return integer.ToString();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is not string str)
                return 0;

            bool result = int.TryParse(str, out int integer);

            if (result == false)
                return 0;

            return integer;
        }
    }
}
