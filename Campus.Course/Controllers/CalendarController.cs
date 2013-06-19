using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NRemedy.MVC.UI;

namespace Campus.Course.Controllers
{
    public class CalendarController : BaseController
    {
        //
        // GET: /Calendar/

        public ActionResult Query()
        {
            string json = "{\"start\":\"\\/Date(1370736000000)\\/\",\"end\":\"\\/Date(1371427199999)\\/\",\"error\":null,\"issort\":true,\"events\":[[157,\"asd\",\"\\/Date(1370908800000)\\/\",\"\\/Date(1370911500000)\\/\",0,0,1,1,1,null,null]]}";
            return new RawJsonResult() { Data = json, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }



    }
}
