using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Campus.Course.Business.Interface;

namespace Campus.Course.Controllers
{
    public class LoginController : Controller
    {
        private ILogin _login;

        public LoginController(ILogin __login)
        {
            _login = __login;
        }

        public ActionResult Index()
        {
            return View();
        }

        public JsonResult Login(string user, string password, string authentication)
        {
            bool checkin = true;
            string UserInfo = user + "|" + authentication;
           
            if (authentication == "student")
            {
                checkin = _login.CheckLogin(null, user, password, true);
            }
            else if (authentication == "teacher")
            {
                checkin = _login.CheckLogin(null, user, password, false);
            }
            
            if (checkin)
            {
                FormsAuthentication.SetAuthCookie(UserInfo, false);
                bool Success = true;
                return Json(Success);
            }
            else
            {
                bool Success = false;
                return Json(Success);
            }
        }

        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return Redirect(FormsAuthentication.LoginUrl);
        }
    }
}
