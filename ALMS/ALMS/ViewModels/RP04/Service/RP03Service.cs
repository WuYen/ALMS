using ALMS.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using ALMS.Utilities;

namespace ALMS.ViewModels.RP04.Service
{
    public class RP04Service : ServiceBase
    {
        public RP04Service()
        {
            _entity = new ALMSEntities();
        }

        public DataTable GetData(string type, string begingDate, string endDate)
        {
            List<SqlParameter> sqlParameterList = new List<SqlParameter>();
            sqlParameterList.Add(new SqlParameter("Type", type));
            sqlParameterList.Add(new SqlParameter("BEG_DT", begingDate));
            sqlParameterList.Add(new SqlParameter("END_DT", endDate));

            return new SQLHelper(_entity).ExecSqlReader("SP_ALMS_RP04", sqlParameterList);
        }


    }
}