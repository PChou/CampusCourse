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

        public JsonResult Login(string user, string password)
        {
            bool checkin = _login.CheckLogin(null, user, password, true);
            if (checkin)
            {
                FormsAuthentication.SetAuthCookie(user, false);
                //string returnUrl = Request["ReturnUrl"];
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
