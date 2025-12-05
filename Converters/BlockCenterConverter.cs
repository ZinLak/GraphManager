using System;
using System.Globalization;
using System.Windows.Data;

namespace GraphManager.Converters
{
    public class BlockCenterConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            // values[0] — это Координата (X или Y)
            // values[1] — это Размер (Width или Height)

            if (values.Length == 2 &&
                values[0] is double coord &&
                values[1] is double size)
            {
                // Формула центра
                return coord + (size / 2);
            }

            return 0.0;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}