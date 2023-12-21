using WordCounter.Models;

namespace WordCounter.Services.Counter
{
    public interface ICounterService
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        IReadOnlyList<WordPosition> CountWords(string text);
    }
}
