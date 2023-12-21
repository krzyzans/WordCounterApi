using WordCounter.Services.FileConverter;

namespace WordCounter.IntegrationTests.Services
{
    public class FileContentConverterError : IFileContentConverter
    {
        public Task<string> ConvertToStringAsync(Stream formFile, CancellationToken cancelationToken = default)
        {
            throw new Exception("Exception occured!!!");
        }
    }
}
