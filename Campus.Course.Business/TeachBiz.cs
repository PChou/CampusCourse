﻿using Campus.Course.Business.Interface;
using Campus.Course.Model;
using Campus.Course.Model.Business;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Campus.Course.Business
{
    public class TeachBiz : ITeach
    {
        public IEnumerable<TeachInfo> GetTeachInfoByTeacher(CampusEntities context, string TNo, int InsId)
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
                var q = from teach in campus.Teaches
                        join course in campus.Courses on teach.CourseNo equals course.CourseNo
                        where teach.InstituteId == InsId && teach.TeacherNo == TNo
                        select new TeachInfo
                        {
                            Teach = teach,
                            Course = course
                        };

                return q.ToArray();
            }
            finally
            {
                if (context == null)
                    campus.Dispose();
            }
 
        }

        public IEnumerable<TeachInfo> GetTeachInfoByStudent(CampusEntities context, string SNo, int InsId)
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
                var q = from stutea in campus.StudentTeaches
                        join teach in campus.Teaches on stutea.TeachNo equals teach.TeachNo
                        join course in campus.Courses on teach.CourseNo equals course.CourseNo
                        where teach.InstituteId == InsId && stutea.StudentNo == SNo
                        select new TeachInfo
                        {
                            Teach = teach,
                            Course = course
                        };

                return q.ToArray();
            }
            finally
            {
                if (context == null)
                    campus.Dispose();
            }

        }

        public IEnumerable<Student> GetStudentsByTeachNo(CampusEntities context, string TeachNo)
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
                var q = from stutea in campus.StudentTeaches
                        join stud in campus.Students on stutea.StudentNo equals stud.StudentNo
                        where stutea.TeachNo == TeachNo
                        select stud;

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
