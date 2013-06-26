using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Campus.Course.Model.Business;
using Campus.Course.Business.Interface;
using Campus.Course.Business;

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

        protected override IActionInvoker CreateActionInvoker()
        {
            return new CustomControllerActionInvoker();
        }

        public V_CurrentUser CurrentUser
        {
            get
            {
                V_CurrentUser current = new Model.Business.V_CurrentUser();

                string[] UserInfo = HttpContext.User.Identity.Name.Split('|');
                string userName = UserInfo[0];
                string auth = UserInfo[1];
                bool isStudent = auth == "student";
                Login login = new Login();
                current = Session["V_CurrentUser"] as V_CurrentUser;
                if (current == null)
                {
                    current = login.GetCurrentUser(null, userName, isStudent);
                    Session["V_CurrentUser"] = current;
                }
               
                return current;
            }
        }
    }
}