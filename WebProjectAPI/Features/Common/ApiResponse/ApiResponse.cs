namespace WebProjectAPI.Features.Common.ApiResponse
{
    public class ApiResponse<T>
    {
        public bool Success { get; set; }

        public string Message { get; set; }

        public T Data { get; set; }

        public int TotalRecords { get; set; }
    }
}
