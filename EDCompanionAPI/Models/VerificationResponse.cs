using System.Net;

namespace EDCompanionAPI.Models
{
    /// <summary>
    /// Verification response
    /// </summary>
    public class VerificationResponse
    {
        /// <summary>
        /// Returned http status code for request
        /// </summary>
        public HttpStatusCode HttpStatusCode { get; set; }
        /// <summary>
        /// Returns true if verification was successfull
        /// </summary>
        public bool Success { get; set; }
    }
}
