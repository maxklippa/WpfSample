using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using LaunchSample.Domain.Models.Entities;

namespace LaunchSample.DAL.Repositories.LaunchRepository
{
	public class DbLaunchRepository : ILaunchRepository
	{
		#region Private fields

		private readonly LaunchSampleDbContext _context;

		#endregion // Private fields

		#region Constructor

		public DbLaunchRepository()
		{
			_context = new LaunchSampleDbContext();
		}

		#endregion

		#region ILaunchRepository Members

		public IQueryable<Launch> All()
		{
			return _context.Launches;
		}

		public Launch Create(Launch launch)
		{
			DbEntityEntry dbEntityEntry = _context.Entry(launch);
			if (dbEntityEntry.State != EntityState.Detached)
			{
				dbEntityEntry.State = EntityState.Added;
			}
			else
			{
				launch = _context.Launches.Add(launch);
			}
			_context.SaveChanges();
			return launch;
		}

		public Launch Find(int id)
		{
			return _context.Launches.FirstOrDefault(l => l.Id == id);
		}

		public void Update(Launch launch)
		{
			var dbEntityEntry = _context.Entry(launch);
			if (dbEntityEntry.State == EntityState.Detached)
			{
				_context.Launches.Attach(launch);
			}
			dbEntityEntry.State = EntityState.Modified;
			_context.SaveChanges();
		}

		public void Delete(int id)
		{
			var entity = _context.Launches.FirstOrDefault(l => l.Id == id);

			DbEntityEntry dbEntityEntry = _context.Entry(entity);

			if (entity == null)
			{
				return;
			}

			if (dbEntityEntry.State != EntityState.Deleted)
			{
				dbEntityEntry.State = EntityState.Deleted;
			}
			else
			{
				_context.Launches.Attach(entity);
				_context.Launches.Remove(entity);
			}

			_context.SaveChanges();
		}

		#endregion // ILaunchRepository Members
	}
}