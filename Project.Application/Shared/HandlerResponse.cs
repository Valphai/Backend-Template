using Project.Application.DataTransferObjects;

namespace Project.Application.Shared;

public class HandlerResponse(string message, int status, UserResponseDTO? data = null)
{
    public string Message { get; set; } = message;
    public int Status { get; set; } = status;
    public UserResponseDTO? Data { get; set; } = data;
}