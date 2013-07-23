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
    public class ReviewHomeWorkController : BaseController
    {
        private ITeach _teach = null;
        private IHomeWorkBiz _HomeWork = null;
        private IStudent _Student = null;

        public ReviewHomeWorkController(ITeach __teach, IHomeWorkBiz __HomeWork, IStudent __Student)
        {
            _teach = __teach;
            _HomeWork = __HomeWork;
            _Student = __Student;
        }

        public ActionResult Index(string TeachNo, string SudentNo, string CurrentHomeWorkId = null)
        {
            if (CurrentUser.IsStudent)
                throw new Exception("The current user is not teacher.permission denied.");
            List<HomeWorkInfo> list = _HomeWork.GetStudentHomeWorkInfoByTeachNo(null, TeachNo, SudentNo);
            List<Student> students = _teach.GetStudentsByTeachNo(null, TeachNo).ToList();
            int homeworkid = -1;
            int.TryParse(CurrentHomeWorkId, out homeworkid);
            int index = list.FindIndex(delegate(HomeWorkInfo p) { return p.HomeWork.ID == homeworkid; });
            ViewBag.TeachNo = TeachNo;
            ViewBag.HomeWorks = list;
            ViewBag.Students = students;
            ViewBag.CurrentStudentNo = SudentNo;
            ViewBag.CurrentHomeworkId = CurrentHomeWorkId;
            ViewBag.CurrentHomeworkIndex = index > 0 ? index : 0;

            return View();
        }

        public void ReviewHomeWork(int id, string score, string commits)
        {
            _HomeWork.ReviewHomeWork(null, id, score, commits);
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
                            refreshurl: '/ReviewHomeWork/GetHomeworkMateiral?HomworkId={0}',
                            autorefresh: false,
                            readonly:true,
                            onPreview: function (row) {{
                                alert('preview' + row.id);
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
    }
}
