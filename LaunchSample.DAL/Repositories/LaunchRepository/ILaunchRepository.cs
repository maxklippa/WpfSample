using System.Linq;
using LaunchSample.Domain.Models.Entities;

namespace LaunchSample.DAL.Repositories.LaunchRepository
{
	public interface ILaunchRepository
	{
		IQueryable<Launch> All();
		Launch Create(Launch launch);
		Launch Find(int id);
		void Update(Launch launch);
		void Delete(int id);
	}
}