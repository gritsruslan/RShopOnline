namespace RShopAPI_Test.Core.Common;
public abstract class Result
{
    public Result(Error error)
    {
        _error = error;
        _successCode = null;
        IsSuccess = false;
    }

    public Result(SuccessCode successCode)
    {
        _error = null;
        _successCode = successCode;
        IsSuccess = true;
    }
    
    private readonly Error? _error;
    private readonly SuccessCode? _successCode;
    
    public bool IsSuccess { get; }
    public bool IsFailure => !IsSuccess;
    
    public Error Error => IsFailure ? 
        _error! : throw new InvalidOperationException("Cannot get an error of a success result!");
    
    public ErrorCode ErrorCode => IsFailure ? 
        _error!.Code : throw new InvalidOperationException("Cannot get an error code of a success result!");
    
    public string ErrorMessage => IsFailure ? 
        _error!.Message : throw new InvalidOperationException("Cannot get an error message of a success result!");
    
    public SuccessCode SuccessCode => IsSuccess ? 
        (SuccessCode) _successCode! : throw new InvalidOperationException("Cannot get an success code of failure result!");
}

public class Result<TValue> : Result
{
    public Result(Error error) : base(error)
    {
        _value = default;
    }

    public Result(TValue value, SuccessCode successCode) : base(successCode)
    {
        _value = value;
    }

    public Result(TValue value) : base(SuccessCode.Ok)
    {
        _value = value;
    }
    
    private readonly TValue? _value;
    
    public TValue Value => IsSuccess ? 
        _value! : throw new InvalidOperationException("Cannot get a value of a failure result!");

    public static Result<TValue> Success(TValue value) => new(value);
    public static Result<TValue> Success(TValue value, SuccessCode successCode) => new(value, successCode);
    public static implicit operator Result<TValue>(TValue value) => new(value);
    
    public static implicit operator Result<TValue>(Error error) => new(error);
}