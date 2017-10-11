using System.Net;

namespace EDCompanionAPI.Models
{
    /// <summary>
    /// Profile data response
    /// </summary>
    public class ProfileResponse
    {
        /// <summary>
        /// If user was not logged in this will return the login status
        /// </summary>
        public LoginStatus LoginStatus { get; set; }
        /// <summary>
        /// Returned http status code for request
        /// </summary>
        public HttpStatusCode HttpStatusCode { get; set; }
        /// <summary>
        /// Raw json data returned from api
        /// </summary>
        public string Json { get; set; } = Constants.RESPONSE_EMPTY;
        /// <summary>
        /// Raw json data returned from api
        /// </summary>
        public string PlainData { get; set; } 
        /// <summary>
        /// Json data is cached for 60 seconds. This indicates if returned data came from cache or not
        /// </summary>
        public bool Cached { get; set; }
    }
}
