namespace WordCounter.Services.FileConverter
{
    public interface IFileContentConverter
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="formFile"></param>
        /// <param name="cancelationToken"></param>
        /// <returns></returns>
        Task<string> ConvertToStringAsync(Stream formFile, CancellationToken cancelationToken = default);
    }
}
