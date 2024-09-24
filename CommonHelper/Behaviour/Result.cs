namespace MeesageService.Shared.Behaviour
{
    public class Result
    {
        public bool IsSuccess { get; }
        public Errors? Error { get; }

        protected Result(bool isSuccess, Errors? error)
        {
            IsSuccess = isSuccess;
            Error = error;
        }

        public static Result Success() => new(true, Errors.None);
        public static Result<TValue> Success<TValue>(TValue value) => new(value, true, Errors.None);

        public static Result Failure(Errors error) => new Result(false, error);
        public static Result<TValue> Failure<TValue>(TValue value) => new(value, true, Errors.None);
    }

    public class Result<TValue> : Result
    {
        private readonly TValue? _value;

        // Constructor
        public Result(TValue? value, bool isSuccess, Errors? error)
            : base(isSuccess, error)
        {
            _value = value;
        }

        public TValue Value
        {
            get
            {
                if (IsSuccess)
                {
                    return _value!;
                }
                throw new InvalidOperationException("The value of a failure result can't be accessed.");
            }
        }

        public static Result<TValue> Success(TValue value) =>
            new Result<TValue>(value, true, Errors.None);

        public static Result<TValue> Failure(Errors error) =>
            new Result<TValue>(default, false, error);

        public static implicit operator Result<TValue>(TValue value) =>
            value != null ? Success(value) : Failure(Errors.Null);
    }


}



