using ALMS.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace ALMS.Utilities
{
    public class SQLHelper
    {
        private ALMSEntities _entity;
        public SQLHelper(ALMSEntities entity)
        {
            _entity = entity;
        }
        public DataTable ExecSqlReader(string procedureName, List<SqlParameter> sqlParameters)
        {
            DataTable dt = new DataTable();

            var conn = _entity.Database.Connection;
            var connectionState = conn.State;
            if (connectionState != ConnectionState.Open)
            {
                conn.Open();
            }

            using (var cmd = conn.CreateCommand())
            {
                cmd.Parameters.AddRange(sqlParameters.ToArray());
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = procedureName;
                cmd.CommandTimeout = 60;
                using (var reader = cmd.ExecuteReader())
                {
                    do
                    {
                        dt.Load(reader);
                    }
                    while (!reader.IsClosed);
                }
            }

            return dt;
        }

        public DataSet GetDataSet(string procedureName, List<SqlParameter> sqlParameters)
        {
            DataSet ds = new DataSet();

            var conn = _entity.Database.Connection;
            var connectionState = conn.State;
            if (connectionState != ConnectionState.Open)
            {
                conn.Open();
            }

            using (var cmd = conn.CreateCommand())
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddRange(sqlParameters.ToArray());
                cmd.CommandText = procedureName;
                cmd.CommandTimeout = 60;
                using (var reader = cmd.ExecuteReader())
                {
                    do
                    {
                        DataTable dt = new DataTable();
                        dt.Load(reader);
                        ds.Tables.Add(dt);
                    }
                    while (!reader.IsClosed);
                }
            }

            return ds;
        }

        public static string GetSQLMessage(System.Runtime.InteropServices._Exception exception)
        {
            if (exception.InnerException == null)
            {
                return exception.Message;
            }
            else
            {
                return GetSQLMessage(exception.InnerException);
            }  
        }

        //public DataSet ExecuteStoreProcedure(string storeProcedureName, List<SqlParameter> parameterList, System.Data.Entity.DbContextTransaction trans = null)
        //{
        //    DataSet ds = new DataSet();
        //    if (trans == null) //無交易單獨呼叫StoreProcedure
        //    {
        //        using (_happyRecome_PublicEntities = new HappyRecome_PublicEntities())
        //        {
        //            var conn = _happyRecome_PublicEntities.Database.Connection;
        //            var connectionState = conn.State;
        //            if (connectionState != ConnectionState.Open)
        //            {
        //                conn.Open();
        //            }

        //            using (var cmd = conn.CreateCommand())
        //            {
        //                //http://www.radwick.com/2016/02/entity-framework-performance-arithabort.html
        //                /*
        //                cmd.Parameters.AddRange(parameterList.ToArray());
        //                cmd.CommandType = CommandType.StoredProcedure;
        //                cmd.CommandText = storeProcedureName;
        //                */
        //                cmd.CommandType = CommandType.Text;

        //                var paras = new List<string>();
        //                foreach (var item in parameterList)
        //                {
        //                    paras.Add(string.Format("@{0}='{1}'", item.ParameterName.Replace("@", ""), item.Value));
        //                }
        //                var text = string.Format("SET ARITHABORT ON; EXEC {0} {1}", storeProcedureName,
        //                    string.Join(",", paras));

        //                cmd.CommandText = text;

        //                cmd.CommandTimeout = 180;
        //                using (var reader = cmd.ExecuteReader())
        //                {
        //                    do
        //                    {
        //                        DataTable dt = new DataTable();
        //                        dt.Load(reader);
        //                        ds.Tables.Add(dt);
        //                    }
        //                    while (!reader.IsClosed);
        //                }
        //            }
        //        }
        //    }
        //    else   //交易時呼叫StoreProcedure
        //    {
        //        var conn = _happyRecome_PublicEntities.Database.Connection;
        //        conn = trans.UnderlyingTransaction.Connection;
        //        var connectionState = conn.State;
        //        if (connectionState != ConnectionState.Open)
        //        {
        //            conn.Open();
        //        }

        //        using (var cmd = conn.CreateCommand())
        //        {
        //            cmd.Parameters.AddRange(parameterList.ToArray());
        //            cmd.CommandType = CommandType.StoredProcedure;
        //            cmd.CommandText = storeProcedureName;
        //            cmd.CommandTimeout = 60;
        //            cmd.Transaction = trans.UnderlyingTransaction;
        //            using (var reader = cmd.ExecuteReader())
        //            {
        //                do
        //                {
        //                    DataTable dt = new DataTable();
        //                    dt.Load(reader);
        //                    ds.Tables.Add(dt);
        //                }
        //                while (!reader.IsClosed);
        //            }
        //        }
        //    }
        //    return ds;
        //}

    }
}