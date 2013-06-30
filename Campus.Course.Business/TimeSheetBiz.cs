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
    public class TimeSheetBiz : ITimeSheet
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
                    join teach in campus.Teaches on sheet.TeachNo equals teach.TeachNo
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
                        TeacherName = teacher.Name,
                        CourseTheme = course.Theme
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

        public IEnumerable<SheetCourseInfo> GetSheetCourseInfoByClass(CampusEntities context, string ClassNo, DateTime StartDate, DateTime EndDate)
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
                    join teach in campus.Teaches on sheet.TeachNo equals teach.TeachNo
                    join course in campus.Courses on teach.CourseNo equals course.CourseNo
                    join teacher in campus.Teachers on teach.TeacherNo equals teacher.TeacherNo
                    join ct in campus.ClassTeaches on teach.TeachNo equals ct.TeachNo
                    where sheet.Date >= StartDate && sheet.Date <= EndDate && ct.ClassNo == ClassNo
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
                        TeacherName = teacher.Name,
                        CourseTheme = course.Theme
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

        public TimeSheetInfo GetTimeSheetInfo(CampusEntities context, string SNo, DateTime baseDate)
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

            var q = from student in campus.Students
                    join _class in campus.Classes on student.ClassNo equals _class.ClassNo
                    where student.StudentNo == SNo
                    select new
                    {
                        Class = _class
                    };
            try
            {
                var onesc = q.First();
                if (onesc == null)
                    return null;
                TimeSheetInfo si = new TimeSheetInfo();
             
                si.ClassGrade = "假期";
                si.ClassGradeQ = "假期";

                if (baseDate >= onesc.Class.Q1B && baseDate <= onesc.Class.Q1E)
                {
                    si.ClassGrade = onesc.Class.Q1B.Value.Year.ToString() + "-" + onesc.Class.Q1E.Value.Year.ToString() + "学年";
                    si.ClassGradeQ = "第一学期";
                    si.QB = onesc.Class.Q1B;
                    si.QE = onesc.Class.Q1E;
                }
                else if (baseDate >= onesc.Class.Q2B && baseDate <= onesc.Class.Q2E)
                {
                    si.ClassGrade = onesc.Class.Q1B.Value.Year.ToString() + "-" + onesc.Class.Q1E.Value.Year.ToString() + "学年";
                    si.ClassGradeQ = "第二学期";
                    si.QB = onesc.Class.Q2B;
                    si.QE = onesc.Class.Q2E;
                }
                else if (baseDate >= onesc.Class.Q3B && baseDate <= onesc.Class.Q3E)
                {
                    si.ClassGrade = onesc.Class.Q3B.Value.Year.ToString() + "-" + onesc.Class.Q3E.Value.Year.ToString() + "学年";
                    si.ClassGradeQ = "第一学期";
                    si.QB = onesc.Class.Q3B;
                    si.QE = onesc.Class.Q3E;
                }
                else if (baseDate >= onesc.Class.Q4B && baseDate <= onesc.Class.Q4E)
                {
                    si.ClassGrade = onesc.Class.Q3B.Value.Year.ToString() + "-" + onesc.Class.Q3E.Value.Year.ToString() + "学年";
                    si.ClassGradeQ = "第二学期";
                    si.QB = onesc.Class.Q4B;
                    si.QE = onesc.Class.Q4E;
                }
                else if (baseDate >= onesc.Class.Q5B && baseDate <= onesc.Class.Q5E)
                {
                    si.ClassGrade = onesc.Class.Q5B.Value.Year.ToString() + "-" + onesc.Class.Q5E.Value.Year.ToString() + "学年";
                    si.ClassGradeQ = "第一学期";
                    si.QB = onesc.Class.Q5B;
                    si.QE = onesc.Class.Q5E;
                }
                else if (baseDate >= onesc.Class.Q6B && baseDate <= onesc.Class.Q6E)
                {
                    si.ClassGrade = onesc.Class.Q5B.Value.Year.ToString() + "-" + onesc.Class.Q5E.Value.Year.ToString() + "学年";
                    si.ClassGradeQ = "第二学期";
                    si.QB = onesc.Class.Q6B;
                    si.QE = onesc.Class.Q6E;
                }
                else if (baseDate >= onesc.Class.Q7B && baseDate <= onesc.Class.Q7E)
                {
                    si.ClassGrade = onesc.Class.Q7B.Value.Year.ToString() + "-" + onesc.Class.Q7E.Value.Year.ToString() + "学年";
                    si.ClassGradeQ = "第一学期";
                    si.QB = onesc.Class.Q7B;
                    si.QE = onesc.Class.Q7E;
                }
                else if (baseDate >= onesc.Class.Q8B && baseDate <= onesc.Class.Q8E)
                {
                    si.ClassGrade = onesc.Class.Q7B.Value.Year.ToString() + "-" + onesc.Class.Q7E.Value.Year.ToString() + "学年";
                    si.ClassGradeQ = "第二学期";
                    si.QB = onesc.Class.Q8B;
                    si.QE = onesc.Class.Q8E;
                }
                return si;

            }
            finally
            {
                if (context == null)
                    campus.Dispose();
            }
        }

        public IEnumerable<Class> GetClasses(CampusEntities context)
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
                var q = from s in campus.Classes
                        select s;
                return q.ToArray();
            }
            finally
            {
                if (context == null)
                    campus.Dispose();
            }
        }

        public WeekInQGrade CalWeekInQGrade(DateTime BDate,  DateTime showday)
        {
            WeekInQGrade w = new WeekInQGrade();
            int chinaDayOfWeekOffset = BDate.DayOfWeek == DayOfWeek.Sunday ? 6 : (int)BDate.DayOfWeek - 1;
            var startTime = BDate.AddDays(chinaDayOfWeekOffset * -1);
            w.Week = (showday - startTime).Days / 7 + 1;
            w.WeekBDay = startTime.AddDays((w.Week - 1) * 7);
            w.WeekEDay = w.WeekBDay.AddDays(6);
            return w;
        }

        public IEnumerable<SheetCourseInfo> GetTimesheetsByTeachNo(CampusEntities context, string TeachNo)
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
                var q = from s in campus.TeachTimeSheets
                        join prep in campus.Preparations on s.ID equals prep.TeachTimeSheetId into sprep
                        from sp in sprep.DefaultIfEmpty()
                        where s.TeachNo == TeachNo
                        orderby s.BTime
                        select new SheetCourseInfo { 
                            ID = s.ID,
                            PreparationID = sp == null ? -1 : sp.ID,
                            PreparationName = sp == null ? "尚未备课" : sp.PrepName,
                            Date = s.Date
                        };
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
