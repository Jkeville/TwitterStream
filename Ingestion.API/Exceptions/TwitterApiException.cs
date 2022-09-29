namespace Ingestion.API.Exceptions
{
    public class TwitterApiException : Exception
    {
        public DateTimeOffset? XRateLimitResetDate { get; set; }

        public TwitterApiException(string message) : base(message) { }

        public TwitterApiException(string message, Exception innerException) : base(message, innerException) { }

        public TwitterApiException(string message, Exception innerException, int? xRateLimitReset) : this(message, innerException)
        {
            if (xRateLimitReset.HasValue)
            {
                XRateLimitResetDate = DateTimeOffset.FromUnixTimeSeconds(xRateLimitReset.Value);
            }
        }

        public TwitterApiException(string message, int? xRateLimitReset) : this(message, null, xRateLimitReset){ } 
    }
}