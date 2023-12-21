using Microsoft.Extensions.Logging.Abstractions;
using WordCounter.Services.Counter;

namespace WordCounter.UnitTests.CounterServiceTests
{
    public class CounterServiceTests
    {
        private CounterService counterService;
        private string textTest = string.Empty;

        [SetUp]
        public void Setup()
        {
            counterService = new CounterService(NullLogger<CounterService>.Instance);
            textTest = File.ReadAllText(@"TestFile/ExampleFile.txt");
        }

        [Test]
        public void when_EmptyText_then_NoEException()
        {
            Assert.DoesNotThrow(() => counterService.CountWords(string.Empty));
        }

        [TestCase(72)]
        public void when_CorrectText_then_ReturnsElements(int number)
        {
            var wordsOccurences = counterService.CountWords(textTest);

            Assert.That(wordsOccurences.Count, Is.EqualTo(number));
        }

        [Test]
        public void when_NoSpaceText_then_ReturnsElements()
        {
            var wordsOccurences = counterService.CountWords(textTest.Replace(" ",""));

            Assert.That(wordsOccurences.Count, Is.EqualTo(0));
        }

        [TearDown]
        public void TearDown()
        {
            counterService = null;
        }
    }
}