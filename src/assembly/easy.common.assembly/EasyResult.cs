using Microsoft.AspNetCore.Mvc;
using System;

namespace Easy.Common.Assembly
{
    public static class EasyResponse
    {
        public static IActionResult Success()
        {
            var returnBody = new ReturnBody();
            return new OkObjectResult(returnBody);
        }

        public static ActionResult Success<T>(T data)
            where T : class, new()
        {
            var returnBody = new ReturnBody<T>(data);
            return new OkObjectResult(returnBody);
        }

        public static ActionResult Fail(int code,string message)
        {
            var returnBody = new ReturnBody(code, message);
            return new OkObjectResult(returnBody);
        }

        public static ActionResult Fail(string message)
        {
            var returnBody = new ReturnBody(-1, message);
            return new OkObjectResult(returnBody);
        }
    }

    public class ReturnBody<T>: ReturnBody
        where T : class, new()
    {
        public ReturnBody()
        { 
        }

        public ReturnBody(int code, string message)
        :base(code,message)
        { 
        }

        public ReturnBody(int code, string message, T data)
        :base(code,message)
        {
            Data = data;
        }

        public ReturnBody(T data)
        {
            Data = data;
        }

        public T Data { get; set; }
    }

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
}
