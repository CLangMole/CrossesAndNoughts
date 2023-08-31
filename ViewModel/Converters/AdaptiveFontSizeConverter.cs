using System;
using System.Globalization;
using System.Windows.Data;

namespace CrossesAndNoughts.ViewModel.Converters;

class AdaptiveFontSizeConverter : IMultiValueConverter
{
    public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
    {
        if (values.Length == 1 && values[0] is double actualHeight)
        {
            return actualHeight;
        }

        return Binding.DoNothing;
    }

    public object[] ConvertBack(object values, Type[] targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
