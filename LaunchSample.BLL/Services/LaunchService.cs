using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using LaunchSample.BLL.EventArguments;
using LaunchSample.Core.Enumerations;
using LaunchSample.DAL.Repositories;
using LaunchSample.Domain.Models.Dtos;
using LaunchSample.Domain.Models.Entities;

namespace LaunchSample.BLL.Services
{
	public class LaunchService : ILaunchService
	{
		#region Private Fields

		private readonly LaunchRepository _launchRepository;

		#endregion // Private Fields

		#region Public Fields

		public event EventHandler<LaunchCreatedEventArgs> LaunchCreated;
		public event EventHandler<LaunchUpdatedEventArgs> LaunchUpdated;
		public event EventHandler<LaunchDeletedEventArgs> LaunchDeleted;

		#endregion // Public Fields

		#region Constructor

		public LaunchService()
		{
			_launchRepository = new LaunchRepository();
		}

		#endregion // Constructor

		#region Public Interface

		public IEnumerable<LaunchDto> All()
		{
			return Mapper.Map<IQueryable<Launch>, IEnumerable<LaunchDto>>(_launchRepository.All());
		}

		public void Create(LaunchDto launch)
		{
			var launchEntity = _launchRepository.Create(Mapper.Map<LaunchDto, Launch>(launch));

			if (LaunchCreated != null)
			{
				LaunchCreated(this, new LaunchCreatedEventArgs(Mapper.Map<Launch, LaunchDto>(launchEntity)));
			}
		}

		public Launch Read(int id)
		{
			return _launchRepository.Read(id);
		}

		public void Update(LaunchDto launch)
		{
			_launchRepository.Update(Mapper.Map<LaunchDto, Launch>(launch));

			if (LaunchUpdated != null)
			{
				LaunchUpdated(this, new LaunchUpdatedEventArgs(launch));
			}
		}

		public void Delete(int id)
		{
			_launchRepository.Delete(id);

			if (LaunchDeleted != null)
			{
				LaunchDeleted(this, new LaunchDeletedEventArgs(id));
			}
		}

		public bool IsAlreadyExists(int id)
		{
			return _launchRepository.All().Any(l => l.Id == id);
		}

		public IEnumerable<LaunchDto> GetAll(string city = null, DateTime? from = null, DateTime? to = null, LaunchStatus? status = null)
		{
			var launches = _launchRepository.All().Where(l => (city == null || l.City == city)
			                                          && (!from.HasValue || from.Value <= l.Month)
			                                          && (!to.HasValue || l.Month <= to.Value)
			                                          && (!status.HasValue || l.Status == status.Value));

			return Mapper.Map<IQueryable<Launch>, IEnumerable<LaunchDto>>(launches); 
		}

		public void SaveAll(IEnumerable<LaunchDto> launches)
		{
			List<Launch> all = _launchRepository.All().ToList();

			foreach (Launch launch in all)
			{
				_launchRepository.Delete(launch.Id);
			}

			foreach (var launch in launches)
			{
				Create(launch);
			}
		}

		#endregion // Public Interface
	}
}