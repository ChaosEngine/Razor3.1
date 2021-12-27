using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
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
		#region Main

		public static void Main(string[] args)
		{
			CreateHostBuilder(args).Build().Run();
		}

		public static IHostBuilder CreateHostBuilder(string[] args) =>
			Host.CreateDefaultBuilder(args)
				.ConfigureWebHostDefaults(webBuilder =>
				{
					webBuilder.UseStartup<Startup>();
				});

		#endregion Main

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

			///
			/// https://docs.microsoft.com/en-us/aspnet/core/fundamentals/app-state?view=aspnetcore-6.0#session-state
			///
			services.AddDistributedMySqlCache(options =>
			{
				options.ConnectionString = conn_str;

				var builder = new DbConnectionStringBuilder();
				builder.ConnectionString = conn_str;
				options.SchemaName = builder["database"] as string;

				options.TableName = nameof(SessionCache);
				options.ExpiredItemsDeletionInterval = TimeSpan.FromMinutes(30);
			});

			///
			/// https://docs.microsoft.com/en-us/aspnet/core/fundamentals/app-state?view=aspnetcore-6.0#configure-session-state
			/// 
			services.AddSession(options =>
			{
				// Set a short timeout for easy testing.
				options.IdleTimeout = TimeSpan.FromMinutes(60);
				options.Cookie.Path = "/";
				options.Cookie.HttpOnly = true;
				options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
				options.Cookie.SameSite = SameSiteMode.Strict;
				options.Cookie.IsEssential = true;
			});

			services.AddRazorPages()
				.AddSessionStateTempDataProvider();
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
		{
			#region Ensure DB is created

			using (var scope = app.ApplicationServices.CreateScope())
			using (var db_ctx = scope.ServiceProvider.GetService<BloggingContext>())
				db_ctx.Database.EnsureCreated();

			#endregion Ensure DB is created


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
			app.UseSession();
			app.UseAuthorization();
			app.UseSession();

			app.UseEndpoints(endpoints =>
			{
				endpoints.MapRazorPages();
			});
		}
	}
}
