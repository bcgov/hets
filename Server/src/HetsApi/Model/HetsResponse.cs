using Microsoft.Extensions.Configuration;

namespace HetsApi.Model
{
    /// <summary>
    /// Standard Hets Response Model
    /// </summary>
    public sealed class HetsResponse
    {
        public HetsResponse(object responseData)
        {
            Data = responseData;
            ResponseStatus = "OK";
        }

        public HetsResponse(string error, string description)
        {
            Error = new ErrorViewModel(error, description);
            ResponseStatus = "ERROR";
        }

        /// <summary>
        /// Response Status (OK / ERROR)
        /// </summary>
        public string ResponseStatus { get; }

        public object Data { get; }

        public ErrorViewModel Error { get; }
    }

    /// <summary>
    /// Error View Model - used for business layer error messaging between Server and UI
    /// </summary>
    public sealed class ErrorViewModel
    {        
        /// <summary>
        /// Error View Model Constructor
        /// </summary>
        public ErrorViewModel()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ErrorViewModel" /> class.
        /// </summary>
        /// <param name="error">Error Number (required).</param>
        /// <param name="description">Description (required).</param>
        public ErrorViewModel(string error,  string description)
        {
            Error = error;
            Description = description;
        }

        /// <summary>
        /// Gets or Sets Number
        /// </summary>
        public string Error { get; set; }

        /// <summary>
        /// Gets or Sets Description
        /// </summary>
        public string Description { get; set; }

        public static string GetDescription(string error, IConfiguration configuration)
        {
            try
            {
                string temp = configuration.GetSection("Constants:ExceptionDescriptions:" + error).Value;

                if (!string.IsNullOrEmpty(temp))
                    return temp;
            }
            catch
            {
                // do nothing
            }

            return error;
        }
    }
}
