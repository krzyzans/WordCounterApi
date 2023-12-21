namespace WordCounter.Models.Error
{
    public class ErrorInformation
    {
        /// <summary>
        /// Message
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// Error guid.
        /// </summary>
        public Guid Guid { get; set; }
    }
}
