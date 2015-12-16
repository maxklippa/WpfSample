using System;
using System.Collections.Generic;
using LaunchSample.Core.Enumerations;
using LaunchSample.Domain.Models;

namespace LaunchSample.BLL.Services
{
	public interface ILaunchService
	{
		IEnumerable<Launch> All();
		void Create(Launch launch);
		Launch Read(int id);
		void Update(Launch launch);
		void Delete(int id);

		IEnumerable<Launch> GetAll(string city = null, DateTime? from = null, DateTime? to = null, LaunchStatus? status = null);
		void SaveAll(IEnumerable<Launch> launches);
	}
}