using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bReader.Shared.Services
{
    // This class provides an example of how JavaScript functionality can be wrapped
    // in a .NET class for easy consumption. The associated JavaScript module is
    // loaded on demand when first needed.
    //
    // This class can be registered as scoped DI service and then injected into Blazor
    // components for use.
    public class CommonJsInterop : IAsyncDisposable
    {
        private readonly Lazy<Task<IJSObjectReference>> moduleTask;

        public CommonJsInterop(IJSRuntime jsRuntime)
        {
            moduleTask = new(() => jsRuntime.InvokeAsync<IJSObjectReference>(
               "import", "./_content/bReader.Shared/commonJsInterop.js").AsTask());
        }

        public async ValueTask<string> Prompt(string message)
        {
            var module = await moduleTask.Value;
            return await module.InvokeAsync<string>("showPrompt", message);
        }
        public async Task Alert(string message)
        {
            var module = await moduleTask.Value;
            await module.InvokeVoidAsync("showAlert", message);
        }
        public async Task<string> GetCurrentTitle()
        {
            var module = await moduleTask.Value;
            return await module.InvokeAsync<string>("getCurrentTitle");
        }
        public async Task SetTitle(string text)
        {
            var module = await moduleTask.Value;
            await module.InvokeVoidAsync("setTitle", text);
        }

        public async ValueTask DisposeAsync()
        {
            if (moduleTask.IsValueCreated)
            {
                var module = await moduleTask.Value;
                await module.DisposeAsync();
            }
        }
    }
}
