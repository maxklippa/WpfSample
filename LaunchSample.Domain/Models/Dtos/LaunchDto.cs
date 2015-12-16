using System;
using LaunchSample.Core.Enumerations;

namespace LaunchSample.Domain.Models.Dtos
{
	public class LaunchDto
	{
		public int Id { get; set; }
		public string City { get; set; }
		public DateTime StartDateTime { get; set; }
		public DateTime EndDateTime { get; set; }
		public DateTime Month { get; set; }
		public LaunchStatus Status { get; set; } 
	}
}