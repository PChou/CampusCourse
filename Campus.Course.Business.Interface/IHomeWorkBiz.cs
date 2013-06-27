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
    }
}
