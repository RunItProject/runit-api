using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Net.Http.Headers;
using Runit.Backend.Models;

namespace Runit.Backend.Infrastructure
{
    internal static class ControllerExtensions
    {
        public static StatusCodeResult InternalServerError(this Controller controller) {
            return controller.StatusCode(500);
        }
    }
}