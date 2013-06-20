using Campus.Course.Business.Interface;
using Campus.Course.Model;
using Campus.Course.Model.Business;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace Campus.Course.Business
{
    public class Student : IStudent
    {
        public StudentInfo GetStudentBySNo(CampusEntities context,string SNo,DateTime baseDate)
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
                    select new { 
                        Student = student,
                        Class = _class
                    };
            try
            {
                var onesc = q.First();
                if (onesc == null)
                    return null;
                StudentInfo si = new StudentInfo();
                si.StudentNo = onesc.Student.StudentNo;
                si.Name = onesc.Student.Name;
                si.ClassNo = onesc.Class.ClassNo;
                si.ClassInstitute = onesc.Class.Institute;
                si.ClassSpecialty = onesc.Class.Specialty;

                si.ClassGrade = "假期";
                si.ClassGradeQ = "假期";

                if (baseDate >= onesc.Class.Q1B && baseDate <= onesc.Class.Q1E)
                {
                    si.ClassGrade = "第一学年";
                    si.ClassGradeQ = "第一学期";
                }
                else if (baseDate >= onesc.Class.Q2B && baseDate <= onesc.Class.Q2E)
                {
                    si.ClassGrade = "第一学年";
                    si.ClassGradeQ = "第二学期";
                }
                else if (baseDate >= onesc.Class.Q3B && baseDate <= onesc.Class.Q3E)
                {
                    si.ClassGrade = "第二学年";
                    si.ClassGradeQ = "第一学期";
                }
                else if (baseDate >= onesc.Class.Q4B && baseDate <= onesc.Class.Q4E)
                {
                    si.ClassGrade = "第二学年";
                    si.ClassGradeQ = "第二学期";
                }
                else if (baseDate >= onesc.Class.Q5B && baseDate <= onesc.Class.Q5E)
                {
                    si.ClassGrade = "第三学年";
                    si.ClassGradeQ = "第一学期";
                }
                else if (baseDate >= onesc.Class.Q6B && baseDate <= onesc.Class.Q6E)
                {
                    si.ClassGrade = "第三学年";
                    si.ClassGradeQ = "第二学期";
                }
                else if (baseDate >= onesc.Class.Q7B && baseDate <= onesc.Class.Q7E)
                {
                    si.ClassGrade = "第四学年";
                    si.ClassGradeQ = "第一学期";
                }
                else if (baseDate >= onesc.Class.Q8B && baseDate <= onesc.Class.Q8E)
                {
                    si.ClassGrade = "第四学年";
                    si.ClassGradeQ = "第二学期";
                }
                return si;

            }
            finally
            {
                if (context == null)
                    campus.Dispose();
            }
        }
    }
}
