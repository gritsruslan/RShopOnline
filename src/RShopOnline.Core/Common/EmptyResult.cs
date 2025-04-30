namespace RShopAPI_Test.Core.Common;

public class EmptyResult : Result
{
    public EmptyResult(Error error) : base(error)
    {
    }

    public EmptyResult() : base(SuccessCode.Ok)
    {
    }

    public EmptyResult(SuccessCode successCode) : base(successCode)
    {
    }

    public static EmptyResult Success() => new();
    
    public static implicit operator EmptyResult(Error error) => new(error);
}