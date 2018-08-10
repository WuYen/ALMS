using ALMS.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using ALMS.Utilities;

namespace ALMS.ViewModels.RP02.Service
{
    public class RP02Service : ServiceBase
    {
        public RP02Service()
        {
            _entity = new ALMSEntities();
        }

        public DataTable GetData(string date1, string date2)
        {
            if (date1.Contains("0001")|| date2.Contains("0001"))
            {
                return new DataTable();
            }
            List<SqlParameter> sqlParameterList = new List<SqlParameter>();
            sqlParameterList.Add(new SqlParameter("BEG_DT", date1));
            sqlParameterList.Add(new SqlParameter("END_DT", date2));

            return new SQLHelper(_entity).ExecSqlReader("SP_ALMS_RP02", sqlParameterList);
        }

        public string SetMonthClose(string date)
        {
            List<SqlParameter> sqlParameterList = new List<SqlParameter>();
            sqlParameterList.Add(new SqlParameter("BEG_DT", date));

            return new SQLHelper(_entity).ExecuteCommand("SP_ALMS_MonthEndToBeg", sqlParameterList);
        }


    }
}