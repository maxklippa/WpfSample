using AutoMapper;
using LaunchSample.Domain.Extensions;
using LaunchSample.Domain.Models.Dtos;
using LaunchSample.Domain.Models.Entities;
using LaunchSample.WPF.ViewModel;

namespace LaunchSample.WPF
{
	public class AutoMapperConfig
	{
		public static void Configure()
		{
			ConfigureMapping();
		}

		private static void ConfigureMapping()
		{
			// DTO to ViewModel 
			Mapper.CreateMap<LaunchDto, LaunchViewModel>().IgnoreAllNonExisting();

			// ViewModel to DTO
			Mapper.CreateMap<LaunchViewModel, LaunchDto>().IgnoreAllNonExisting();
		} 
	}
}