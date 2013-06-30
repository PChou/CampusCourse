using Campus.Course.Model;
using Campus.Course.Model.Business;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Campus.Course.Business.Interface
{
    public interface IPreparation
    {
        IEnumerable<PreparationInfo> GetPreparationByPId(CampusEntities context, int PId);

        Preparation SavePreparation(CampusEntities context, Preparation preparation);
    }
}
