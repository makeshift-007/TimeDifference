using System;
using System.Collections.Generic;
using System.Json;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Helpers;
using System.Web.Http.Controllers;

using System.Web.Mvc;
using Microsoft.Ajax.Utilities;
using ActionFilterAttribute = System.Web.Http.Filters.ActionFilterAttribute;

namespace TimeDifference.Services.Filters
{
    public class ValidationActionFilter:ActionFilterAttribute
    {
        public override void OnActionExecuting(HttpActionContext context)
        {
            var modelState = context.ModelState;
            if (!modelState.IsValid)
            {
                var errors = new JsonObject();
                foreach (var key in modelState.Keys )
                {
                    var state = modelState[key];
                    if (state.Errors.Any())
                    {
                        errors[key] = state.Errors.First().ErrorMessage;
                    }
                }
              
                context.Response = context.Request.CreateResponse(HttpStatusCode.BadRequest, errors);
                

            }
        }
    }
}