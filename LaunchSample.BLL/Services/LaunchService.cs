using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using LaunchSample.Core.Enumerations;
using LaunchSample.DAL.Repositories;
using LaunchSample.Domain.Models.Dtos;
using LaunchSample.Domain.Models.Entities;

namespace LaunchSample.BLL.Services
{
	public class LaunchService : ILaunchService
	{
		private readonly LaunchRepository _launchRepository;

		public LaunchService()
		{
			_launchRepository = new LaunchRepository();
		}

		public IEnumerable<LaunchDto> All()
		{
			return Mapper.Map<IQueryable<Launch>, IEnumerable<LaunchDto>>(_launchRepository.All());
		}

		public void Create(LaunchDto launch)
		{
			_launchRepository.Create(Mapper.Map<LaunchDto, Launch>(launch));
		}

		public Launch Read(int id)
		{
			return _launchRepository.Read(id);
		}

		public void Update(LaunchDto launch)
		{
			_launchRepository.Update(Mapper.Map<LaunchDto, Launch>(launch));
		}

		public void Delete(int id)
		{
			_launchRepository.Delete(id);
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
	}
}