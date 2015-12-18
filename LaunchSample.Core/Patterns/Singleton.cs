namespace LaunchSample.Core.Patterns
{
	/// <summary>
	/// Singleton pattern class
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public class Singleton<T> where T : new()
	{
		/// <summary>
		/// Static field for Singleton pattern realization
		/// </summary>
		protected static T _Instance;

		/// <summary>
		/// Static method for Singleton pattern realization
		/// </summary>
		public static T Instance
		{
			get
			{
				if (_Instance == null)
				{
					_Instance = new T();
				}

				return _Instance;
			}
		}

		protected virtual void OnInit() { }

		/// <summary>
		/// Private constructor for Singleton pattern realization		
		/// </summary>
		protected Singleton() { /* No initialization needed so far */ OnInit(); }
	}
}