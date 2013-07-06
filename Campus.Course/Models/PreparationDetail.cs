using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Campus.Course
{
    public class PreparationDetailViewModel
    {
        public int  PreparationId { get; set; }

        public string PreparationName { get; set; }

        public string PreparationContent { get; set; }

        public string TeachNo { get; set; }

        public int SheetId { get; set; }

        public List<HomeworkViewModel> HomeworkPushes { get; set; }

        public PreparationDetailViewModel()
        {
            HomeworkPushes = new List<HomeworkViewModel>();
        }

    }

    public class HomeworkViewModel
    {
        public int? HomeworkId { get; set; }

        public string Subject { get; set; }

        public string Description { get; set; }

        public string Evaluation { get; set; }

        public DateTime? DeadLine { get; set; }

        public DateTime? PushDate { get; set; }
 
    }

    public class HomeworkPushSubmitModel : HomeworkViewModel
    {
        public int PreparationId { get; set; }

        public string TeachNo { get; set; }

        public int SheetId { get; set; }
    }

}