﻿using Campus.Course.Model;
using Campus.Course.Model.Business;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;

namespace Campus.Course.Business.Interface
{
    public interface IPreparation
    {
        IEnumerable<PreparationInfo> GetPreparationByPId(CampusEntities context, int PId);

        Preparation SavePreparation(CampusEntities context, Preparation preparation);

        PreparationMeteiral SavePrepMateiral(CampusEntities context, PreparationMeteiral preparation, HttpPostedFileBase file, string targetbase);

        PreparationMeteiral GetPrepMateiralById(CampusEntities context, int mId);

        void DeletePrepMateiralById(CampusEntities context, int mId);

        IEnumerable<PreparationMeteiral> GetPrepMateiralByPrepId(CampusEntities context, int PrepId);


    }
}
