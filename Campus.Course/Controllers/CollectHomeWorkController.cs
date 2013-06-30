using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Campus.Course.Business.Interface;
using Campus.Course.Model;
using Campus.Course.Model.Business;

namespace Campus.Course.Controllers
{
    public class CollectHomeWorkController : BaseController
    {
        private ITeacher _teacher = null;
        private ITimeSheet _timesheet = null;
        private ITeach _teach = null;
        private IHomeWorkBiz _HomeWork = null;

        public CollectHomeWorkController(ITeacher __teahcer, ITimeSheet __timesheet, ITeach __teach, IHomeWorkBiz __HomeWork)
        {
            _teacher = __teahcer;
            _timesheet = __timesheet;
            _teach = __teach;
            _HomeWork = __HomeWork;
        }

        public ActionResult Index()
        {
            if (CurrentUser.IsStudent)
                throw new Exception("The current user is not teacher.permission denied.");
            var ins = _teacher.GetInstituteInfoByTeacher(null, CurrentUser.Teacher.Teacher.TeacherNo, null);
            ViewBag.Grade = ins.Grade;
            ViewBag.QGrade = ins.QGrade;
            ViewBag.QGradeBegin = ins.BDate;
            var weekinfo = _timesheet.CalWeekInQGrade(ins.BDate.Value, DateTime.Now);
            ViewBag.WeekInfo = weekinfo;

            ViewBag.TeachInfoes = _teach.GetTeachInfoByTeacher(null, CurrentUser.Teacher.Teacher.TeacherNo, ins.ID);
            return View();
        }

        public ActionResult CollectHomeWorkPartialView(int TimeSheetId)
        {
            List<HomeWorkExtend> list = _HomeWork.GetHomeWorkExtendByPushId(null, TimeSheetId);
            ViewData["HomeWorkExtend"] = list;
            return PartialView("_CollectHomeWorkPartial");
        } 
    }
}
