namespace Microsoft.AspNetCore.Mvc
{
    public class ReturnBody
    {
        public ReturnBody()
        { 
        }

        public ReturnBody(int code, string message)
        {
            Code = code;
            Message = message;
        }

        public int Code { get; set; }
        public string Message { get; set; }
    }

    public class ReturnBody<T> : ReturnBody
       where T : class, new()
    {
        public ReturnBody()
        {
        }

        public ReturnBody(int code, string message)
        : base(code, message)
        {
        }

        public ReturnBody(int code, string message, T data)
        : base(code, message)
        {
            Data = data;
        }

        public ReturnBody(T data)
        {
            Data = data;
        }

        public T Data { get; set; }
    }
}
