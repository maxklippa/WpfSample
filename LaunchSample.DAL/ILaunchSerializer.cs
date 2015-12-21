using System.Collections.Generic;
using LaunchSample.Domain.Models.Entities;

namespace LaunchSample.DAL
{
	public interface ILaunchSerializer
	{
		void Serialize(IEnumerable<Launch> launches);
		IEnumerable<Launch> Deserialize();
	}
}