using Campus.Course.Model;
using Campus.Course.Model.Business;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Campus.Course.Business.Interface
{
    public interface IHomeWorkBiz
    {
        IEnumerable<HomeWork> GetHomeWorkByPushId(CampusEntities context, int PushId);
        List<HomeWorkExtend> GetHomeWorkExtendByPushId(CampusEntities context, int TeachTimeSheetId);
        List<HomeWorkInfo> GetHomeWorkInfoByTeachNo(CampusEntities context, string TeachNo, string StudentNo);
        void ReviewHomeWork(CampusEntities context, int id, string score, string commits);
        void SubmitHomeWork(CampusEntities context, int id, string commits);
    }
}
