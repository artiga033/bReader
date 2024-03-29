using bReader.Server.Data;
using bReader.Server.Services;
using bReader.Shared.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Fast.Components.FluentUI;
namespace bReader.Server
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddRazorPages();
            services.AddServerSideBlazor();
            services.AddDbContextFactory<FeedDbContext>(options =>
                options.UseSqlite(Configuration.GetConnectionString("bReader")));

            services.AddHttpClient();
            services.AddFluentUIComponents();

            services.AddAutoMapper(typeof(bReader.Shared.MapperProfile));
            services.AddScoped<IFeedService, FeedDataService>();
            services.AddSingleton<ISettingService, SettingService>();
            services.AddHostedService<RefreshBackgroundService>();
            services.AddScoped<CommonJsInterop>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
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

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapBlazorHub();
                endpoints.MapFallbackToPage("/_Host");
            });
        }
    }
}
