using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Razor3._1.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Razor3_1.Pages
{
	public class IndexModel : PageModel
	{
		private readonly ILogger<IndexModel> _logger;
		private readonly BloggingContext _dbContext;

		public int SessionCacheCount { get; set; }

		public int SeenCount { get; set; }

		public IndexModel(ILogger<IndexModel> logger, BloggingContext dbContext)
		{
			_logger = logger;
			_dbContext = dbContext;
		}

		public void OnGet()
		{
			SessionCacheCount = _dbContext.SessionCache.Count();

			string seen_value = HttpContext.Session.GetString("Seen");
			if (int.TryParse(seen_value, out var seen))
			{
				SeenCount = seen;
			}
			else
				seen = 0;

			_logger.LogInformation($"Seen => {seen}");
			seen++;

			HttpContext.Session.SetString("Seen", seen.ToString());
		}
	}
}
