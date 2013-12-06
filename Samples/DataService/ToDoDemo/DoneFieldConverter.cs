using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Examples.MessagingService.ToDoDemo
{
  public class DoneFieldConverter : IValueConverter
  {
    object IValueConverter.Convert( object value, Type targetType, object parameter, CultureInfo culture )
    {
      return ((bool) value) ? Visibility.Visible : Visibility.Collapsed;
    }

    object IValueConverter.ConvertBack( object value, Type targetType, object parameter, CultureInfo culture )
    {
      return (Visibility) value == Visibility.Visible;
    }
  }
}