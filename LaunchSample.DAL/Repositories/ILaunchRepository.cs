using System.Linq;
using LaunchSample.Domain.Models;
using LaunchSample.Domain.Models.Entities;

namespace LaunchSample.DAL.Repositories
{
	public interface ILaunchRepository
	{
		IQueryable<Launch> All();
		Launch Create(Launch launch);
		Launch Read(int id);
		void Update(Launch launch);
		void Delete(int id);
	}
}