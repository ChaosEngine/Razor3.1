using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Razor3._1.Model;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;

namespace Razor3_1
{
	public class Startup
	{
		public Startup(IConfiguration configuration)
		{
			Configuration = configuration;
		}

		public IConfiguration Configuration { get; }

		// This method gets called by the runtime. Use this method to add services to the container.
		public void ConfigureServices(IServiceCollection services)
		{
			string conn_str = Configuration.GetConnectionString("MySQL");

			services.AddDbContextPool<BloggingContext>(options =>
			{
				options.UseMySql(conn_str, MySqlServerVersion.LatestSupportedServerVersion);
			});

			//services.AddDistributedMySqlCache(opts =>
			//{
			//	opts.ConnectionString = conn_str;

			//	var builder = new DbConnectionStringBuilder();
			//	builder.ConnectionString = conn_str;
			//	opts.SchemaName = builder["database"] as string;

			//	opts.TableName = nameof(SessionCache);
			//	opts.ExpiredItemsDeletionInterval = TimeSpan.FromMinutes(30);
			//});

			services.AddRazorPages()
				.AddSessionStateTempDataProvider();
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
		{
			using (var scope = app.ApplicationServices.CreateScope())
			using (var db_ctx = scope.ServiceProvider.GetService<BloggingContext>())
				db_ctx.Database.EnsureCreated();


			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
			}
			else
			{
				app.UseExceptionHandler("/Error");
				// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
				app.UseHsts();
			}

			app.UseHttpsRedirection();
			app.UseStaticFiles();

			app.UseRouting();

			app.UseAuthorization();

			app.UseEndpoints(endpoints =>
			{
				endpoints.MapRazorPages();
			});
		}
	}
}
