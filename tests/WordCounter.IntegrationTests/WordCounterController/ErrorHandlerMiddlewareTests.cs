using Newtonsoft.Json;
using System.Net;
using System.Text;
using WordCounter.IntegrationTests.Factory;
using WordCounter.Models.Error;

namespace WordCounter.IntegrationTests.WordCounterController
{
    [TestFixture]
    public class ErrorHandlerMiddlewareTests
    {
        private WebApiFactoryWithError factory;
        private HttpClient client;
        private string fileContent;
        private MultipartFormDataContent formData;

        private MemoryStream memoryStream;
        private StreamContent content;

        [OneTimeSetUp]
        public void Setup()
        {
            factory = new WebApiFactoryWithError();
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
        public async Task when_ErrorInside_then_Status500()
        {
            var resultPost = await client.PostAsync("/api/wordcount", formData);

            Assert.That(resultPost.StatusCode, Is.EqualTo(HttpStatusCode.InternalServerError));
        }

        [Test]
        public async Task when_ErrorInside_then_ErrorInformationResult()
        {
            var resultPost = await client.PostAsync("/api/wordcount", formData);

            var response = await resultPost.Content.ReadAsStringAsync();
            var errorResponse = JsonConvert.DeserializeObject<ErrorInformation>(response);

            Assert.That(errorResponse.Guid, !Is.EqualTo(Guid.Empty));
            Assert.That(errorResponse.Message, Is.EqualTo("Server error"));
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
            fileContent = null;
        }
    }
}