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
            //string cnnString = System.Configuration.ConfigurationManager.ConnectionStrings["ALMSEntities"].ConnectionString;

            //SqlConnection cnn = new SqlConnection(cnnString);
            //SqlCommand cmd = new SqlCommand();
            //SqlDataReader reader;

            //cmd.CommandText = "SP_ALMS_RP02";
            //cmd.CommandType = CommandType.StoredProcedure;
            //cmd.Connection = cnn;
            //cmd.Parameters.Add("@BEG_DT", SqlDbType.VarChar);
            //cmd.Parameters["@BEG_DT"].Value = date1;

            //cmd.Parameters.Add("@END_DT", SqlDbType.VarChar);
            //cmd.Parameters["@END_DT"].Value = date2;


            //cnn.Open();

            //reader = cmd.ExecuteReader();
            //// Data is accessible through the DataReader object here.
            //var dataTable = new DataTable();
            //dataTable.Load(reader);

            //cnn.Close();
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