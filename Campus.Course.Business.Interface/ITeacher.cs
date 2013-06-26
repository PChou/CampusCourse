using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Campus.Course.Model;

namespace Campus.Course.Business.Interface
{
    public interface ITeacher
    {
        InstituteSheet GetInstituteInfoByTeacher(CampusEntities context, string TNo, DateTime? showday);
    }
}
