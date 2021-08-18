using bReader.Shared.Services;
using bReader.Wasm.Services;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using BlazorFluentUI;
using Blazored.LocalStorage;
namespace bReader.Wasm
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<bReader.Shared.App>("#app");

            builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress)});
            builder.Services.AddScoped<IFeedService, FeedDataService>()
                            .AddScoped<CommonJsInterop>();

            builder.Services.AddBlazoredLocalStorage().AddBlazorFluentUI();
            builder.Services.AddAutoMapper(typeof(bReader.Shared.MapperProfile));
            await builder.Build().RunAsync();
        }
    }
}
