using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using LaunchSample.Core.Enumerations;
using LaunchSample.DAL.Repositories;
using LaunchSample.DAL.Repositories.LaunchRepository;
using LaunchSample.Domain.Models.Dtos;
using LaunchSample.Domain.Models.Entities;

namespace LaunchSample.BLL.Services.LaunchService

{
	public class LaunchService : ILaunchService
	{

		private ILaunchRepository _launchRepository;

		private ILaunchRepository LaunchRepository
		{
			get
			{
				if (_launchRepository != null)
				{
					return _launchRepository;
				}
				else
				{
					var launchRepositoryFactory = new LaunchRepositoryFactory();
					_launchRepository = launchRepositoryFactory.CreateRepository();
					return _launchRepository;
				}
			}
		}

		public event EventHandler<LaunchCreatedEventArgs> LaunchCreated;
		public event EventHandler<LaunchUpdatedEventArgs> LaunchUpdated;
		public event EventHandler<LaunchDeletedEventArgs> LaunchDeleted;

		#region ILaunchService Members

		public IEnumerable<LaunchDto> All()
		{
			return Mapper.Map<IQueryable<Launch>, IEnumerable<LaunchDto>>(LaunchRepository.All());
		}

		public void Create(LaunchDto launch)
		{
			var launchEntity = LaunchRepository.Create(Mapper.Map<LaunchDto, Launch>(launch));

			if (LaunchCreated != null)
			{
				LaunchCreated(this, new LaunchCreatedEventArgs(Mapper.Map<Launch, LaunchDto>(launchEntity)));
			}
		}

		public Launch Find(int id)
		{
			return LaunchRepository.Find(id);
		}

		public void Update(LaunchDto launch)
		{
			LaunchRepository.Update(Mapper.Map<LaunchDto, Launch>(launch));

			if (LaunchUpdated != null)
			{
				LaunchUpdated(this, new LaunchUpdatedEventArgs(launch));
			}
		}

		public void Delete(int id)
		{
			LaunchRepository.Delete(id);

			if (LaunchDeleted != null)
			{
				LaunchDeleted(this, new LaunchDeletedEventArgs(id));
			}
		}

		public bool IsAlreadyExists(int id)
		{
			return LaunchRepository.All().Any(l => l.Id == id);
		}

		public IEnumerable<LaunchDto> GetAll(string city = null,
		                                     DateTime? from = null,
		                                     DateTime? to = null,
		                                     LaunchStatus? status = null)
		{
			var launches = LaunchRepository.All()
			                                .Where(l => (city == null || l.City == city) &&
			                                            (!from.HasValue || from.Value <= l.Month) &&
			                                            (!to.HasValue || l.Month <= to.Value) &&
			                                            (!status.HasValue || l.Status == status.Value));

			return Mapper.Map<IQueryable<Launch>, IEnumerable<LaunchDto>>(launches); 
		}

		public void SaveAll(IEnumerable<LaunchDto> launches)
		{
			var all = LaunchRepository.All().ToList();

			foreach (var launch in all)
			{
				LaunchRepository.Delete(launch.Id);
			}

			foreach (var launch in launches)
			{
				Create(launch);
			}
		}

		#endregion // ILaunchService Members
	}
}