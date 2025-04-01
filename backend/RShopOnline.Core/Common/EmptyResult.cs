namespace RShopAPI_Test.Core.Common;

public class EmptyResult
{
    private EmptyResult()
    {
        _error = null;
        IsSuccess = true;
    }

    private EmptyResult(Error error)
    {
        _error = error;
        IsSuccess = false;
    }

    private readonly Error? _error;
    
    public Error Error => IsFailure ? 
        _error! : throw new InvalidOperationException();

    public string ErrorMessage => IsFailure ? _error!.Message
        : throw new InvalidOperationException();
    public bool IsSuccess { get; }

    public bool IsFailure => !IsSuccess;

    public static EmptyResult Success() => new();

    public static implicit operator EmptyResult(Error error) => new(error);
}