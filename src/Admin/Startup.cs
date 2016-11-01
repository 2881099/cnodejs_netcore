using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Redis;
using NLog.Extensions.Logging;
using Swashbuckle.Swagger.Model;
using System.IO;
using Microsoft.AspNetCore.DataProtection;
using cnodejs.BLL;
using cnodejs.Model;

namespace cnodejs.Admin {
	public class Startup {
		public Startup(IHostingEnvironment env) {
			var builder = new ConfigurationBuilder()
				.SetBasePath(env.ContentRootPath)
				.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
				.AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true);

			if (env.IsDevelopment()) {
				// For more details on using the user secret store see http://go.microsoft.com/fwlink/?LinkID=532709
				//builder.AddUserSecrets();
			}

			builder.AddEnvironmentVariables();
			Configuration = builder.Build();
		}

		public IConfigurationRoot Configuration { get; }

		public void ConfigureServices(IServiceCollection services) {
			services.AddSingleton<IDistributedCache>(
				new RedisCache(new RedisCacheOptions {
					Configuration = IniHelper.LoadIni("../web.config")["connectionStrings"]["cnodejsRedisConnectionString"],
					InstanceName = "Session_cnodejs"
				})).AddSession();

			services.AddMvc();

			//services.Configure<Microsoft.AspNetCore.Server.Kestrel.KestrelServerOptions>(option => {
			//	option.UseHttps(new System.Security.Cryptography.X509Certificates.X509Certificate2());
			//});

			services.ConfigureSwaggerGen(options => {
				options.SingleApiVersion(new Info {
					Version = "v1",
					Title = "cnodejs API",
					Description = "cnodejs 项目webapi接口说明",
					TermsOfService = "None",
					Contact = new Contact { Name = "duoyi", Email = "", Url = "http://duoyi.com" },
					License = new License { Name = "duoyi", Url = "http://duoyi.com" }
				});
				options.IncludeXmlComments(AppContext.BaseDirectory + @"/Admin.xml");
			});
			services.AddSwaggerGen();
			services.AddSingleton<IConfigurationRoot>(Configuration);
		}

		public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory) {
			Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
			Console.OutputEncoding = Encoding.GetEncoding("GB2312");
			Console.InputEncoding = Encoding.GetEncoding("GB2312");

			// 以下写日志会严重影响吞吐量，高并发项目建议改成 redis 订阅发布形式
			loggerFactory.AddConsole(Configuration.GetSection("Logging"));
			loggerFactory.AddNLog().AddDebug();
			env.ConfigureNLog("nlog.config");

			if (env.IsDevelopment())
				app.UseDeveloperExceptionPage();

			cnodejs.DAL.SqlHelper.Instance.Log = loggerFactory.CreateLogger("cnodejs_DAL_sqlhelper");

			app.UseSession(new SessionOptions() { IdleTimeout = TimeSpan.FromMinutes(30) });
			app.UseMvc();
			app.UseDefaultFiles().UseStaticFiles(); //UseDefaultFiles 必须在 UseStaticFiles 之前调用

			//app.UseCors(builder => builder.WithOrigins("https://*").AllowAnyHeader());

			if (env.IsDevelopment())
				app.UseSwagger().UseSwaggerUi();
		}
	}
}
