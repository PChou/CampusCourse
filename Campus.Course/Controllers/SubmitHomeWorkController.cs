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
    public class SubmitHomeWorkController : BaseController
    {
        private IStudent _student= null;
        private ITimeSheet _timesheet = null;
        private ITeach _teach = null;
        private IHomeWorkBiz _HomeWork = null;

        public SubmitHomeWorkController(IStudent __student, ITimeSheet __timesheet, ITeach __teach, IHomeWorkBiz __HomeWork)
        {
            _student = __student;
            _timesheet = __timesheet;
            _teach = __teach;
            _HomeWork = __HomeWork;
        }

        public ActionResult Index()
        {
            if (!CurrentUser.IsStudent)
                throw new Exception("The current user is not student.permission denied.");
            var ins = _student.GetInstituteInfoByStudent(null, CurrentUser.Student.Student.StudentNo, null);
            ViewBag.Grade = ins.Grade;
            ViewBag.QGrade = ins.QGrade;
            ViewBag.QGradeBegin = ins.BDate;
            var weekinfo = _timesheet.CalWeekInQGrade(ins.BDate.Value, DateTime.Now);
            ViewBag.WeekInfo = weekinfo;

            ViewBag.TeachInfoes = _teach.GetTeachInfoByStudent(null, CurrentUser.Student.Student.StudentNo, ins.ID);
            return View();
        }

        public ActionResult SubmitHomeWorkPartialView(int TimeSheetId)
        {
            List<HomeWorkInfo> list = _HomeWork.GetHomeWorkInfoBySheetId(null, TimeSheetId);
            ViewBag.HomeWorks = list;
            return PartialView("_SubmitHomeWorkPartial");
        }

        public void submithomework(int id, string commits)
        {
            _HomeWork.SubmitHomeWork(null, id, commits);
        }

    }
}
