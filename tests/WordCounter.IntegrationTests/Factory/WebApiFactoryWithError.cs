using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using WordCounter.IntegrationTests.Services;
using WordCounter.Services.FileConverter;

namespace WordCounter.IntegrationTests.Factory
{
    internal class WebApiFactoryWithError : WebApplicationFactory<Program>
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureTestServices(services =>
            {
                var fileConverterToRemove = services.SingleOrDefault(d => d.ServiceType == typeof(IFileContentConverter));
                services.Remove(fileConverterToRemove);

                services.AddSingleton<IFileContentConverter, FileContentConverterError>();
            });
        }
    }
}
