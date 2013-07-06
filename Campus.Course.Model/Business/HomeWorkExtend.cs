using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Campus.Course.Model.Business
{
    public class HomeWorkExtend
    {
        public HomeWorkPush HomeWorkPush { get; set; }
        public int AllCount { get; set; }
        public int NewCount { get; set; }
        public int SubmitCount { get; set; }
        public int LateCount { get; set; }
        public List<HomeWork> HomeWorkList { get; set; }
    }
}
