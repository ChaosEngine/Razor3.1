using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Razor3._1.Model
{
	public partial class BloggingContext : DbContext
	{
		public virtual DbSet<SessionCache> SessionCache { get; set; }

		public BloggingContext(DbContextOptions<BloggingContext> options)
			: base(options)
		{
		}

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);
			// Customize the ASP.NET Identity model and override the defaults if needed.
			// For example, you can rename the ASP.NET Identity table names and more.
			// Add your customizations after calling base.OnModelCreating(builder);

			modelBuilder.Entity<SessionCache>(entity =>
			{
				entity.HasIndex(e => e.ExpiresAtTime)
					.HasDatabaseName("Index_ExpiresAtTime");

				entity.Property(e => e.Id).HasMaxLength(449);
				entity.Property(e => e.Value).IsRequired();
			});
		}
	}
}
