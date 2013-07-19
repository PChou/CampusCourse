using Campus.Course.Business.Interface;
using Campus.Course.Model;
using Campus.Course.Model.Business;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Campus.Course.Utility;
using System.Web;
using System.IO;

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
            DateTime now = DateTime.Now;
            var insti = from stud in campus.Students
                        join clas in campus.Classes on stud.ClassNo equals clas.ClassNo
                        join ins in campus.InstituteSheets on clas.Institute equals ins.Name
                        where ins.BDate <= now && ins.EDate >= now && stud.StudentNo == StudentNo
                        select ins;
            var bdate = insti.First().BDate;
            foreach (var item in result)
            {
                WeekInQGrade week = biz.CalWeekInQGrade((DateTime)bdate, (DateTime)item.HomeWorkPush.TeachTimeSheetDate);
                item.WeekInQGrade = week;
                //var pushmetiral = from m in campus.HomeWorkPushMeteirals
                //                  where m.HomeworkPushId == item.HomeWorkPush.ID
                //                  select m;
                //item.HomeWorkPushMeteiralList = pushmetiral.ToList();
            }
            return result;
        }

        public List<HomeWorkInfo> GetStudentHomeWorkInfoBySheetId(CampusEntities context, int TeachTimeSheetId, string StudentNo)
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
                    where push.TeachTimeSheetId == TeachTimeSheetId && work.StudentNo == StudentNo
                    orderby work.PushDate descending
                    select new HomeWorkInfo { HomeWork = work, HomeWorkPush = push };
            List<HomeWorkInfo> result = q.ToList();
            TimeSheetBiz biz = new TimeSheetBiz();
            DateTime now = DateTime.Now;
            var insti = from stud in campus.Students
                    join clas in campus.Classes on stud.ClassNo equals clas.ClassNo
                    join ins in campus.InstituteSheets on clas.Institute equals ins.Name
                    where ins.BDate <= now && ins.EDate >= now && stud.StudentNo == StudentNo
                    select ins;
            var bdate = insti.First().BDate;
            foreach (var item in result)
            {
                WeekInQGrade week = biz.CalWeekInQGrade((DateTime)bdate, (DateTime)item.HomeWorkPush.TeachTimeSheetDate);
                item.WeekInQGrade = week;
                //var pushmetiral = from m in campus.HomeWorkPushMeteirals
                //                  where m.HomeworkPushId == item.HomeWorkPush.ID
                //                  select m;
                //item.HomeWorkPushMeteiralList = pushmetiral.ToList();
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



        public IEnumerable<HomeWorkMeteiral> GetHomeworkMateiralByHomworkId(CampusEntities context, int hId)
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
            try
            {
                var q = from m in campus.HomeWorkMeteirals
                        where m.HomeworkId == hId
                        select m;
                return q.ToArray();
            }
            finally
            {
                if (context == null)
                    campus.Dispose();
            }
        }

        public HomeWorkMeteiral SaveHomeworkMateiral(CampusEntities context, HomeWorkMeteiral meteriral, HttpPostedFileBase file, string targetbase)
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
            //TODO Transaction support
            //TransactionScope scope = null;
            try
            {
                //scope = new TransactionScope();
                campus.HomeWorkMeteirals.Add(meteriral);
                campus.SaveChanges();

                int Id = meteriral.ID;
                string path = Path.Combine(targetbase, meteriral.HomeworkId.ToString(), Id.ToString());
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                string target = Path.Combine(path, file.FileName);
                if (File.Exists(target))
                {
                    throw new Exception("Duplicate file " + file.FileName);
                }
                else
                {
                    file.SaveAs(target);
                }

                //scope.Complete();
                return meteriral;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (context == null)
                    campus.Dispose();
            }
        }

        public HomeWorkMeteiral GetHomeworkMateiralById(CampusEntities context, int hId)
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
            try
            {
                var q = from m in campus.HomeWorkMeteirals
                        where m.ID == hId
                        select m;
                return q.FirstOrDefault();

            }
            finally
            {
                if (context == null)
                    campus.Dispose();
            }
        }

        public void DeleteHomeWorkMeteiralById(CampusEntities context, int mId)
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
            try
            {
                var q = from m in campus.HomeWorkMeteirals
                        where m.ID == mId
                        select m;
                campus.HomeWorkMeteirals.Remove(q.First());
                campus.SaveChanges();

            }
            finally
            {
                if (context == null)
                    campus.Dispose();
            }
        }
    }
}
