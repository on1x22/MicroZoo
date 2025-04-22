namespace MicroZoo.Infrastructure.MassTransit
{
    /// <summary>
    /// List of status codes of responces
    /// </summary>
    public enum ErrorCodes
    {
        /// <summary>
        /// Everithing is good
        /// </summary>
        Ok200,

        /// <summary>
        /// Request has invalid data
        /// </summary>
        BadRequest400,

        /// <summary>
        /// User is not authorized
        /// </summary>
        Unauthorized401,

        /// <summary>
        /// The user does not have the necessary permissions
        /// </summary>
        Forbiden403,

        /// <summary>
        /// Requested resource does not exist
        /// </summary>
        NotFound404,

        /// <summary>
        /// An unexpected error occurred while processing request
        /// </summary>
        InternalServerError500
    }
}
