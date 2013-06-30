using Campus.Course.Business.Interface;
using Campus.Course.Model;
using System;
using System.Collections.Generic;
using System.IO;
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
        private IPreparation s_prep = null;
        private IHomeWorkPushBiz s_homeworkpush = null;

        public PreparationController(ITeacher _teahcer, ITimeSheet _timesheet, ITeach _teach, IPreparation _prep, IHomeWorkPushBiz _homeworkpush)
        {
            s_teacher = _teahcer;
            s_timesheet = _timesheet;
            s_teach = _teach;
            s_prep = _prep;
            s_homeworkpush = _homeworkpush;
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

//        {
        //    id:
        //    sheetid:
        //    name:
        //    content:
        //    teachno:
        //    m:[
        //    {id:type:name:path},
        //    {id:type:name:path}
        //    ]
//          }
        public ActionResult GetPreparation(int prepId, int sheetid)
        {
            JsonObject o = new JsonObject();
            o.MergeProperty("sheetid", new JsonConstant(sheetid));
            if (prepId > 0)
            {
                var prepInfoes = s_prep.GetPreparationByPId(null, prepId);


                foreach (var oneprep in prepInfoes.GroupBy(m => m.PrepId))
                {
                    o.MergeProperty("id", new JsonConstant(oneprep.First().PrepId));                    
                    o.MergeProperty("name", new JsonConstant(oneprep.First().PrepName ?? string.Empty));
                    o.MergeProperty("content", new JsonConstant(oneprep.First().PrepContent ?? string.Empty));
                    JsonCollection mcollection = new JsonCollection();
                    o.MergeProperty("m", mcollection);
                    foreach (var m in oneprep)
                    {
                        if (m.MeteiralId < 1)
                            continue;
                        JsonObject mo = new JsonObject();
                        mo.MergeProperty("id", new JsonConstant(m.MeteiralId));
                        mo.MergeProperty("type", new JsonConstant(m.MeteiralType));
                        mo.MergeProperty("name", new JsonConstant(m.MeteiralName ?? string.Empty));
                        mo.MergeProperty("path", new JsonConstant(m.MeteiralPath));
                        mcollection.AppendObject(mo);
                    }
                }
            }

            return RawJson(o,JsonRequestBehavior.AllowGet);
 
        }

        //[
            //{id:sheetid:subject:description:deadline,pushdate},
            //{id:sheetid:subject:description:deadline,pushdate}
        //]
        public ActionResult GetHomworkPush(int TimeSheetId)
        {
            var homeworkpushes = s_homeworkpush.GetHomeWorkPushByWorkSheetId(null, TimeSheetId);
            JsonCollection mcollection = new JsonCollection();
            foreach (var push in homeworkpushes)
            {
                JsonObject o = new JsonObject();
                o.MergeProperty("id", new JsonConstant(push.ID));
                o.MergeProperty("sheetid", new JsonConstant(push.TeachTimeSheetId));
                o.MergeProperty("subject", new JsonConstant(push.Subject));
                o.MergeProperty("description", new JsonConstant(push.Description));
                o.MergeProperty("deadline", new JsonConstant(push.DeadLine.Value.ToShortDateString()));
                o.MergeProperty("pushdate", new JsonConstant(push.PushDate.Value.ToShortDateString()));
                mcollection.AppendObject(o);
            }

            return RawJson(mcollection, JsonRequestBehavior.AllowGet);
        }


        //        {
        //    id:
        //    sheetid:
        //    name:
        //    content:
        //    teachno:
        //    error:
        //          }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult SavePreparation(Preparation prep)
        {
            JsonObject o = new JsonObject();
            o.MergeProperty("sheetid", new JsonConstant(prep.TeachTimeSheetId));
            try
            {
                var result = s_prep.SavePreparation(null,prep);
                o.MergeProperty("id", new JsonConstant(result.ID));
                o.MergeProperty("name", new JsonConstant(result.PrepName ?? string.Empty));
                o.MergeProperty("content", new JsonConstant(result.PrepContent ?? string.Empty));
            }
            catch(Exception ex) { 
                o.MergeProperty("error",new JsonConstant(ex.Message));
            }
            return RawJson(o, JsonRequestBehavior.DenyGet);
        }


        //{id:sheetid:subject:description:deadline,pushdate,error}
        [HttpPost]
        public ActionResult SaveHomeworkPush(HomeWorkPush homework)
        {
            JsonObject o = new JsonObject();
            try
            {
                var hwp = s_homeworkpush.SaveHomeworkPush(null, homework);
                o.MergeProperty("id", new JsonConstant(hwp.ID));
                o.MergeProperty("sheetid", new JsonConstant(hwp.TeachTimeSheetId));
                o.MergeProperty("subject", new JsonConstant(hwp.Subject));
                o.MergeProperty("description", new JsonConstant(hwp.Description));
                o.MergeProperty("deadline", new JsonConstant(hwp.DeadLine.Value.ToShortDateString()));
                o.MergeProperty("pushdate", new JsonConstant(hwp.PushDate.Value.ToShortDateString()));
            }
            catch(Exception ex)
            {
                o.MergeProperty("error", new JsonConstant(ex.Message));
            }
            return RawJson(o, JsonRequestBehavior.DenyGet);
        }

        //[
        //    {id:type:name:path},
        //    {id:type:name:path}
        //    ]
        public ActionResult GetPrepMateiral(int PrepId)
        {
            JsonCollection ms = new JsonCollection();
            var pms = s_prep.GetPrepMateiralByPrepId(null, PrepId);

            foreach (var pm in pms)
            {
                JsonObject o = new JsonObject();
                o.MergeProperty("id", new JsonConstant(pm.ID));
                o.MergeProperty("name", new JsonConstant(pm.Name));
                ms.AppendObject(o);
            }

            return RawJson(ms, JsonRequestBehavior.AllowGet);
        }
    }
}
