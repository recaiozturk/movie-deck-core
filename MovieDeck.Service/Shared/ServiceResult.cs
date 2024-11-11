namespace MovieDeck.Service.Shared
{
    public class ServiceResult<T>
    {
        public T? Data { get; set; }

        public List<string>? Errors { get; set; }

        public string GetFirstError => Errors!.First();


        public bool AnyError => Errors != null && Errors.Count > 0;

        public bool AnySuccess => !AnyError;

        public static ServiceResult<T> Success(T data)
        {
            return new ServiceResult<T>
            {
                Data = data
            };
        }

        public static ServiceResult<T> Fail(List<string> errors)
        {
            return new ServiceResult<T>
            {
                Errors = errors
            };
        }

        public static ServiceResult<T> Fail(string error)
        {
            return new ServiceResult<T>
            {
                Errors = [error]
            };
        }
    }

    public class ServiceResult
    {
        public List<string>? Errors { get; set; }
        public bool AnyError => Errors != null && Errors.Count > 0;

        public bool AnySuccess => !AnyError;
        public string GetFirstError => Errors!.First();

        public static ServiceResult Success()
        {
            return new ServiceResult();
        }

        // Static Factory Method 
        public static ServiceResult Fail(List<string> errors)
        {
            return new ServiceResult
            {
                Errors = errors
            };
        }

        public static ServiceResult Fail(string error)
        {
            return new ServiceResult
            {
                Errors = [error]
            };
        }
    }
}
