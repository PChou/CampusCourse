using Campus.Course.Model;
using Campus.Course.Model.Business;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Campus.Course.Business.Interface
{
    public interface ITimeSheet
    {
        IEnumerable<SheetCourseInfo> GetSheetCourseInfoByStudent(CampusEntities context, string StudentNo, DateTime StartDate, DateTime EndDate);
        IEnumerable<SheetCourseInfo> GetSheetCourseInfoByTeacher(CampusEntities context, string StudentNo, DateTime StartDate, DateTime EndDate);
        IEnumerable<SheetCourseInfo> GetSheetCourseInfoByClass(CampusEntities context, string ClassNo, DateTime StartDate, DateTime EndDate);

        TimeSheetInfo GetTimeSheetInfo(CampusEntities context, string SNo, bool IsTeacher,DateTime baseDate);

        IEnumerable<Class> GetClasses(CampusEntities context);

        WeekInQGrade CalWeekInQGrade(DateTime BDate, DateTime showday);

        IEnumerable<SheetCourseInfo> GetTimesheetsByTeachNo(CampusEntities context, string TeachNo);
    }
}
