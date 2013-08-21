using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Dynamic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;

namespace Dyno
{
    public class StoredProcedure
    {
        private Schema _schema;
        private string _name;
        private Dictionary<string, object> _args;

        public StoredProcedure(Schema schema, string storedProcedureName, Dictionary<string, object> args)
        {
            _schema = schema;
            _name = storedProcedureName;
            _args = args;
        }

        /// <summary>
        /// This method executes the stored procedure and return a DataSet 
        /// </summary>
        /// <returns></returns>
        public DataSet DataSet()
        {
            var fullStoredProcName = string.Format("{0}.{1}", _schema.SchemaName, _name);
            var ds = new DataSet();            
            var cmd = new SqlCommand(fullStoredProcName, _schema.DB.SqlConnection);
            foreach (var key in _args.Keys)
            {
                cmd.Parameters.AddWithValue(key, _args[key]);
            }
            cmd.CommandType = CommandType.StoredProcedure;
            var da = new SqlDataAdapter(cmd);

            
            da.Fill(ds);
            return ds;
        }
    }
}
