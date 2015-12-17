using System.Linq;
using LaunchSample.Core.Enumerations;
using LaunchSample.Domain.Models;
using System.Configuration;
using LaunchSample.Domain.Models.Entities;

namespace LaunchSample.DAL.Repositories
{
    public class LaunchRepository : ILaunchRepository
    {
	    private readonly ILaunchRepository _launchRepository;

        public LaunchRepository()
        {
            var dataSourceType = ConfigurationManager.AppSettings["DataSourceType"];
            
            if (dataSourceType == DataSourceType.Xml.ToString())
            {
                _launchRepository = XmlLaunchRepository.Instance;
            }
            else if (dataSourceType == DataSourceType.Database.ToString())
            {
                _launchRepository = new DbLaunchRepository();
            }
            else
            {
				throw new ConfigurationErrorsException();
            }
        }

        public IQueryable<Launch> All()
        {
	        return _launchRepository.All();
        }

        public Launch Create(Launch launch)
        {
            return _launchRepository.Create(launch);
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
    }
}