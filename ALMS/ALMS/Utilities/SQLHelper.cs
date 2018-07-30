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
        public string ExecuteCommand(string procedureName, List<SqlParameter> sqlParameters)
        {
            var errMsg = "";
            try
            {
                var conn = _entity.Database.Connection;
                var connectionState = conn.State;
                if (connectionState != ConnectionState.Open)
                {
                    conn.Open();
                }

                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = procedureName;
                    cmd.Parameters.AddRange(sqlParameters.ToArray());
                    cmd.CommandTimeout = 60;
                    cmd.ExecuteNonQuery();
                }
            }
            catch (System.Data.Entity.Validation.DbEntityValidationException ex)
            {
                errMsg = ex.Message;
            }
            catch (Exception ex)
            {
                errMsg = SQLHelper.GetSQLMessage(ex);
            }
            return errMsg;
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

        public string GetDbEntityValidationExceptionMessage(System.Data.Entity.Validation.DbEntityValidationException ex)
        {
            var errMsg = string.Empty;
            foreach (var eve in ex.EntityValidationErrors)
            {
                errMsg = string.Format("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:", eve.Entry.Entity.GetType().Name, eve.Entry.State);
                foreach (var ve in eve.ValidationErrors)
                {
                    errMsg += "<br />" + string.Format("- Property: \"{0}\", Error: \"{1}\"", ve.PropertyName, ve.ErrorMessage);
                }
            }

            //errMsg = CommonHelper.GetCodeName("W009");

            return errMsg;
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
        //public void ExecuteCommand(string commandText, System.Data.Entity.DbContextTransaction trans = null)
        //{
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
        //                cmd.CommandType = CommandType.Text;
        //                cmd.CommandText = commandText;
        //                cmd.CommandTimeout = 60;
        //                cmd.ExecuteNonQuery();
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
        //            cmd.CommandText = commandText;
        //            cmd.CommandTimeout = 60;
        //            cmd.Transaction = trans.UnderlyingTransaction;
        //            cmd.ExecuteNonQuery();
        //        }
        //    }
        //}

    }
}