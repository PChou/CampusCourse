using Campus.Course.Business.Interface;
using Campus.Course.Model;
using Campus.Course.Model.Business;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Campus.Course.Business
{
    public class HomeWorkBiz
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
    }
}
