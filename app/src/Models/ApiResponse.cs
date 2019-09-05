namespace Bittn.Api.Models
{
    public class ApiResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public object Data { get; set; }
        public ApiNavigation Navigation { get; set; }
    }
}