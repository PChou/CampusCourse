using Campus.Course.Business.Interface;
using Campus.Course.Model;
using Campus.Course.Model.Business;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace Campus.Course.Business
{
    public class HomeWorkBiz : IHomeWorkBiz
    {
        public IEnumerable<HomeWork> GetHomeWorkByPushId(CampusEntities context, int PushId)
        {
             CampusEntities campus = null;
            if (context == null)
            {
                campus = new CampusEntities();
            }
            else
            {
                campus = context;
            }

            var q = from work in campus.HomeWorks
                    where work.HomeWorkPushID == PushId
                    select work;
            try
            {
                return q.ToArray();
            }
            finally
            {
                if (context == null)
                    campus.Dispose();
            }
        }

        public List<HomeWorkExtend> GetHomeWorkExtendByPushId(CampusEntities context, int TeachTimeSheetId)
        {
            List<HomeWorkExtend> result = new List<HomeWorkExtend>();
            CampusEntities campus = null;
            if (context == null)
            {
                campus = new CampusEntities();
            }
            else
            {
                campus = context;
            }
            var q = from push in campus.HomeWorkPushes
                    where push.TeachTimeSheetId == TeachTimeSheetId
                    select push;
            List<HomeWorkPush> pushs = q.ToList();
            foreach (var item in pushs)
            {
                HomeWorkExtend extend = new HomeWorkExtend();
                var w = from work in campus.HomeWorks
                        where work.HomeWorkPushID == item.ID
                        select work;
                extend.HomeWorkPush = item;
                extend.HomeWorkList = w.ToList();
                result.Add(extend);
            }
            return result;
        }

        public List<HomeWorkInfo> GetHomeWorkInfoByTeachNo(CampusEntities context, string TeachNo, string StudentNo)
        {
            CampusEntities campus = null;
            if (context == null)
            {
                campus = new CampusEntities();
            }
            else
            {
                campus = context;
            }
            var q = from work in campus.HomeWorks
                    join push in campus.HomeWorkPushes on work.HomeWorkPushID equals push.ID
                    where work.TeachNo == TeachNo && work.StudentNo == StudentNo
                    select new HomeWorkInfo { HomeWork = work, HomeWorkPush = push };
            return q.ToList();
        }

        public void ReviewHomeWork(CampusEntities context, int id, string score, string commits)
        {
            CampusEntities campus = null;
            if (context == null)
            {
                campus = new CampusEntities();
            }
            else
            {
                campus = context;
            }
            var review = campus.HomeWorks.First(p => p.ID == id);
            review.Score = score;
            review.TeacherCommits = commits;
            campus.HomeWorks.Attach(review);
            campus.Entry(review).State = EntityState.Modified;
            campus.SaveChanges();
        }

        public void SubmitHomeWork(CampusEntities context, int id, string commits)
        {
            CampusEntities campus = null;
            if (context == null)
            {
                campus = new CampusEntities();
            }
            else
            {
                campus = context;
            }
            var review = campus.HomeWorks.First(p => p.ID == id);
            review.Commits = commits;
            campus.HomeWorks.Attach(review);
            campus.Entry(review).State = EntityState.Modified;
            campus.SaveChanges();
        }
    }
}
