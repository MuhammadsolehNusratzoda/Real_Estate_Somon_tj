namespace RealEstate.Domain.Common;

public class GenericResponse<T>
{
    public bool IsSuccess { get; set; }
    public string Message { get; set; } = string.Empty;
    public T? Data { get; set; }
    public List<string>? Errors { get; set; }
    public int StatusCode { get; set; }

    public static GenericResponse<T> Success(T data, string message = "Success", int statusCode = 200) =>
        new() { IsSuccess = true, Data = data, Message = message, StatusCode = statusCode };

    public static GenericResponse<T> Failure(string message, List<string>? errors = null, int statusCode = 400) =>
        new() { IsSuccess = false, Message = message, Errors = errors, StatusCode = statusCode };

    public static GenericResponse<T> NotFound(string message = "Not found") =>
        new() { IsSuccess = false, Message = message, StatusCode = 404 };

    public static GenericResponse<T> Unauthorized(string message = "Unauthorized") =>
        new() { IsSuccess = false, Message = message, StatusCode = 401 };
}
