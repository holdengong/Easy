using Easy.Common.Assembly;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;

namespace Microsoft.AspNetCore.Mvc
{
    public static class EasyResult
    {
        public static IActionResult PagedList<T>(IEnumerable<T> list, int total)
            where T:class,new()
        {
            var pagedReturnBody = new PagedReturnBody<T>
            {
                List = list,
                Total = total
            };

            return Ok(pagedReturnBody);
        }

        public static IActionResult Ok()
        {
            var returnBody = new ReturnBody();
            return new OkObjectResult(returnBody);
        }

        public static ActionResult Ok<T>(T data)
            where T : class, new()
        {
            var returnBody = new ReturnBody<T>(data);
            return new OkObjectResult(returnBody);
        }

        public static ActionResult Error(int code,string message)
        {
            var returnBody = new ReturnBody(code, message);
            return new OkObjectResult(returnBody);
        }

        public static ActionResult Error(string message)
        {
            var returnBody = new ReturnBody(-1, message);
            return new OkObjectResult(returnBody);
        }
    }
}
