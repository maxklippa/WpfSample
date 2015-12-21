using System.Configuration;
using System.IO;
using System.Linq;
using LaunchSample.Core.Enumerations;
using LaunchSample.Domain.Models.Entities;

namespace LaunchSample.DAL.Repositories.LaunchRepository
{
	public class LaunchRepositoryFactory
	{
		private readonly string _dataSourceType;

		public LaunchRepositoryFactory()
		{
			_dataSourceType = ConfigurationManager.AppSettings["DataSourceType"];
		}

		public ILaunchRepository CreateRepository()
		{
			if (_dataSourceType == DataSourceType.Xml.ToString())
			{
				var filename = ConfigurationManager.AppSettings["XmlFileName"];

				if (string.IsNullOrEmpty(filename))
				{
					throw new ConfigurationErrorsException();
				}

				if (!File.Exists(filename))
				{
					File.Create(filename);
				}

				return new XmlLaunchRepository(new LaunchSerializer(filename));
			}
			if (_dataSourceType == DataSourceType.Database.ToString())
			{
				return new DbLaunchRepository();
			}
			throw new ConfigurationErrorsException();
		} 
	}
}