﻿using System;
using System.Windows;
using LaunchSample.Domain.Mapping;
using LaunchSample.WPF.ViewModel;

namespace LaunchSample.WPF
{
	/// <summary>
	/// Interaction logic for App.xaml
	/// </summary>
	public partial class App : Application
	{
		protected override void OnStartup(StartupEventArgs e)
		{
			base.OnStartup(e);

			AutoMapperConfiguration.Configure();
			AutoMapperConfig.Configure();

			var window = new MainWindow();

			var viewModel = new MainWindowViewModel();

			window.DataContext = viewModel;

			window.Show();
		}
	}
}
