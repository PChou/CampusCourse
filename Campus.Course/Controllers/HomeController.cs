using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Campus.Course.Business.Interface;
using Campus.Course.Model.Business;

namespace Campus.Course.Controllers
{
    public class HomeController : BaseController
    {
        private IStudent s_student;

        public HomeController(IStudent _s_student)
        {
            s_student = _s_student;
        }

        public ActionResult Index()
        {
            return View();
        }

    }
}
