﻿using Campus.Course.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Campus.Course.Business.Interface
{
    public interface IPreparation
    {
        void GetPreparationByPId(CampusEntities context,int PId);
    }
}
