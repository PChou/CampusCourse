using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Campus.Course.Model.Business
{
    public class V_CurrentUser
    {
        public bool IsStudent { get; set; }
        public string Name { get; set; }
        public string UserNo { get; set; }
        public V_CurrentStudent Student { get; set; }
        public V_CurrentTeacher Teacher { get; set; }
    }

    public class V_CurrentStudent
    {
        public Student Student { get; set; }
        public Class Class { get; set; }
    }

    public class V_CurrentTeacher
    {
        public Teacher Teacher { get; set; }
    }
}