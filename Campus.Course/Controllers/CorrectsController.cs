using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Campus.Course.Business.Interface;

namespace Campus.Course.Controllers
{
    public class CorrectsController : BaseController
    {
        private IStudent _student = null;
        private ITimeSheet _timesheet = null;
        private ITeach _teach = null;

        public CorrectsController(IStudent __student, ITimeSheet __timesheet, ITeach __teach)
        {
            _student = __student;
            _timesheet = __timesheet;
            _teach = __teach;
        }

        public ActionResult Index()
        {
            if (!CurrentUser.IsStudent)
                throw new Exception("The current user is not teacher.permission denied.");
            var ins = _student.GetInstituteInfoByStudent(null, CurrentUser.Student.Student.StudentNo, null);
            ViewBag.Grade = ins.Grade;
            ViewBag.QGrade = ins.QGrade;
            ViewBag.QGradeBegin = ins.BDate;
            var weekinfo = _timesheet.CalWeekInQGrade(ins.BDate.Value, DateTime.Now);
            ViewBag.WeekInfo = weekinfo;

            ViewBag.TeachInfoes = _teach.GetTeachInfoByStudent(null, CurrentUser.Student.Student.StudentNo, ins.ID);
            return View();
        }

        public ActionResult PartialView(int TimeSheetId)
        {


            return PartialView("_CorrectsPartial");


        } 
    }
}
