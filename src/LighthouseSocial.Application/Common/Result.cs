namespace LighthouseSocial.Application.Common;

public class Result<T> : Result
{
    private Result(bool isSuccess, T data, string errorMessage) : base(isSuccess, errorMessage)
    {
        Data = data;
    }

    public T Data { get; }

    public static Result<T> Success(T data) => new Result<T>(true, data, string.Empty);
    
    public static Result<T> Failure(string errorMessage) => new Result<T>(false, default, errorMessage);
}


public class Result
{
    protected Result(bool isSuccess, string errorMessage)
    {
        IsSuccess = isSuccess;
        ErrorMessage = errorMessage;
    }

    public bool IsSuccess { get; }
    public string ErrorMessage { get; }

    public static Result Success() => new Result(true, string.Empty);
    
    public static Result Failure(string errorMessage) => new Result(false, errorMessage);
}