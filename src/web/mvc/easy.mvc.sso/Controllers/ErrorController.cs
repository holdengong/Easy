using IdentityServer4.Models;
using IdentityServer4.Stores;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Identity.API.Controllers
{
    public class ErrorController : Controller
    {
        private readonly IMessageStore<ErrorMessage> _errorMessageStore;

        public ErrorController(IMessageStore<ErrorMessage> errorMessageStore)
        {
            _errorMessageStore = errorMessageStore;
        }

        public async Task<IActionResult> Index(string errorId)
        {
            var decryptedError = await _errorMessageStore.ReadAsync(errorId);
            
            return View(decryptedError.Data);
        }
    }
}
