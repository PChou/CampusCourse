using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Campus.Course.Model.Business
{
    /// <summary>
    /// 描述某天在某个学年和学期中的周次
    /// </summary>
    public class WeekInQGrade
    {
        public int Week { get; set; }

        public DateTime WeekBDay { get; set; }

        public DateTime WeekEDay { get; set; }
    }
}
