using System.Text;
using Microsoft.Extensions.Logging.Abstractions;
using WordCounter.Services.FileConverter;

namespace WordCounter.UnitTests.FileContentConverterTests
{
    public class FileContentConverterTests
    {
        private FileContentConverter fileContentConverter;
        private string textTest = string.Empty;

        [SetUp]
        public void Setup()
        {
            fileContentConverter = new FileContentConverter(NullLogger<FileContentConverter>.Instance);
            textTest = File.ReadAllText(@"TestFile/ExampleFile.txt");
        }

        [Test]
        public async Task when_CorrectStreamWithData_then_Result()
        {
            await using MemoryStream stream = new MemoryStream(Encoding.UTF8.GetBytes(textTest));

            var result = await fileContentConverter.ConvertToStringAsync(stream);

            Assert.That(result, Is.EqualTo(textTest));
        }

        [Test]
        public async Task when_EmptyStreamWithData_then_NoException()
        {
            await using MemoryStream stream = new MemoryStream(Encoding.UTF8.GetBytes(String.Empty));

            Assert.DoesNotThrowAsync(() => fileContentConverter.ConvertToStringAsync(stream));
        }

        [TearDown]
        public void TearDown()
        {
            fileContentConverter = null;
        }
    }
}