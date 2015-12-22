using System;
using System.CodeDom;
using System.Globalization;
using System.Windows.Data;
using LaunchSample.Core.Enumerations;

namespace LaunchSample.WPF.Converters
{
	[ValueConversion(typeof(LaunchStatus), typeof(bool))]
	public class SuccessLaunchConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			var status = (LaunchStatus) value;

			return status == LaunchStatus.Success;
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			return null; 
		}
	}
}