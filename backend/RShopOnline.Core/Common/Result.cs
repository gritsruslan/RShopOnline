namespace RShopAPI_Test.Core.Common;

public class Result<TValue>
{
    private Result(TValue value)
    {
        _value = value;
        IsSuccess = true;
        _error = null;
    }

    private Result(Error error)
    {
        _value = default;
        IsSuccess = false;
        _error = error;
    }

    private readonly TValue? _value;
    private readonly Error? _error;

    public TValue Value =>
        IsSuccess ? _value! : throw new InvalidOperationException("Cannot get a value of a failure result");

    public Error Error =>
        IsFailure ? _error! : throw new InvalidOperationException("Cannot get a error of a success result");
    
    public ErrorCode ErrorCode => IsFailure ? 
        _error!.Code : throw new InvalidOperationException("Cannot get a error Code of a success result");

    public string ErrorMessage
        => IsFailure ? _error!.Message : throw new InvalidOperationException("Cannot get an error message of a success result");


    public bool IsSuccess { get; }
    public bool IsFailure => !IsSuccess;

    public static Result<TValue> Success(TValue value) => new(value);

    public static Result<TValue> Failure(Error error) => new(error);

    public static implicit operator Result<TValue>(TValue value) => new(value);
    public static implicit operator Result<TValue>(Error error) => new(error);
}