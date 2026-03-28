namespace ASPNET.Application.Common.Results
{
    public class Result
    {
        protected bool success;
        protected string? error;
        public bool Success => success;
        public bool Failed => !success;
        public string? ErrorMessage => error;
        protected Result(bool success, string? error)
        {
            this.success = success;
            this.error = error;
        }
        public static Result Ok() => new Result(true, null);
        public static Result Fail(string error) => new Result(false, error);
    }
}
