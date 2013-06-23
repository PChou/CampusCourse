using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Campus.Course.Business.Interface;
using Campus.Course.Model;
using Campus.Course.Model.Business;


namespace Campus.Course.Business
{
    public class Login : ILogin
    {
        public bool CheckLogin(CampusEntities context, string user, string password, bool isStudent)
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
            if (isStudent)
            {
                var q = from student in campus.Students
                        where student.StudentNo == user
                        select student;
                return q.Count() > 0;
            }
            else
            {
                var q = from teacher in campus.Teachers
                        where teacher.TeacherNo == user
                        select teacher;
                return q.Count() > 0;
            }
        }

        public V_CurrentUser GetCurrentUser(CampusEntities context, string userno, bool isStudent)
        {
            V_CurrentUser v = new V_CurrentUser();
            v.IsStudent = isStudent;
            CampusEntities campus = null;
            if (context == null)
            {
                campus = new CampusEntities();
            }
            else
            {
                campus = context;
            }
            if (isStudent)
            {
                var q = from student in campus.Students
                        join _class in campus.Classes on student.ClassNo equals _class.ClassNo
                        where student.StudentNo == userno
                        select new
                        {
                            Student = student,
                            Class = _class
                        };
                V_CurrentStudent vs = new V_CurrentStudent();
                vs.Student = q.First().Student;
                vs.Class = q.First().Class;
                v.Student = vs;
                v.Name = q.First().Student.Name;
                v.UserNo = q.First().Student.StudentNo;
            }
            else
            {
                var q = from teacher in campus.Teachers
                        where teacher.TeacherNo == userno
                        select teacher;
                V_CurrentTeacher vt = new V_CurrentTeacher();
                vt.Teacher = q.First();
                v.Teacher = vt;
                v.Name = q.First().Name;
                v.UserNo = q.First().TeacherNo;
            }
            return v;
        }
    }
}
