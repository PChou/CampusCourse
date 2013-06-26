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
        //private IStudent s_student;

        public TimeSheetController(ITimeSheet _s_timesheet)//,IStudent _s_student)
        {
            s_timesheet = _s_timesheet;
            //s_student = _s_student;
        }

        public ActionResult Index()
        {
            //clases select
            ViewBag.Classes = s_timesheet.GetClasses(null);
            var a = ViewBag.aa;

            return View();
        }


        public ActionResult GetTimeSheetByStudent(string SNo, DateTime? showdate, string viewtype)
        {
            JsonEncoding root;
            DateTime startTime = default(DateTime);
            DateTime endTime = default(DateTime);

            try
            {
                CalculateDateDuration(viewtype, showdate, out startTime, out endTime);

                var sheet = s_timesheet.GetSheetCourseInfoByStudent(null, SNo, startTime, endTime);

                root = BuildTimeSheetJson(sheet, startTime, endTime, null);
            }
            catch (Exception ex)
            {
                root = BuildTimeSheetJson(null, startTime, endTime, ex);
            }

            return RawJson(root, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetTimeSheetByClass(string CNo, DateTime? showdate, string viewtype)
        {
            JsonEncoding root;
            DateTime startTime = default(DateTime);
            DateTime endTime = default(DateTime);

            try
            {
                CalculateDateDuration(viewtype, showdate, out startTime, out endTime);

                var sheet = s_timesheet.GetSheetCourseInfoByClass(null, CNo, startTime, endTime);

                root = BuildTimeSheetJson(sheet, startTime, endTime, null);
            }
            catch (Exception ex)
            {
                root = BuildTimeSheetJson(null, startTime, endTime, ex);
            }

            return RawJson(root, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetTimeSheetInfo(string SNo,DateTime showdate)
        {
            var stuInfo = s_timesheet.GetTimeSheetInfo(null, SNo, showdate);
            JsonObject j = new JsonObject();
            j.MergeProperty("grade", new JsonConstant(stuInfo.ClassGrade));
            j.MergeProperty("gradeq", new JsonConstant(stuInfo.ClassGradeQ));
            j.MergeProperty("currentdate", new JsonConstant(string.Format("{0}年{1}月{2}日",showdate.Year,showdate.Month,showdate.Day)));
            //计算周次
            if (stuInfo.QB != null)//非假期才计算,这时showdate >= stuInfo.QB && showdate <= stuInfo.QE
            {
                int chinaDayOfWeekOffset = stuInfo.QB.Value.DayOfWeek == DayOfWeek.Sunday ? 6 : (int)stuInfo.QB.Value.DayOfWeek - 1;
                var startTime = stuInfo.QB.Value.AddDays(chinaDayOfWeekOffset * -1);
                var a = showdate - startTime;
                j.MergeProperty("currentweek", new JsonConstant(string.Format("第{0}周", a.Days / 7 + 1)));
            }
                
            return RawJson(j, JsonRequestBehavior.AllowGet);
        }


        public ActionResult GetTimesheetsByTeachNo(DateTime QGradeBegin ,string TeachNo)
        {
            var timesheets = s_timesheet.GetTimesheetsByTeachNo(null,TeachNo);
            JsonCollection collection = new JsonCollection();
            if (timesheets != null) {
                Dictionary<int, JsonCollection> tmp = new Dictionary<int, JsonCollection>();
                
                foreach (var time in timesheets)
                {
                    int week = s_timesheet.CalWeekInQGrade(QGradeBegin, time.Date).Week;
                    if (tmp.ContainsKey(week))
                    {
                        JsonObject child = new JsonObject();
                        child.MergeProperty("label", new JsonConstant(time.PreparationName));
                        child.MergeProperty("id", new JsonConstant(time.PreparationID));
                        child.MergeProperty("date", new JsonConstant(time.Date.ToShortDateString()));
                        tmp[week].AppendObject(child);
                    }
                    else
                    {
                        JsonObject each = new JsonObject();
                        each.MergeProperty("label", new JsonConstant("第" + week + "周"));

                        JsonObject child = new JsonObject();
                        child.MergeProperty("label", new JsonConstant(time.PreparationName));
                        child.MergeProperty("id", new JsonConstant(time.PreparationID));
                        child.MergeProperty("date", new JsonConstant(time.Date.ToShortDateString()));
                        JsonCollection children = new JsonCollection();
                        children.AppendObject(child);
                        each.MergeProperty("children", children);

                        collection.AppendObject(each);

                        //store in dic
                        tmp.Add(week, children);                   

                    }
 
                }

                
            }
            return RawJson(collection, JsonRequestBehavior.AllowGet);
 

        }

        private void CalculateDateDuration(string viewtype, DateTime? showdate, out DateTime startTime, out DateTime endTime)
        {
            if (viewtype == "week")
            {
                int chinaDayOfWeekOffset = showdate.Value.DayOfWeek == DayOfWeek.Sunday ? 6 : (int)showdate.Value.DayOfWeek - 1;
                startTime = showdate.Value.AddDays(chinaDayOfWeekOffset * -1);
                endTime = showdate.Value.AddDays(6 - chinaDayOfWeekOffset);
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
            else
            {
                throw new NotSupportedException("onlt week view or month view supported.");
            }
        }

        private JsonEncoding BuildTimeSheetJson(IEnumerable<SheetCourseInfo> sheet, DateTime startTime, DateTime endTime, Exception ex)
        {
            JsonObject root = new JsonObject();
            JsonCollection events = new JsonCollection();
            root.MergeProperty("start", new JsonDateTime(startTime));
            root.MergeProperty("end", new JsonDateTime(endTime));
            if (ex != null)
            {
                root.MergeProperty("error", new JsonConstant(ex.ToString()));
            }
            else
            {
                root.MergeProperty("error", new JsonConstant(null));
            }
            root.MergeProperty("issort", new JsonConstant(true));//if issort is false,must provider a sort function on client
            root.MergeProperty("events", events);
            if (sheet != null)
            {
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

            return root;
        }

    }
}
