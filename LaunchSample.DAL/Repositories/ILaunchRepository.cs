using System.Linq;
using LaunchSample.Domain.Models;

namespace LaunchSample.DAL.Repositories
{
	public interface ILaunchRepository
	{
		IQueryable<Launch> All();
		void Create(Launch launch);
		Launch Read(int id);
		void Update(Launch launch);
		void Delete(int id);
	}
}