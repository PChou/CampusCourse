//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Campus.Course.Model
{
    using System;
    using System.Collections.Generic;
    
    public partial class HomeWork
    {
        public int ID { get; set; }
        public Nullable<int> HomeWorkPushID { get; set; }
        public string StudentNo { get; set; }
        public string TeachNo { get; set; }
        public Nullable<System.DateTime> PushDate { get; set; }
        public Nullable<System.DateTime> SubmitDate { get; set; }
        public Nullable<System.DateTime> ReviewDate { get; set; }
        public string Status { get; set; }
        public string Score { get; set; }
        public string Group { get; set; }
        public string Commits { get; set; }
        public string TeacherCommits { get; set; }
        public Nullable<System.DateTime> TeachTimeSheetDate { get; set; }
    }
}
