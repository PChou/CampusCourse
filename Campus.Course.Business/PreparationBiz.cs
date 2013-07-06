using Campus.Course.Business.Interface;
using Campus.Course.Model;
using Campus.Course.Model.Business;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.IO;
using System.Linq;
using System.Text;
using System.Transactions;
using System.Web;

namespace Campus.Course.Business
{
    public class PreparationBiz : IPreparation
    {
        public Course.Model.Preparation GetPreparationByPId(CampusEntities context,int PId)
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
                //        join met in campus.PreparationMeteirals on prep.ID equals met.PreparationId
                //        into prepjmet
                //        from pt in prepjmet.DefaultIfEmpty()
                //        where prep.ID == PId
                //        select new PreparationInfo { 
                //            PrepId = prep.ID,
                //            PrepName = prep.PrepName,
                //            PrepContent = prep.PrepContent,
                //            MeteiralId = pt == null ? 0 : pt.ID,
                //            MeteiralName = pt == null ? null : pt.Name,
                //            MeteiralPath = pt == null ? null : pt.PhysicalStore,
                //            MeteiralType = pt == null ? null : pt.Type,
                //        };
                var q = from prep in campus.Preparations
                        where prep.ID == PId
                        select prep;
                return q.FirstOrDefault();
                
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

        public PreparationMeteiral SavePrepMateiral(CampusEntities context, PreparationMeteiral preparation,HttpPostedFileBase file,string targetbase)
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
            //TODO Transaction support
            //TransactionScope scope = null;
            try
            {
                //scope = new TransactionScope();
                campus.PreparationMeteirals.Add(preparation);
                campus.SaveChanges();

                int Id = preparation.ID;
                string path = Path.Combine(targetbase, preparation.PreparationId.ToString(), Id.ToString());
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                string target = Path.Combine(path, file.FileName);
                if (File.Exists(target))
                {
                    throw new Exception("Duplicate file " + file.FileName);
                }
                else
                {
                    file.SaveAs(target);
                }

                //scope.Complete();
                return preparation;
            }
            catch(Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (context == null)
                    campus.Dispose();
            }
        }

        public PreparationMeteiral GetPrepMateiralById(CampusEntities context, int mId)
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
                var q = from m in campus.PreparationMeteirals
                        where m.ID == mId
                        select m;
                return q.FirstOrDefault();

            }
            finally
            {
                if (context == null)
                    campus.Dispose();
            }
        }

        public IEnumerable<PreparationMeteiral> GetPrepMateiralByPrepId(CampusEntities context, int PrepId)
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
                var q = from m in campus.PreparationMeteirals
                        where m.PreparationId == PrepId
                        select m;
                return q.ToArray();
            }
            finally
            {
                if (context == null)
                    campus.Dispose();
            }
        }


        public IEnumerable<HomeWorkPushMeteiral> GetHomeworkPushMateiralByHomworkId(CampusEntities context, int hId)
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
                var q = from m in campus.HomeWorkPushMeteirals
                        where m.HomeworkPushId == hId
                        select m;
                return q.ToArray();
            }
            finally
            {
                if (context == null)
                    campus.Dispose();
            }
        }

        public void DeletePrepMateiralById(CampusEntities context, int mId)
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
                var q = from m in campus.PreparationMeteirals
                        where m.ID == mId
                        select m;
                campus.PreparationMeteirals.Remove(q.First());
                campus.SaveChanges();

            }
            finally
            {
                if (context == null)
                    campus.Dispose();
            }
        }


        public HomeWorkPushMeteiral SaveHomeworkPushMateiral(CampusEntities context, HomeWorkPushMeteiral preparation, HttpPostedFileBase file, string targetbase)
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
            //TODO Transaction support
            //TransactionScope scope = null;
            try
            {
                //scope = new TransactionScope();
                campus.HomeWorkPushMeteirals.Add(preparation);
                campus.SaveChanges();

                int Id = preparation.ID;
                string path = Path.Combine(targetbase, preparation.HomeworkPushId.ToString(), Id.ToString());
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                string target = Path.Combine(path, file.FileName);
                if (File.Exists(target))
                {
                    throw new Exception("Duplicate file " + file.FileName);
                }
                else
                {
                    file.SaveAs(target);
                }

                //scope.Complete();
                return preparation;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (context == null)
                    campus.Dispose();
            }
        }

        public HomeWorkPushMeteiral GetHomeworkPushMateiralById(CampusEntities context, int hId)
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
                var q = from m in campus.HomeWorkPushMeteirals
                        where m.ID == hId
                        select m;
                return q.FirstOrDefault();

            }
            finally
            {
                if (context == null)
                    campus.Dispose();
            }
        }
    }
}
