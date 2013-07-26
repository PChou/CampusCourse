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
        private IPreparation _prep = null;

        public SubmitHomeWorkController(IStudent __student, ITimeSheet __timesheet, ITeach __teach, IHomeWorkBiz __HomeWork, IPreparation __prep)
        {
            _student = __student;
            _timesheet = __timesheet;
            _teach = __teach;
            _HomeWork = __HomeWork;
            _prep = __prep;
        }

        public ActionResult Index(string TeachNo, int? TimeSheetId, int? HomeworkId)
        {
            if (!CurrentUser.IsStudent)
                throw new Exception("The current user is not student.permission denied.");
            var ins = _student.GetInstituteInfoByStudent(null, CurrentUser.Student.Student.StudentNo, null);
            ViewBag.Grade = ins.Grade;
            ViewBag.QGrade = ins.QGrade;
            ViewBag.QGradeBegin = ins.BDate;
            ViewBag.WeekInfo = _timesheet.CalWeekInQGrade(ins.BDate.Value, DateTime.Now);
            var TeachInfoes = _teach.GetTeachInfoByStudent(null, CurrentUser.Student.Student.StudentNo, ins.ID).ToList();
            ViewBag.TeachInfoes = TeachInfoes;

            ViewBag.HomeWorks = new List<HomeWorkInfo>();
            ViewBag.CurrentTeachNoIndex = false;
            ViewBag.CurrentTimeSheetId = -1;

            if (TeachNo != null)
            {
                if (TimeSheetId != null)
                {
                    ViewBag.HomeWorks = _HomeWork.GetStudentHomeWorkInfoBySheetId(null, (int)TimeSheetId, CurrentUser.Student.Student.StudentNo);
                    ViewBag.CurrentTimeSheetId = TimeSheetId;
                }
                else
                {
                    ViewBag.HomeWorks = _HomeWork.GetStudentHomeWorkInfoByTeachNo(null, TeachNo.ToString(), CurrentUser.Student.Student.StudentNo);
                }
                ViewBag.CurrentTeachNoIndex = TeachInfoes.FindIndex(delegate(TeachInfo p) { return p.Teach.TeachNo == TeachNo.ToString(); });
            }

            return View();
        }

        [ValidateInput(false)]
        public void submithomework(int id, string commits)
        {
            _HomeWork.SubmitHomeWork(null, id, commits);
        }

        public ActionResult GetHomeworkMateiral(int HomworkId)
        {
            JsonCollection ms = new JsonCollection();
            var pms = _HomeWork.GetHomeworkMateiralByHomworkId(null, HomworkId);

            foreach (var pm in pms)
            {
                JsonObject o = new JsonObject();
                o.MergeProperty("id", new JsonConstant(pm.ID));
                o.MergeProperty("name", new JsonConstant(pm.Name));
                o.MergeProperty("downloadurl", new JsonConstant(string.Format("/File/DownloadHomeworkSubmitM?hId={0}", pm.ID)));
                o.MergeProperty("preview", new JsonConstant(true));
                o.MergeProperty("removeurl", new JsonConstant("/File/DeleteHomeworkSubmitM"));
                ms.AppendObject(o);
            }

            return RawJson(ms, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetHomeworkPushMateiral(int HomworkPushId)
        {
            JsonCollection ms = new JsonCollection();
            var pms = _prep.GetHomeworkPushMateiralByHomworkId(null, HomworkPushId);

            foreach (var pm in pms)
            {
                JsonObject o = new JsonObject();
                o.MergeProperty("id", new JsonConstant(pm.ID));
                o.MergeProperty("name", new JsonConstant(pm.Name));
                o.MergeProperty("downloadurl", new JsonConstant(string.Format("/File/DownloadHomeworkPushM?hId={0}", pm.ID)));
                o.MergeProperty("preview", new JsonConstant(true));
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
                            uploadurl: '/File/UploadHomeworkSubmitM?HomworkId={0}',
                            refreshurl: '/SubmitHomeWork/GetHomeworkMateiral?HomworkId={0}',
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
            ", Id, Guid.NewGuid().ToString());
            return Content(function);
        }

        public ActionResult HomeworkPushMFunction(int Id)
        {
            string function = string.Format(@"
                function setHomeworkPushM{0}(){{
                var option = {{
                            uniqueId:'{1}',
                            refreshurl: '/SubmitHomeWork/GetHomeworkPushMateiral?HomworkPushId={0}',
                            autorefresh: false,
                            readonly:true,
                            onPreview: function (row) {{
                                alert('preview' + row.id);
                            }},
                           
                            onError: function (data, status, e) {{
                                alert(e);
                            }}
                        }};
                        $('#HomeworkPushMeteiral{0}').attpool(option);
                }}
            ", Id, Guid.NewGuid().ToString());
            return Content(function);
        }

    }
}
