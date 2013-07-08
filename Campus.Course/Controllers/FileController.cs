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
    public class FileController : BaseController
    {
        private IPreparation s_prep = null;

        public FileController(IPreparation _prep)
        {
            s_prep = _prep;
 
        }

        #region prep meteiral
        //{error:''}
        [HttpPost]
        public ActionResult UploadPrepM(int PrepId)
        {
            JsonObject o = new JsonObject();
            try
            {
                var file = GetFile();
                if(file == null)
                    throw new NullReferenceException("no file get");
                string targetbase = Server.MapPath("../Upload/Preparation");
                PreparationMeteiral pm = new PreparationMeteiral();
                pm.Name = file.FileName;
                pm.PreparationId = PrepId;
                pm.Type = GetType(pm.Name);
                s_prep.SavePrepMateiral(null, pm,file,targetbase);
                o.MergeProperty("error", new JsonConstant(null));
            }
            catch (Exception ex)
            {
                o.MergeProperty("error", new JsonConstant(ex.Message));
            }
            return RawJson(o, JsonRequestBehavior.DenyGet);
        }

        public ActionResult DownloadPrepM(int mId)
        {
            var f = s_prep.GetPrepMateiralById(null, mId);
            if(f == null)
                return new EmptyResult();
            string filepath = Path.Combine(Server.MapPath("../Upload/Preparation"), f.PreparationId.ToString(), f.ID.ToString(), f.Name);
            return ReturnFile(filepath, f.Name);
        }

        //{error:''}
        [HttpPost]
        public ActionResult DeletePrepM(int mId)
        {
            JsonObject o = new JsonObject();
            try
            {
                s_prep.DeletePrepMateiralById(null, mId);
                o.MergeProperty("error", new JsonConstant(null));
            }
            catch(Exception ex)
            {
                o.MergeProperty("error", new JsonConstant(ex.Message));
            }
            return RawJson(o, JsonRequestBehavior.DenyGet);
        }

        #endregion

        #region homeworkpush meteiral

        //{error:''}
        [HttpPost]
        public ActionResult UploadHomeworkM(int HomworkId)
        {
            JsonObject o = new JsonObject();
            try
            {
                var file = GetFile();
                if (file == null)
                    throw new NullReferenceException("no file get");
                string targetbase = Server.MapPath("../Upload/HomeworkPush");
                HomeWorkPushMeteiral hm = new HomeWorkPushMeteiral();
                hm.Name = file.FileName;
                hm.HomeworkPushId = HomworkId;
                hm.Type = GetType(hm.Name);
                s_prep.SaveHomeworkPushMateiral(null, hm, file, targetbase);
                o.MergeProperty("error", new JsonConstant(null));
            }
            catch (Exception ex)
            {
                o.MergeProperty("error", new JsonConstant(ex.Message));
            }
            return RawJson(o, JsonRequestBehavior.DenyGet);
        }

        public ActionResult DownloadHomeworkPushM(int hId)
        {
            var f = s_prep.GetHomeworkPushMateiralById(null, hId);
            if (f == null)
                return new EmptyResult();
            string filepath = Path.Combine(Server.MapPath("../Upload/HomeworkPush"), f.HomeworkPushId.ToString(), f.ID.ToString(), f.Name);
            return ReturnFile(filepath, f.Name);
        }

        //{error:''}
        [HttpPost]
        public ActionResult DeleteHomeworkPushM(int mId)
        {
            JsonObject o = new JsonObject();
            try
            {
                s_prep.DeletePrepMateiralById(null, mId);
                o.MergeProperty("error", new JsonConstant(null));
            }
            catch (Exception ex)
            {
                o.MergeProperty("error", new JsonConstant(ex.Message));
            }
            return RawJson(o, JsonRequestBehavior.DenyGet);
        }

        #endregion

        private HttpPostedFileBase GetFile()
        {
            if (Request.Files != null && Request.Files.Count > 0)
            {
                return Request.Files[0];
            }
            else
            {
                return null;
            }
        }

        private string GetType(string fileName)
        {
            return null;
        }

        private ActionResult ReturnFile(string filepath,string filename)
        {
            try
            {
                return File(filepath, "application/octet-stream", filename);
            }
            catch (Exception ex)
            {
                return new ContentResult() {  Content = ex.Message };
            }
            
        }
    }
}
