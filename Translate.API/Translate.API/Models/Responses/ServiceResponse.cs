namespace Translation.API.Models.Responses
{
    public class ServiceResponse<T> where T : class
    {
        public T? Data { get; set; }
        public bool Success { get; set; } = true;
        public string Message { get; set; } = string.Empty;
    }
}
