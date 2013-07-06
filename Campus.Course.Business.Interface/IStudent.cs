using Campus.Course.Model;
using Campus.Course.Model.Business;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Campus.Course.Business.Interface
{
    public interface IStudent
    {
        Student GetStudentBySNo(CampusEntities context, string SNo);
        StudentInfo GetStudentBySNo(CampusEntities context, string SNo, DateTime baseDate);
        InstituteSheet GetInstituteInfoByStudent(CampusEntities context, string SNo, DateTime? showday);
    }
}
