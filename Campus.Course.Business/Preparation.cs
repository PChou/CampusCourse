using Campus.Course.Business.Interface;
using Campus.Course.Model;
using Campus.Course.Model.Business;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Campus.Course.Business
{
    public class Preparation : IPreparation
    {
        public IEnumerable<PreparationInfo> GetPreparationByPId(CampusEntities context,int PId)
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
                var q = from prep in campus.Preparations
                        join met in campus.PreparationMeteirals on prep.ID equals met.PreparationId
                        into prepjmet
                        from pt in prepjmet.DefaultIfEmpty()
                        where prep.ID == PId
                        select new PreparationInfo { 
                            PrepId = prep.ID,
                            PrepName = prep.PrepName,
                            PrepContent = prep.PrepContent,
                            MeteiralId = pt == null ? 0 : pt.ID,
                            MeteiralName = pt == null ? null : pt.Name,
                            MeteiralPath = pt == null ? null : pt.PhysicalStore,
                            MeteiralType = pt == null ? null : pt.Type,
                        };
                return q.ToArray();
                
            }
            finally
            {
                if (context == null)
                    campus.Dispose();
            }
        }


        public Model.Preparation SavePreparation(CampusEntities context, Model.Preparation preparation)
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
                if (preparation.ID == 0)//create
                {
                    campus.Preparations.Add(preparation);
                    campus.SaveChanges();
                    return preparation;
                }
                else
                {
                    var q = from prep in campus.Preparations
                            where prep.ID == preparation.ID
                            select prep;
                    var target = q.First();
                    target.PrepContent = preparation.PrepContent;
                    target.PrepName = preparation.PrepName;
                    campus.SaveChanges();
                    return target;
 
                }


            }
            finally
            {
                if (context == null)
                    campus.Dispose();
            }
        }
    }
}
