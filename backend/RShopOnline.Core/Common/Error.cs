namespace RShopAPI_Test.Core.Common;

public class Error(string message)
{
    public string Message { get; } = message;
}