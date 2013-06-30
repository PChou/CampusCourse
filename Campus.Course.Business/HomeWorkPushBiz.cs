using Campus.Course.Business.Interface;
using Campus.Course.Model;
using Campus.Course.Model.Business;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Campus.Course.Business
{
    public class HomeWorkPushBiz : IHomeWorkPushBiz
    {
        public IEnumerable<HomeWorkPush> GetHomeWorkPushByWorkSheetId(CampusEntities context, int TeachTimeSheetId)
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

            var q = from push in campus.HomeWorkPushes
                    where push.TeachTimeSheetId == TeachTimeSheetId
                    select push;

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

        protected int CreateHomeworkPush(CampusEntities context, HomeWorkPush hwp)
        {
            if (context == null)
                throw new ArgumentNullException("context");
            if(hwp == null)
                throw new ArgumentNullException("context");
            return context.sp_PublishHomework(hwp.TeachTimeSheetId, hwp.DeadLine, hwp.TeachNo, hwp.Subject, hwp.Description, hwp.Notice, hwp.Evaluation, hwp.HtmlContent);
        }

        public HomeWorkPush SaveHomeworkPush(CampusEntities context, HomeWorkPush hwp)
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
                if (hwp.ID < 1) //create new
                {
                    int Id = CreateHomeworkPush(campus, hwp);
                    if (Id < 0)
                        throw new Exception("create entry failed.");
                    hwp.ID = Id;
                    return hwp;
                }
                else//update
                {
                    var q = from h in campus.HomeWorkPushes
                            where h.ID == hwp.ID
                            select h;
                    var target = q.First();
                    target.DeadLine = hwp.DeadLine;
                    target.Description = hwp.Description;
                    target.Evaluation = hwp.Evaluation;
                    target.HtmlContent = hwp.HtmlContent;
                    target.Notice = hwp.Notice;
                    target.Subject = hwp.Subject;
                    target.TeachNo = hwp.TeachNo;
                    target.TeachTimeSheetId = hwp.TeachTimeSheetId;
                    campus.SaveChanges();
                    return target;
                }
            }
            finally
            {
                if (context == null)
                    campus.Dispose();
            }
        }
    }
}
