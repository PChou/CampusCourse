using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Campus.Course.Model.Business
{
    public class StudentInfo
    {
        public string Name { get;set; }

        public string StudentNo { get; set; }

        public string ClassNo { get;set; }

        public string ClassSpecialty { get; set; }//专业

        public string ClassGrade { get; set; } //学年

        public string ClassInstitute { get; set; }

        public string ClassGradeQ { get; set; }//学期
    }
}
