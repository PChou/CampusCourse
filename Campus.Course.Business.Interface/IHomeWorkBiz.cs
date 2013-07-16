using Campus.Course.Model;
using Campus.Course.Model.Business;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace Campus.Course.Business.Interface
{
    public interface IHomeWorkBiz
    {
        IEnumerable<HomeWork> GetHomeWorkByPushId(CampusEntities context, int PushId);
        List<HomeWorkExtend> GetHomeWorkExtendBySheetId(CampusEntities context, int TeachTimeSheetId);
        List<HomeWorkInfo> GetStudentHomeWorkInfoByTeachNo(CampusEntities context, string TeachNo, string StudentNo);
        void ReviewHomeWork(CampusEntities context, int id, string score, string commits);
        void SubmitHomeWork(CampusEntities context, int id, string commits);
        List<HomeWorkInfo> GetHomeWorkInfoBySheetId(CampusEntities context, int TeachTimeSheetId);
        IEnumerable<HomeWorkMeteiral> GetHomeworkMateiralByHomworkId(CampusEntities context, int hId);
        HomeWorkMeteiral SaveHomeworkMateiral(CampusEntities context, HomeWorkMeteiral meteriral, HttpPostedFileBase file, string targetbase);
        HomeWorkMeteiral GetHomeworkMateiralById(CampusEntities context, int hId);
        void DeleteHomeWorkMeteiralById(CampusEntities context, int mId);
    }
}
