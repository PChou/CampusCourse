using Campus.Course.Business.Interface;
using Campus.Course.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

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
        public ActionResult Index(string teachno, int? sheetid, int? prepid)
        {
            if (CurrentUser.IsStudent)
                throw new Exception("The current user is not teacher.permission denied.");
            var ins = s_teacher.GetInstituteInfoByTeacher(null, CurrentUser.Teacher.Teacher.TeacherNo, null);
            ViewBag.Grade = ins.Grade;
            ViewBag.QGrade = ins.QGrade;
            ViewBag.QGradeBegin = ins.BDate;
            var weekinfo = s_timesheet.CalWeekInQGrade(ins.BDate.Value, DateTime.Now);
            ViewBag.WeekInfo = weekinfo;

            var teachinfoes = s_teach.GetTeachInfoByTeacher(null, CurrentUser.Teacher.Teacher.TeacherNo, ins.ID);
            
            ViewBag.TeachInfoes = teachinfoes;
            PreparationDetailViewModel model = new PreparationDetailViewModel();
            ViewData.Model = model;

            if (!string.IsNullOrEmpty(teachno))
            {
                //ViewBag.TeachNo = teachno;
                model.TeachNo = teachno;
                int count = 0;
                foreach (var a in teachinfoes)
                {
                    if (a.Teach.TeachNo == teachno)
                    {
                        ViewBag.AcitveIndex = count;
                        break;
                    }
                    count++;
                }
            }
            if (sheetid != null)
            {
                //ViewBag.SheetId = sheetid;
                model.SheetId = sheetid.Value;
                var homeworkpushes = s_homeworkpush.GetHomeWorkPushByWorkSheetId(null, sheetid.Value);
                foreach (var work in homeworkpushes)
                {
                    HomeworkViewModel target = new HomeworkViewModel();
                    target.HomeworkId = work.ID;
                    //target.SheetId = work.TeachTimeSheetId.Value;
                    target.Subject = work.Subject;
                    target.Description = work.Description;
                    target.DeadLine = work.DeadLine;
                    target.PushDate = work.PushDate;
                    model.HomeworkPushes.Add(target);
                }
            }
            if (prepid != null)
            {
                //ViewBag.PrepId = prepid;
                model.PreparationId = prepid.Value;
                Course.Model.Preparation prepInfo = s_prep.GetPreparationByPId(null, prepid.Value);
                if (prepInfo != null)
                {
                    model.PreparationId = prepInfo.ID;
                    model.PreparationName = prepInfo.PrepName;
                    model.PreparationContent = prepInfo.PrepContent; 
                }

                
            }

            return View();
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Index(string teachno, int? sheetid, int? prepid,PreparationDetailViewModel model)
        {
            Preparation prep = new Preparation();
            prep.ID = model.PreparationId;
            prep.PrepName = model.PreparationName;
            prep.PrepContent = model.PreparationContent;
            prep.TeachTimeSheetId = model.SheetId;
            s_prep.SavePreparation(null, prep);

            return View();
        }

        //public ActionResult GetHomworkPush(int TimeSheetId)
        //{
        //    var homeworkpushes = s_homeworkpush.GetHomeWorkPushByWorkSheetId(null, TimeSheetId);
        //    JsonCollection mcollection = new JsonCollection();
        //    foreach (var push in homeworkpushes)
        //    {
        //        JsonObject o = new JsonObject();
        //        o.MergeProperty("id", new JsonConstant(push.ID));
        //        o.MergeProperty("sheetid", new JsonConstant(push.TeachTimeSheetId));
        //        o.MergeProperty("subject", new JsonConstant(push.Subject));
        //        o.MergeProperty("description", new JsonConstant(push.Description));
        //        o.MergeProperty("deadline", new JsonConstant(push.DeadLine.Value.ToShortDateString()));
        //        o.MergeProperty("pushdate", new JsonConstant(push.PushDate.Value.ToShortDateString()));
        //        mcollection.AppendObject(o);
        //    }

        //    return RawJson(mcollection, JsonRequestBehavior.AllowGet);
        //}


        //        {
        //    id:
        //    sheetid:
        //    name:
        //    content:
        //    teachno:
        //    error:
        //          }
        //[HttpPost]
        //[ValidateInput(false)]
        //public ActionResult SavePreparation(Preparation prep)
        //{
        //    JsonObject o = new JsonObject();
        //    o.MergeProperty("sheetid", new JsonConstant(prep.TeachTimeSheetId));
        //    try
        //    {
        //        var result = s_prep.SavePreparation(null,prep);
        //        o.MergeProperty("id", new JsonConstant(result.ID));
        //        o.MergeProperty("name", new JsonConstant(result.PrepName ?? string.Empty));
        //        o.MergeProperty("content", new JsonConstant(result.PrepContent ?? string.Empty));
        //    }
        //    catch(Exception ex) { 
        //        o.MergeProperty("error",new JsonConstant(ex.Message));
        //    }
        //    return RawJson(o, JsonRequestBehavior.DenyGet);
        //}


        //{id:sheetid:subject:description:deadline,pushdate,error}
        [HttpPost]
        public ActionResult SaveHomeworkPush(HomeworkPushSubmitModel homework)
        {
            HomeWorkPush hp = new HomeWorkPush();
            hp.Subject = homework.Subject;
            hp.Description = homework.Description;
            hp.DeadLine = homework.DeadLine;
            hp.Evaluation = homework.Evaluation;
            hp.TeachTimeSheetId = homework.SheetId;
            hp.TeachNo = homework.TeachNo;
            if (homework.HomeworkId != null)//create
            {
                hp.ID = homework.HomeworkId.Value;
            }
            s_homeworkpush.SaveHomeworkPush(null, hp);

            RouteValueDictionary param = new RouteValueDictionary();
            param.Add("teachno", homework.TeachNo);
            param.Add("sheetid", homework.SheetId);
            param.Add("prepid", homework.PreparationId);

            return RedirectToAction("Index", param);

            //return Index(homework.TeachNo, homework.SheetId, homework.PreparationId);


            //JsonObject o = new JsonObject();
            //try
            //{
            //    var hwp = s_homeworkpush.SaveHomeworkPush(null, homework);
            //    o.MergeProperty("id", new JsonConstant(hwp.ID));
            //    o.MergeProperty("sheetid", new JsonConstant(hwp.TeachTimeSheetId));
            //    o.MergeProperty("subject", new JsonConstant(hwp.Subject));
            //    o.MergeProperty("description", new JsonConstant(hwp.Description));
            //    o.MergeProperty("deadline", new JsonConstant(hwp.DeadLine.Value.ToShortDateString()));
            //    o.MergeProperty("pushdate", new JsonConstant(hwp.PushDate.Value.ToShortDateString()));
            //}
            //catch(Exception ex)
            //{
            //    o.MergeProperty("error", new JsonConstant(ex.Message));
            //}
            //return RawJson(o, JsonRequestBehavior.DenyGet);
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
                o.MergeProperty("downloadurl", new JsonConstant(string.Format("/File/DownloadPrepM?mId={0}", pm.ID)));
                o.MergeProperty("preview", new JsonConstant(true));
                o.MergeProperty("removeurl", new JsonConstant("/File/DeletePrepM"));
                ms.AppendObject(o);
            }

            return RawJson(ms, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetHomeworkMateiral(int HomworkId)
        {
            JsonCollection ms = new JsonCollection();
            var pms = s_prep.GetHomeworkPushMateiralByHomworkId(null, HomworkId);

            foreach (var pm in pms)
            {
                JsonObject o = new JsonObject();
                o.MergeProperty("id", new JsonConstant(pm.ID));
                o.MergeProperty("name", new JsonConstant(pm.Name));
                o.MergeProperty("downloadurl", new JsonConstant(string.Format("/File/DownloadHomeworkPushM?hId={0}", pm.ID)));
                o.MergeProperty("preview", new JsonConstant(true));
                o.MergeProperty("removeurl", new JsonConstant("/File/DeleteHomeworkPushM"));
                ms.AppendObject(o);
            }

            return RawJson(ms, JsonRequestBehavior.AllowGet);
        }

        public ActionResult HomeworkMFunction(int Id)
        {
            string function = string.Format(@"
                function setHomeworkM{0}(){{
                var option = {{
                            uniqueId:'{1}',
                            uploadurl: '/File/UploadHomeworkM?HomworkId={0}',
                            refreshurl: '/Preparation/GetHomeworkMateiral?HomworkId={0}',
                            autorefresh: true,
                            onPreview: function (row) {{
                                alert('preview' + row.id);
                            }},
                            onBeforeRemove: function (r, param) {{
                                if (confirm('确定删除?')) {{
                                    param.mId = r.id;
                                    return true;
                                }}
                                else {{
                                    return false;
                                }}
                            }},
                            onSuccessed: function (data, status) {{
                                if (typeof (data.error) != 'undefined') {{
                                    if (data.error == null || data.error == '') {{
                                        alert('上传成功');
                                    }}
                                    else {{
                                        alert(data.error);
                                    }}
                                }}
                            }},
                            onError: function (data, status, e) {{
                                alert(e);
                            }}
                        }};
                        $('#HomeworkMeteiral{0}').attpool(option);
                }}
            ", Id,Guid.NewGuid().ToString());
            return Content(function);
        }
    }
}
