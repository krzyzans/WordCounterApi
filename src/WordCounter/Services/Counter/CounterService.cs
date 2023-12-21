using System.Collections.Immutable;
using System.Text.RegularExpressions;
using WordCounter.Models;

namespace WordCounter.Services.Counter
{
    public class CounterService : ICounterService
    {
        private readonly ILogger<CounterService> logger;

        public CounterService(ILogger<CounterService> logger)
        {
            this.logger = logger;
        }

        /// <summary>
        /// Method count words. Uses regex pattern.
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public IReadOnlyList<WordPosition> CountWords(string text)
        {
            //alpha numeric characters surrounded by white-space
            var wordFindPattern = new Regex(@"(?<=[\s])(\w+)(?=[\s])", RegexOptions.Multiline);

            var words = wordFindPattern.Matches(text)
                .Select(match => match.Value.Trim());

            var wordCounts = words.GroupBy(word => word.ToLower())
                .OrderByDescending(group => group.Count())
                .Select(group => new WordPosition(group.Key, group.Count()))
                .ToImmutableList();

            return wordCounts;
        }
    }
}
