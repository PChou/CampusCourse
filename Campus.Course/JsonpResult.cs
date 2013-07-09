using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Campus.Course
{
    public class JsonpResult : ActionResult
    {
        private JsonEncoding rootJsonObject;
        private string callback;

        public JsonpResult(JsonEncoding RawJsonObject, string Callback)
        {
            rootJsonObject = RawJsonObject;
            callback = Callback;
        }

        public override void ExecuteResult(ControllerContext context)
        {
            string jsonp = callback + "(" + rootJsonObject.ToString() + ")";
            context.HttpContext.Response.ContentType = "application/json";
            context.HttpContext.Response.Write(jsonp);
        }
    }
}