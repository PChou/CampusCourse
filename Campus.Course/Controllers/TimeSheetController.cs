using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Campus.Course.Business.Interface;
using Campus.Course.Model.Business;
using Campus.Course.Model;

namespace Campus.Course.Controllers
{
    public class TimeSheetController : BaseController
    {
        private ITimeSheet s_timesheet;
        private IStudent s_student;

        public TimeSheetController(ITimeSheet _s_timesheet,IStudent _s_student)
        {
            s_timesheet = _s_timesheet;
            s_student = _s_student;
        }

        public ActionResult Index()
        {
            return View();
        }


        public ActionResult GetTimeSheetByStudent(string SNo, DateTime? showdate, string viewtype)
        {
            JsonObject root = new JsonObject();
            try
            {
                DateTime startTime = default(DateTime);
                DateTime endTime = default(DateTime);
                
                if (viewtype == "week")
                {
                    int chinaDayOfWeekOffset = showdate.Value.DayOfWeek == DayOfWeek.Sunday ? 6 : (int)showdate.Value.DayOfWeek - 1;
                    startTime = showdate.Value.AddDays(chinaDayOfWeekOffset * -1);
                    endTime = showdate.Value.AddDays(6-chinaDayOfWeekOffset); 
                }
                else if (viewtype == "month")
                {
                    //获取一个月的第一天
                    DateTime fday = new DateTime(showdate.Value.Year, showdate.Value.Month, 1);
                    int chinaDayOfWeekOffset = fday.DayOfWeek == DayOfWeek.Sunday ? 6 : (int)fday.DayOfWeek - 1;
                    startTime = fday.AddDays(chinaDayOfWeekOffset * -1);

                    //获取一个月的最后一天
                    int Days = DateTime.DaysInMonth(showdate.Value.Year, showdate.Value.Month);
                    DateTime eday = fday.AddDays(Days);
                    int chinaDayOfWeekOffset2 = fday.DayOfWeek == DayOfWeek.Sunday ? 6 : (int)fday.DayOfWeek - 1;
                    endTime = eday.AddDays(6 - chinaDayOfWeekOffset2); 
                }

                var sheet = s_timesheet.GetSheetCourseInfoByStudent(null, SNo, startTime, endTime);

                root = new JsonObject();
                JsonCollection events = new JsonCollection();
                root.MergeProperty("start", new JsonDateTime(startTime));
                root.MergeProperty("end", new JsonDateTime(endTime));
                root.MergeProperty("error", new JsonConstant(null));
                root.MergeProperty("issort", new JsonConstant(true));//if issort is false,must provider a sort function on client
                root.MergeProperty("events", events);
                foreach (var s in sheet)
                {
                    JsonCollection oneEvent = new JsonCollection();
                    oneEvent.AppendObject(new JsonConstant(s.ID));
                    oneEvent.AppendObject(new JsonConstant(s.CourseName));
                    oneEvent.AppendObject(new JsonDateTime(s.Date.AddMilliseconds(s.BTime * 3600000).ToUniversalTime()));
                    oneEvent.AppendObject(new JsonDateTime(s.Date.AddMilliseconds(s.ETime * 3600000).ToUniversalTime()));
                    oneEvent.AppendObject(new JsonConstant(0));
                    oneEvent.AppendObject(new JsonConstant(0));
                    oneEvent.AppendObject(new JsonConstant(0));
                    oneEvent.AppendObject(new JsonConstant(s.CourseTheme == null ? 1 : s.CourseTheme.Value));//color
                    oneEvent.AppendObject(new JsonConstant(1));//permission
                    oneEvent.AppendObject(new JsonConstant(s.Location));
                    oneEvent.AppendObject(new JsonConstant(s.TeacherName));
                    events.AppendObject(oneEvent);
                }

            }
            catch(Exception ex)
            {
                root.MergeProperty("error", new JsonConstant(ex.ToString()));
            }

            return RawJson(root,JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetStudentInfo(string SNo,DateTime showdate)
        {
            var stuInfo = s_student.GetStudentBySNo(null,SNo,showdate);
            JsonObject j = new JsonObject();
            j.MergeProperty("grade", new JsonConstant(stuInfo.ClassGrade));
            j.MergeProperty("gradeq", new JsonConstant(stuInfo.ClassGradeQ));
            j.MergeProperty("class", new JsonConstant(stuInfo.ClassNo));
            j.MergeProperty("specialty", new JsonConstant(stuInfo.ClassSpecialty));
            j.MergeProperty("institute", new JsonConstant(stuInfo.ClassInstitute));

            return RawJson(j, JsonRequestBehavior.AllowGet);
        }

    }
}
