using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Campus.Course.Model.Business
{
    //课程表的学期信息
    public class TimeSheetInfo
    {
        public string ClassGradeQ { get; set; }//学期

        public string ClassGrade { get; set; } //学年

        public DateTime? QB { get; set; } //当前学期的开始时间

        public DateTime? QE { get; set; } //当前学期的结束时间
    }
}
