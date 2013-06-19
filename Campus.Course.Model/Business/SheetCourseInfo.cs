using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Campus.Course.Model.Business
{
    public class SheetCourseInfo
    {
        public int ID { get; set; }

        public DateTime Date { get; set; }

        public double BTime { get; set; }

        public double ETime { get; set; }

        public string Location { get; set; }

        public string CourseName { get; set; }

        public string TeacherName { get; set; }
    }
}
