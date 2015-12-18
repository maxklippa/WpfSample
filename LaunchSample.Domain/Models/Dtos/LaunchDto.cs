using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using LaunchSample.Core.Enumerations;

namespace LaunchSample.Domain.Models.Dtos
{
	public class LaunchDto : IDataErrorInfo
	{
		#region Constructor

		public LaunchDto()
		{
			StartDateTime = DateTime.Now;
			EndDateTime = DateTime.Now;
			Month = DateTime.Now;
		}

		#endregion // Constructor

		#region State Properties

		public int Id { get; set; }
		public string City { get; set; }
		public DateTime StartDateTime { get; set; }
		public DateTime EndDateTime { get; set; }
		public DateTime Month { get; set; }
		public LaunchStatus Status { get; set; }

		#endregion // State Properties

		#region IDataErrorInfo Members

		string IDataErrorInfo.this[string propertyName]
		{
			get { return GetValidationError(propertyName); }
		}

		string IDataErrorInfo.Error { get { return null; } }

		#endregion IDataErrorInfo Members

		#region Validation

		public bool IsValid
		{
			get
			{
				return ValidatedProperties.All(property => GetValidationError(property) == null);
			}
		}

		static readonly string[] ValidatedProperties = 
		{ 
			"City", 
			"StartDateTime", 
			"EndDateTime"
		};

		private string GetValidationError(string propertyName)
		{
			if (Array.IndexOf(ValidatedProperties, propertyName) < 0)
			{
				return null;
			}

			string error = null;

			switch (propertyName)
			{
				case "City":
					error = ValidateCity();
					break;

				case "StartDateTime":
					error = ValidateStartDateTime();
					break;

				case "EndDateTime":
					error = ValidateEndDateTime();
					break;

				default:
					Debug.Fail("Unexpected property being validated on Launch: " + propertyName);
					break;
			}

			return error;
		}

		private string ValidateEndDateTime()
		{
			return EndDateTime < StartDateTime
				? "Start time can't be greater than the end time"
				: null;
		}

		private string ValidateStartDateTime()
		{
			return EndDateTime < StartDateTime
				? "End time can't be less than the start time"
				: null;
		}

		private string ValidateCity()
		{
			return string.IsNullOrWhiteSpace(City) ? "City is missing" : null;
		}
		
		#endregion // Validation
	}
}