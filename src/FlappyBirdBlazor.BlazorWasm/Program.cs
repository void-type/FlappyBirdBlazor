using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

namespace FlappyBirdBlazor.BlazorWasm
{
    public static class Program
    {
        public static Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("#app");

            builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

            return builder.Build().RunAsync();
        }
    }
}
