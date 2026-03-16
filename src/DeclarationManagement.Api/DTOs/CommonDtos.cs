namespace DeclarationManagement.Api.DTOs;

/// <summary>
/// 统一 API 响应结构。
/// </summary>
public class ApiResponse<T>
{
    /// <summary>
    /// Success 属性。
    /// </summary>
    public bool Success { get; set; }
    /// <summary>
    /// Message 属性。
    /// </summary>
    public string Message { get; set; } = string.Empty;
    /// <summary>
    /// Data 属性。
    /// </summary>
    public T? Data { get; set; }

    /// <summary>
    /// Ok 方法。
    /// </summary>
    public static ApiResponse<T> Ok(T data, string message = "操作成功") =>
        new() { Success = true, Data = data, Message = message };

    /// <summary>
    /// Fail 方法。
    /// </summary>
    public static ApiResponse<T> Fail(string message) =>
        new() { Success = false, Message = message };
}
