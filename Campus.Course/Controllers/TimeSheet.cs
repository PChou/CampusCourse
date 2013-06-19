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

        public TimeSheetController(ITimeSheet _s_timesheet)
        {
            s_timesheet = _s_timesheet;
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
                    oneEvent.AppendObject(new JsonConstant(10));//color
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

    }
}
