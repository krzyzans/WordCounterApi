using WordCounter.Services.Counter;
using WordCounter.Services.FileConverter;

namespace WordCounter.Extensions
{
    internal static class StartupExtensions
    {
        internal static IServiceCollection AddServices(this IServiceCollection services)
        {
            services.AddSingleton<IFileContentConverter, FileContentConverter>();
            services.AddSingleton<ICounterService, CounterService>();

            return services;
        }
    }
}
