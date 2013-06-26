using Campus.Course.Business.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Campus.Course.Controllers
{
    public class PreparationController : BaseController
    {
        private ITeacher s_teacher = null;
        private ITimeSheet s_timesheet = null;
        private ITeach s_teach = null;

        public PreparationController(ITeacher _teahcer, ITimeSheet _timesheet,ITeach _teach)
        {
            s_teacher = _teahcer;
            s_timesheet = _timesheet;
            s_teach = _teach;
        }

        //
        // GET: /Preparation/
        public ActionResult Index()
        {
            if (CurrentUser.IsStudent)
                throw new Exception("The current user is not teacher.permission denied.");
            var ins = s_teacher.GetInstituteInfoByTeacher(null, CurrentUser.Teacher.Teacher.TeacherNo, null);
            ViewBag.Grade = ins.Grade;
            ViewBag.QGrade = ins.QGrade;
            ViewBag.QGradeBegin = ins.BDate;
            var weekinfo = s_timesheet.CalWeekInQGrade(ins.BDate.Value, DateTime.Now);
            ViewBag.WeekInfo = weekinfo;

            ViewBag.TeachInfoes = s_teach.GetTeachInfoByTeacher(null, CurrentUser.Teacher.Teacher.TeacherNo, ins.ID);

            return View();
        }

        public ActionResult GetPreparation(int prepId)
        {
            return new EmptyResult();
 
        }
    }
}
