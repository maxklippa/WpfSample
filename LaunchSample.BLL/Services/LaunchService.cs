using System;
using System.Collections.Generic;
using System.Linq;
using LaunchSample.Core.Enumerations;
using LaunchSample.DAL.Repositories;
using LaunchSample.Domain.Models;

namespace LaunchSample.BLL.Services
{
	public class LaunchService : ILaunchService
	{
		private readonly LaunchRepository _launchRepository;

		public LaunchService()
		{
			_launchRepository = new LaunchRepository();
		}

		public IEnumerable<Launch> All()
		{
			return _launchRepository.All();
		}

		public void Create(Launch launch)
		{
			_launchRepository.Create(launch);
		}

		public Launch Read(int id)
		{
			return _launchRepository.Read(id);
		}

		public void Update(Launch launch)
		{
			_launchRepository.Update(launch);
		}

		public void Delete(int id)
		{
			_launchRepository.Delete(id);
		}

		public IEnumerable<Launch> GetAll(string city = null, DateTime? from = null, DateTime? to = null, LaunchStatus? status = null)
		{
			return _launchRepository.All().Where(l => (city == null || l.City == city)
			                                          && (!from.HasValue || from.Value <= l.Month)
			                                          && (!to.HasValue || l.Month <= to.Value)
			                                          && (!status.HasValue || l.Status == status.Value));
		}

		public void SaveAll(IEnumerable<Launch> launches)
		{
			List<Launch> all = _launchRepository.All().ToList();

			foreach (Launch launch in all)
			{
				_launchRepository.Delete(launch.Id);
			}

			foreach (Launch launch in launches)
			{
				_launchRepository.Create(launch);
			}
		}
	}
}