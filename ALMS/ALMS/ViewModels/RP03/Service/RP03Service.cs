using ALMS.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using ALMS.Utilities;

namespace ALMS.ViewModels.RP03.Service
{
    public class RP03Service : ServiceBase
    {
        public RP03Service()
        {
            _entity = new ALMSEntities();
        }

        public DataSet GetData(string type,string begingDate, string endDate)
        {
            if (string.IsNullOrWhiteSpace(type))
            {
                type = "";
            }
            List<SqlParameter> sqlParameterList = new List<SqlParameter>();
            sqlParameterList.Add(new SqlParameter("Type", type));
            sqlParameterList.Add(new SqlParameter("BEG_DT", begingDate));
            sqlParameterList.Add(new SqlParameter("END_DT", endDate));

            return new SQLHelper(_entity).GetDataSet("SP_ALMS_RP03", sqlParameterList);
        }


    }
}