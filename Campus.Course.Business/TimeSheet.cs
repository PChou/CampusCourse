using Campus.Course.Business.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Campus.Course.Model;
using System.Data.Entity;
using Campus.Course.Model.Business;

namespace Campus.Course.Business
{
    public class TimeSheet : ITimeSheet
    {
        public IEnumerable<SheetCourseInfo> GetSheetCourseInfoByStudent(CampusEntities context,string StudentNo, DateTime StartDate, DateTime EndDate)
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

            var q = from sheet in campus.TeachTimeSheets
                    join teach in campus.Teaches on sheet.TeachNo equals teach.TeacherNo
                    join course in campus.Courses on teach.CourseNo equals course.CourseNo
                    join teacher in campus.Teachers on teach.TeacherNo equals teacher.TeacherNo
                    join st in campus.StudentTeaches on teach.TeachNo equals st.TeachNo
                    join student in campus.Students on st.StudentNo equals student.StudentNo
                    where sheet.Date >= StartDate && sheet.Date <= EndDate && student.StudentNo == StudentNo
                    orderby sheet.Date ascending
                    orderby sheet.BTime ascending
                    select new SheetCourseInfo()
                    {
                        ID = sheet.ID,
                        Date = sheet.Date,
                        BTime = sheet.BTime,
                        ETime = sheet.ETime,
                        Location = sheet.Location,
                        CourseName = course.Name,
                        TeacherName = teacher.Name

                    };
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
