using System;
using System.Collections.Generic;
using LaunchSample.Core.Enumerations;
using LaunchSample.Domain.Models.Dtos;
using LaunchSample.Domain.Models.Entities;

namespace LaunchSample.BLL.Services.LaunchService
{
	public interface ILaunchService
	{
		IEnumerable<LaunchDto> All();
		void Create(LaunchDto launch);
		Launch Find(int id);
		void Update(LaunchDto launch);
		void Delete(int id);
		bool IsAlreadyExists(int id);
		IEnumerable<LaunchDto> GetAll(string city = null,
		                              DateTime? from = null,
		                              DateTime? to = null,
		                              LaunchStatus? status = null);
		void SaveAll(IEnumerable<LaunchDto> launches);
	}
}