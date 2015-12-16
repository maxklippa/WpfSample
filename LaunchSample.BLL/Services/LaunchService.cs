using System;
using System.Collections.Generic;
using System.Linq;
using LaunchSample.Core.Enumerations;
using LaunchSample.DAL.Repositories;
using LaunchSample.Domain.Models;

namespace LaunchSample.BLL.Services
{
    public class LaunchService
    {
        private LaunchRepository _launchRepository;

        public LaunchService()
        {
            _launchRepository = new LaunchRepository();
        }

        public IEnumerable<Launch> GetAll(string city = null, DateTime? from = null, DateTime? to = null, LaunchStatus? status = null)
        {
            return _launchRepository.All().Where(l => (city == null || l.City == city) 
                && (!from.HasValue || from.Value <= l.Month)
                && (!to.HasValue || l.Month <= to.Value)
                && (!status.HasValue || l.Status == status.Value));
        }

        public void Save(IEnumerable<Launch> launches)
        {
            var all = _launchRepository.All().ToList();

            foreach (var launch in all)
            {
                _launchRepository.Delete(launch.Id);
            }

            foreach (var launch in launches)
            {
                _launchRepository.Create(launch);
            }
        }
    }
}