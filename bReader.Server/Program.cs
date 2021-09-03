using bReader.Server.Data;
using bReader.Shared;
using bReader.Shared.Services;
using bReader.Shared.Utils;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace bReader.Server
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();
            using (var scope = host.Services.CreateScope())
            {
                using var db = scope.ServiceProvider.GetRequiredService<IDbContextFactory<FeedDbContext>>().CreateDbContext();
                db.Database.Migrate();
                var set = scope.ServiceProvider.GetRequiredService<ISettingService>();
                InitSettings(set);
            }
            host.Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });

        protected static void InitSettings(ISettingService settingService)
        {
            Dictionary<string, string> dic = settingService.GetAllSettingsAsync().Result;
            foreach (var field in typeof(SettingKeyMap).GetFields())
            {
                dic.AddIfNone((string)field.GetValue(null), field.GetCustomAttribute<DefaultValueAttribute>()?.Value??"");
            }

            settingService.SaveSettingsAsync(dic);
        }
    }
}
