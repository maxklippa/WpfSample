using AutoMapper;
using LaunchSample.Domain.Extensions;
using LaunchSample.Domain.Models.Dtos;
using LaunchSample.Domain.Models.Entities;

namespace LaunchSample.Domain.Mapping
{
	public class AutoMapperConfiguration
	{
		public static void Configure()
		{
			ConfigureMapping();
		}

		private static void ConfigureMapping()
		{
			// Entity to DTO 
			Mapper.CreateMap<Launch, LaunchDto>().IgnoreAllNonExisting();

			// DTO to Entity
			Mapper.CreateMap<LaunchDto, Launch>().IgnoreAllNonExisting();
		}
	}
}