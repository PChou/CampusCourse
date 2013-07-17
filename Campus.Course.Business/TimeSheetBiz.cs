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

        public IEnumerable<SheetCourseInfo> GetSheetCourseInfoByTeacher(CampusEntities context, string TeacherNo, DateTime StartDate, DateTime EndDate)
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
                    where sheet.Date >= StartDate && sheet.Date <= EndDate && teacher.TeacherNo == TeacherNo
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

        public TimeSheetInfo GetTimeSheetInfo(CampusEntities context, string SNo, bool IsTeacher, DateTime baseDate)
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
                IQueryable<TimeSheetInfo> q;
                if (IsTeacher)
                {
                    q = from insti in campus.InstituteSheets
                        join teacher in campus.Teachers on insti.Name equals teacher.Institute
                        where teacher.TeacherNo == SNo && insti.BDate <= baseDate && insti.EDate >= baseDate
                        select new TimeSheetInfo
                        {
                            ClassGrade = insti.Grade,
                            ClassGradeQ = insti.QGrade,
                            QB = insti.BDate,
                            QE = insti.EDate
                        };
                }
                else
                {
                    q = from insti in campus.InstituteSheets
                        join c in campus.Classes on insti.Name equals c.Institute
                        join student in campus.Students on c.ClassNo equals student.ClassNo
                        where student.StudentNo == SNo && insti.BDate <= baseDate && insti.EDate >= baseDate
                        select new TimeSheetInfo
                        {
                            ClassGrade = insti.Grade,
                            ClassGradeQ = insti.QGrade,
                            QB = insti.BDate,
                            QE = insti.EDate
                        };
                }


                return q.First();

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
