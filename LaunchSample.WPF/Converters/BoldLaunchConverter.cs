using System;
using System.Globalization;
using System.Windows.Data;

namespace LaunchSample.WPF.Converters
{
	[ValueConversion(typeof(DateTime), typeof(bool))]
	public class BoldLaunchConverter : IValueConverter
	{
		private const int FRONTIER_DAY = 20;

		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			var date = System.Convert.ToDateTime(value);

			return FRONTIER_DAY < date.Day;
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			return null;
		}
	}
}