namespace SharpyProxy.Models;

public class ApiResponse
{
    public bool Success { get; set; }
    
    public string? Message { get; set; }

    public ApiResponse(bool success = true, string? message = null)
    {
        Success = success;
        Message = message;
    }
}

public class ApiResponse<T> : ApiResponse
{
    public T? Data { get; set; }

    public ApiResponse(T? data, bool success = true, string? message = null) : base(success, message)
    {
        Data = data;
    }
}