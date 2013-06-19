using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Configuration;
using System.Data.SqlClient;
using Wicresoft.UnifyShow2.Portal;
using Campus.Course.Business.Interface;

namespace Campus.Course.Controllers
{
    public class TimeSheetController : Controller
    {
        private ITimeSheet s_timesheet;


        private string constr = ConfigurationManager.ConnectionStrings["db"].ConnectionString;


        public TimeSheetController(ITimeSheet _s_timesheet)
        {
            s_timesheet = _s_timesheet;
        }

        public ActionResult Index()
        {
            return View();
        }

        //
        // GET: /TeachTimeSheet/

        public ActionResult GetTimeSheetByTeacher()
        {
            using (SqlConnection conn = new SqlConnection(constr))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(@"SELECT ttt.Date,ttt.BeginTime,ttt.EndTime,c.CourseName from Teach t
INNER JOIN TeachTimeSheet ttt ON t.TeachNo = ttt.TeachNo
INNER JOIN Course c ON t.CourseNo = c.CourseNo
WHERE t.TeacherNo = '00000001'",conn);
                SqlDataReader reader = cmd.ExecuteReader();

                var matrix = new string[12,7];

                while (reader.Read())
                {
                    string course = (string)reader["CourseName"];
                    DateTime dt = (DateTime)reader["Date"];
                    int col = (int)dt.DayOfWeek - 1;
                    if(col == -1) col = 6;
                    List<int> rows = GetRowNumber((double)reader["BeginTime"], (double)reader["EndTime"]);
                    foreach (var r in rows)
                    {
                        matrix[r, col] = course;
                    }
                }

                JsonCollection collection = new JsonCollection();
                for (int i = 0; i < matrix.GetLength(0); i++)
                {
                    JsonCollection c = new JsonCollection();
                    for (int j = 0; j < matrix.GetLength(1); j++)
                    {
                        c.AppendObject(new JsonConstant(matrix[i, j]));
                    }

                    collection.AppendObject(c);
                }

                return new NRemedy.MVC.UI.RawJsonResult() { Data = collection.ToString(),JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }


        }

        //8.0-18.75 step is 0.75
        private List<int> GetRowNumber(double b, double e)
        {
            int seq = 0;
            List<int> cover = new List<int>();
            for (double i = 8.0; i <= 19.5; i += 0.75,seq++)
            {
                if (i >= e)
                    break;
                if (i >= b)
                    cover.Add(seq);
 
            }

            return cover;
        }

    }
}
