using Campus.Course.Business.Interface;
using Campus.Course.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Campus.Course.Business
{
    public class Teacher : ITeacher
    {
        public InstituteSheet GetInstituteInfoByTeacher(CampusEntities context, string TNo, DateTime? showday)
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

                var q = from teacher in campus.Teachers
                        join ins in campus.InstituteSheets on teacher.Institute equals ins.Name
                        where ins.BDate <= d && ins.EDate >= d && teacher.TeacherNo == TNo
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
