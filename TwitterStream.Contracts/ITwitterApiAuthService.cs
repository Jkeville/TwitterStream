namespace TwitterStream.Contracts
{
    public interface ITwitterApiAuthService
    {
        /// <summary>
        /// gets the OAuth2 token to use when calling Twitter API methods
        /// </summary>
        /// <returns></returns>
        Task<string> GetOAuth2TokenAsync();
    }
}