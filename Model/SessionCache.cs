using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Razor3._1.Model
{
	/// <summary>
	/// Mapping of Sql Servre distributed cache:
	/// https://docs.microsoft.com/en-us/aspnet/core/performance/caching/distributed
	/// </summary>
	public partial class SessionCache
	{
		public string Id { get; set; }
		public byte[] Value { get; set; }
		public DateTimeOffset ExpiresAtTime { get; set; }
		public long? SlidingExpirationInSeconds { get; set; }
		public DateTimeOffset? AbsoluteExpiration { get; set; }
	}
}
