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
    }
}
