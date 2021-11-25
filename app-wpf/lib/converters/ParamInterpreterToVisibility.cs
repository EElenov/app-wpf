using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

using app_domain;

namespace app_wpf.Converters
{
    public class ParamInterpreterToVisibility : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (ParameterType)value switch
            {
                ParameterType.IsInteger => (ParameterType)parameter == ParameterType.IsInteger ? Visibility.Visible : Visibility.Collapsed,
                ParameterType.IsString => (ParameterType)parameter == ParameterType.IsString ? Visibility.Visible : Visibility.Collapsed,
                ParameterType.IsBoolean => (ParameterType)parameter == ParameterType.IsBoolean ? Visibility.Visible : Visibility.Collapsed,
                ParameterType.IsCollection => (ParameterType)parameter == ParameterType.IsCollection ? Visibility.Visible : Visibility.Collapsed,
                _ => Visibility.Collapsed
            };
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return Binding.DoNothing;
        }
    }
}
