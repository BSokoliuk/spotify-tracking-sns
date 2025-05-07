namespace Results;

public class CustomResult<T>
{
  private readonly T? _value;

  public bool IsSuccess { get; }
  public bool IsFailure => !IsSuccess;

  public T Value 
  {
    get
    {
      if (IsFailure)
        throw new InvalidOperationException("Cannot access Value when result is a failure.");
      return _value!;
    }

    private init => _value = value;
  }

  public CustomError? Error { get; }

  private CustomResult(T value)
  {
    Value = value;
    IsSuccess = true;
    Error = CustomError.None;
  }

  private CustomResult(CustomError error)
  {
    if (error == CustomError.None)
      throw new ArgumentException("Error cannot be None.", nameof(error));
    
    IsSuccess = false;
    Error = error;
  }

  public static CustomResult<T> Success(T value) => new(value);
  public static CustomResult<T> Failure(CustomError error) => new(error);
}