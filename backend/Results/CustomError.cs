namespace Results;

public sealed record CustomError(string Code, string Message)
{
  private static readonly string _recordNotFoundCode = "RecordNotFound";
  private static readonly string _validationErrorCode = "ValidationError";

  public static readonly CustomError None = new(string.Empty, string.Empty);
  public static CustomError RecordNotFound(string Message) => new(_recordNotFoundCode, Message);
  public static CustomError ValidationError(string Message) => new(_validationErrorCode, Message);

  public static readonly string NotFoundCode = _recordNotFoundCode;
  public static readonly string ValidationErrorCode = _validationErrorCode;
}