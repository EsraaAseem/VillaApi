using System.Net;

namespace VillaApi.DataAccess.Helper
{
    public   class ApiResponse
    {
          
        public HttpStatusCode Status { get; set; }=HttpStatusCode.OK;
        public bool IsSuccess { get; set; } = true;
        public string Message { get; set; }
        public object Result { get; set; }
        public static ApiResponse ErrorException(HttpStatusCode status, string message)
        {
            var response = new ApiResponse();
            response.IsSuccess = false;
            response.Status = status;
            response.Message = message;
            return response;
        }
    }
}
