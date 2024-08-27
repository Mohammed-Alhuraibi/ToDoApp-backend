public class OperationResult<T>
{
    public T? Data { get; }
    public bool IsSuccess { get; }
    public string ErrorMessage { get; }

    private OperationResult(T data)
    {
        Data = data;
        IsSuccess = true;
    }

    private OperationResult(string errorMessage)
    {
        ErrorMessage = errorMessage;
        IsSuccess = false;
    }

    public static OperationResult<T> Success(T data)
    {
        return new OperationResult<T>(data);
    }

    public static OperationResult<T> Failure(string errorMessage)
    {
        if (string.IsNullOrEmpty(errorMessage))
        {
            throw new ArgumentNullException(nameof(errorMessage), "Error message cannot be null or empty.");
        }

        return new OperationResult<T>(errorMessage);
    }
}
