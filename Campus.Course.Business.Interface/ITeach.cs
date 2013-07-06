using Campus.Course.Model;
using Campus.Course.Model.Business;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Campus.Course.Business.Interface
{
    public interface ITeach
    {
        IEnumerable<TeachInfo> GetTeachInfoByTeacher(CampusEntities context, string TNo, int InsId);
        IEnumerable<TeachInfo> GetTeachInfoByStudent(CampusEntities context, string SNo, int InsId);
        IEnumerable<Student> GetStudentsByTeachNo(CampusEntities context, string TeachNo);
    }
}
