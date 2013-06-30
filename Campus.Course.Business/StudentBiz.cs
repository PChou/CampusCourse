using Campus.Course.Business.Interface;
using Campus.Course.Model;
using Campus.Course.Model.Business;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace Campus.Course.Business
{
    public class StudentBiz : IStudent
    {
        public Student GetStudentBySNo(CampusEntities context, string SNo)
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
            return campus.Students.FirstOrDefault(p => p.StudentNo == SNo);
        }

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
                    si.ClassGrade = onesc.Class.Q1B.Value.Year.ToString() + "-" + onesc.Class.Q1E.Value.Year.ToString() + "学年";
                    si.ClassGradeQ = "第一学期";
                }
                else if (baseDate >= onesc.Class.Q2B && baseDate <= onesc.Class.Q2E)
                {
                    si.ClassGrade = onesc.Class.Q1B.Value.Year.ToString() + "-" + onesc.Class.Q1E.Value.Year.ToString() + "学年";
                    si.ClassGradeQ = "第二学期";
                }
                else if (baseDate >= onesc.Class.Q3B && baseDate <= onesc.Class.Q3E)
                {
                    si.ClassGrade = onesc.Class.Q3B.Value.Year.ToString() + "-" + onesc.Class.Q3E.Value.Year.ToString() + "学年";
                    si.ClassGradeQ = "第一学期";
                }
                else if (baseDate >= onesc.Class.Q4B && baseDate <= onesc.Class.Q4E)
                {
                    si.ClassGrade = onesc.Class.Q3B.Value.Year.ToString() + "-" + onesc.Class.Q3E.Value.Year.ToString() + "学年";
                    si.ClassGradeQ = "第二学期";
                }
                else if (baseDate >= onesc.Class.Q5B && baseDate <= onesc.Class.Q5E)
                {
                    si.ClassGrade = onesc.Class.Q5B.Value.Year.ToString() + "-" + onesc.Class.Q5E.Value.Year.ToString() + "学年";
                    si.ClassGradeQ = "第一学期";
                }
                else if (baseDate >= onesc.Class.Q6B && baseDate <= onesc.Class.Q6E)
                {
                    si.ClassGrade = onesc.Class.Q5B.Value.Year.ToString() + "-" + onesc.Class.Q5E.Value.Year.ToString() + "学年";
                    si.ClassGradeQ = "第二学期";
                }
                else if (baseDate >= onesc.Class.Q7B && baseDate <= onesc.Class.Q7E)
                {
                    si.ClassGrade = onesc.Class.Q7B.Value.Year.ToString() + "-" + onesc.Class.Q7E.Value.Year.ToString() + "学年";
                    si.ClassGradeQ = "第一学期";
                }
                else if (baseDate >= onesc.Class.Q8B && baseDate <= onesc.Class.Q8E)
                {
                    si.ClassGrade = onesc.Class.Q7B.Value.Year.ToString() + "-" + onesc.Class.Q7E.Value.Year.ToString() + "学年";
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

        public InstituteSheet GetInstituteInfoByStudent(CampusEntities context, string SNo, DateTime? showday)
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
                DateTime d = DateTime.Now;
                if (showday != null)
                    d = showday.Value;

                var q = from stud in campus.Students
                        join clas in campus.Classes on stud.ClassNo equals clas.ClassNo
                        join ins in campus.InstituteSheets on clas.Institute equals ins.Name
                        where ins.BDate <= d && ins.EDate >= d && stud.StudentNo == SNo
                        select ins;
                return q.FirstOrDefault();
            }
            finally
            {
                if (context == null)
                    campus.Dispose();
            }
        }
    }
}
