using System.IO;
using System.Net;
using System.Text;
using System.Text.Json;
using WordCounter.IntegrationTests.Factory;
using WordCounter.Models;

namespace WordCounter.IntegrationTests.WordCounterController
{
    [TestFixture]
    public class WordCounterControllerTests
    {
        private WebApiFactory factory;
        private HttpClient client;
        private string fileContent;

        private MultipartFormDataContent formData;

        private MemoryStream memoryStream;
        private StreamContent content;


        [OneTimeSetUp]
        public void Setup()
        {
            factory = new WebApiFactory();
            client = factory.CreateDefaultClient();

            fileContent = File.ReadAllText(@"TestFile/ExampleFile.txt");
        }

        [SetUp]
        public void ContentPreparation()
        {
            memoryStream = new MemoryStream(Encoding.UTF8.GetBytes(fileContent));
            content = new StreamContent(memoryStream);
            formData = new MultipartFormDataContent();
            formData.Add(content, "formFile", "ExampleFile.txt");
        }

        [Test]
        public async Task when_CorrectFile_then_Status200()
        {
            var resultPost = await client.PostAsync("/api/wordcount", formData);

            Assert.That(resultPost.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        }

        [Test]
        public async Task when_EmptyFile_then_Status400()
        {
            using MemoryStream memoryStreamEmpty = new MemoryStream(Encoding.UTF8.GetBytes(""));
            using var contentEmpty = new StreamContent(memoryStreamEmpty);
            using var formDataEmpty = new MultipartFormDataContent();
            formDataEmpty.Add(contentEmpty, "formFile", "ExampleFile.txt");

            var resultPost = await client.PostAsync("/api/wordcount", formDataEmpty);

            Assert.That(resultPost.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
        }

        [TestCase(59)]
        public async Task when_CorrectFile_then_ReturnCorrectNumberOfElements(int numberOfElements)
        {
            var resultPost = await client.PostAsync("/api/wordcount", formData);

            var result = await resultPost.Content.ReadAsStringAsync();
            var deserializedObject = JsonSerializer.Deserialize<List<WordPosition>>(result);

            Assert.That(deserializedObject?.Count, Is.EqualTo(numberOfElements));
        }

        [Test]
        public async Task when_CorrectFile_then_ListIsOrdered()
        {
            var resultPost = await client.PostAsync("/api/wordcount", formData);

            var result = await resultPost.Content.ReadAsStringAsync();
            var deserializedObject = JsonSerializer.Deserialize<List<WordPosition>>(result);

            for (int i = 1; i < deserializedObject?.Count - 1; i++)
            {
                if (deserializedObject[i - 1].Occurrences < deserializedObject[i].Occurrences)
                {
                    Assert.Fail("List is not ordered ");
                    return;
                }
            }

            Assert.Pass();
        }

        [TearDown]
        public void ContentDestruction()
        {
            formData.Dispose();
            content.Dispose();
            memoryStream.Dispose();
        }

        [OneTimeTearDown]
        public void Destruct()
        {
            factory.Dispose();
            client.Dispose();
        }
    }
}