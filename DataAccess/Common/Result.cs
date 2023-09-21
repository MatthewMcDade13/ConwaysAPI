namespace DataAccess.Common;

public record Result<T> {
    public T? Data { get; init; }
    public bool Success { get; init; }
    public string? Message { get; init; }

    public static Result<T> Ok(T data) {
        return new Result<T> {
            Data = data,
            Success = true
        };
    }

    public static Result<T> Error(string message) {
        return new Result<T> {
            Message = message,
            Success = false,
        };
    }

    public Result<R> Map<R>(Func<T?, R> mapper) {
        if (Success) {            
            return new Result<R> {
                Data = mapper(Data),
            };
        } else {
            return Result<R>.Error(this.Message!);
        }
    }

    public override string ToString()
    {
        if (Success) {
            return Data?.ToString()!;
        } else {
            return Message!;
        }
    }
}