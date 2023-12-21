using Microsoft.AspNetCore.Mvc;
using System.Net;
using WordCounter.Models;
using WordCounter.Services.Counter;
using WordCounter.Services.FileConverter;

namespace WordCounter.Controllers
{
    [ApiController]
    [Route("api")]
    public class WordCounterController : ControllerBase
    {
        private readonly ICounterService counterService;
        private readonly IFileContentConverter fileConverter;
        private readonly ILogger<WordCounterController> logger;

        public WordCounterController(ILogger<WordCounterController> logger, ICounterService counterService, IFileContentConverter fileConverter)
        {
            this.counterService = counterService;
            this.fileConverter = fileConverter;
            this.logger = logger;
        }

        /// <summary>
        /// Post method accepts a IFormFile, which may have been converted into a byte array, or the data may have been retrieved from a body 
        /// </summary>
        /// <param name="formFile"></param>
        /// <param name="cancelationToken"></param>
        /// <returns>Lists of <see cref="WordPosition"/>.</returns>
        [HttpPost(template:"wordcount")]
        [Consumes("multipart/form-data")]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(IEnumerable<WordPosition>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult> WordCount(IFormFile formFile, CancellationToken cancelationToken = default)
        {
            if (formFile.Length == 0)
            {
                logger.LogWarning("File is empty.");

                return BadRequest("File is empty.");
            }

            var textFromFile = await fileConverter.ConvertToStringAsync(formFile.OpenReadStream(), cancelationToken);
            var wordCounts = counterService.CountWords(textFromFile);

            return Ok(wordCounts);
        }
    }
}
