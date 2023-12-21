namespace WordCounter.Services.FileConverter
{
    public class FileContentConverter : IFileContentConverter
    {
        private readonly ILogger<FileContentConverter> logger;

        public FileContentConverter(ILogger<FileContentConverter> logger)
        {
            this.logger = logger;
        }

        /// <summary>
        /// Read data from stream (file)
        /// </summary>
        /// <param name="streamFile"></param>
        /// <param name="cancelationToken"></param>
        /// <returns>string from stream</returns>
        public async Task<string> ConvertToStringAsync(Stream streamFile, CancellationToken cancelationToken = default)
        {
            using StreamReader streamReader = new StreamReader(streamFile);

            return await streamReader.ReadToEndAsync();
        }
    }
}
