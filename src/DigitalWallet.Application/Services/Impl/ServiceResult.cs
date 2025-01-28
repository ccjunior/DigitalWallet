namespace DigitalWallet.Application.Services.Impl
{
    public class ServiceResult<T>
    {
        public bool Success { get; private set; }
        public string Message { get; private set; }
        public T Data { get; private set; }

        private ServiceResult(bool success, string message, T data)
        {
            Success = success;
            Message = message;
            Data = data;
        }

        public static ServiceResult<T> SuccessResult(T data, string message = "Operação realizada com sucesso.")
        {
            return new ServiceResult<T>(true, message, data);
        }

        public static ServiceResult<T> ErrorResult(string message)
        {
            return new ServiceResult<T>(false, message, default);
        }
    }
}
