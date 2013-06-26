using Campus.Course.Business.Interface;
using Campus.Course.Model;
using Campus.Course.Model.Business;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Campus.Course.Business
{
    public class Teach : ITeach
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
    }
}
