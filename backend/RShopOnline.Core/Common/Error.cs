namespace RShopAPI_Test.Core.Common;

public class Error
{
    public Error(string message)
    {
        Message = message;
        Code = ErrorCode.BadRequest;
    }

    public Error(string message, ErrorCode code)
    {
        Message = message;
        Code = code;
    }

    public Error(ErrorCode code)
    {
        Message = string.Empty;
        Code = code;
    }
    
    public string Message { get; }
    
    public ErrorCode Code { get; }
}