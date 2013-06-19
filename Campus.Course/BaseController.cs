using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Campus.Course
{
    public class BaseController : Controller
    {
        protected RawJsonResult RawJson(JsonEncoding raw,JsonRequestBehavior behavior)
        {
            if (raw == null)
                throw new ArgumentNullException("raw");

            return new RawJsonResult() { 
                Data = raw.ToString(),
                JsonRequestBehavior = behavior
            };
        }

    }
}