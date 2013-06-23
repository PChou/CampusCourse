using Campus.Course.Model;
using Campus.Course.Model.Business;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Campus.Course.Business.Interface
{
    public interface ILogin
    {
        bool CheckLogin(CampusEntities context, string user, string password, bool isStudent);
        V_CurrentUser GetCurrentUser(CampusEntities context, string userno, bool isStudent);
    }
}
