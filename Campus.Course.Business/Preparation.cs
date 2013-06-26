using Campus.Course.Business.Interface;
using Campus.Course.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Campus.Course.Business
{
    public class Preparation : IPreparation
    {
        public void GetPreparationByPId(CampusEntities context,int PId)
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
               //var q = from prep in campus.Preparations
               //        join 
            }
            finally
            {
                if (context == null)
                    campus.Dispose();
            }
        }
    }
}
