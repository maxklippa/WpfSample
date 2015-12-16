using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using LaunchSample.Domain.Models;
using LaunchSample.Domain.Models.Entities;

namespace LaunchSample.DAL.Repositories
{
	public class DbLaunchRepository : ILaunchRepository
	{
		private readonly LaunchSampleDbContext _context;

		public DbLaunchRepository()
		{
			_context = new LaunchSampleDbContext();
		}

		public IQueryable<Launch> All()
		{
			return _context.Launches;
		}

		public void Create(Launch launch)
		{
			DbEntityEntry dbEntityEntry = _context.Entry(launch);
			if (dbEntityEntry.State != EntityState.Detached)
			{
				dbEntityEntry.State = EntityState.Added;
			}
			else
			{
				_context.Launches.Add(launch);
			}
			_context.SaveChanges();
		}

		public Launch Read(int id)
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
	}
}