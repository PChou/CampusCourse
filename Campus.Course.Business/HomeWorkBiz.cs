using Campus.Course.Business.Interface;
using Campus.Course.Model;
using Campus.Course.Model.Business;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Campus.Course.Utility;

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

        public List<HomeWorkExtend> GetHomeWorkExtendBySheetId(CampusEntities context, int TeachTimeSheetId)
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
                extend.AllCount = w.Count();
                extend.NewCount = w.Where(p => p.Status == HomeWorkStatus.New || p.Status == null).Count();
                extend.SubmitCount = w.Where(p => p.Status == HomeWorkStatus.Submit ).Count();
                extend.LateCount = w.Where(p => p.Status == HomeWorkStatus.Late).Count();

                result.Add(extend);
            }
            return result;
        }

        public List<HomeWorkInfo> GetStudentHomeWorkInfoByTeachNo(CampusEntities context, string TeachNo, string StudentNo)
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
                    orderby work.PushDate descending
                    select new HomeWorkInfo { HomeWork = work, HomeWorkPush = push };
            List<HomeWorkInfo> result = q.ToList();
            TimeSheetBiz biz = new TimeSheetBiz();
            foreach (var item in result)
            { 
                var insti = from ins in campus.InstituteSheets
                            join teach in campus.Teaches on ins.ID equals teach.InstituteId
                            where teach.TeachNo == item.HomeWork.TeachNo
                            select ins;
                var bdate = insti.First().BDate;
                WeekInQGrade week = biz.CalWeekInQGrade((DateTime)bdate, (DateTime)item.HomeWorkPush.PushDate);
                item.WeekInQGrade = week;
            }
            return result;
        }

        public List<HomeWorkInfo> GetHomeWorkInfoBySheetId(CampusEntities context, int TeachTimeSheetId)
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
                    where push.TeachTimeSheetId == TeachTimeSheetId
                    orderby work.PushDate descending
                    select new HomeWorkInfo { HomeWork = work, HomeWorkPush = push };
            List<HomeWorkInfo> result = q.ToList();
            TimeSheetBiz biz = new TimeSheetBiz();
            foreach (var item in result)
            {
                var insti = from ins in campus.InstituteSheets
                            join teach in campus.Teaches on ins.ID equals teach.InstituteId
                            where teach.TeachNo == item.HomeWork.TeachNo
                            select ins;
                var bdate = insti.First().BDate;
                WeekInQGrade week = biz.CalWeekInQGrade((DateTime)bdate, (DateTime)item.HomeWorkPush.PushDate);
                item.WeekInQGrade = week;
            }
            return result;
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
            review.ReviewDate = System.DateTime.Now;
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
            var homework = campus.HomeWorks.First(p => p.ID == id);
            var push = campus.HomeWorkPushes.First(p => p.ID == homework.HomeWorkPushID);
            if (push.DeadLine != null && (DateTime)push.DeadLine < DateTime.Now)
            {
                homework.Status = HomeWorkStatus.Late;
            }
            else
            {
                homework.Status = HomeWorkStatus.Submit;
            }
            homework.Commits = commits;

            homework.SubmitDate = System.DateTime.Now;
            campus.HomeWorks.Attach(homework);
            campus.Entry(homework).State = EntityState.Modified;
            campus.SaveChanges();
        }
    }
}
