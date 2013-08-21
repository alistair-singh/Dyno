using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Dynamic;
using System.Data.SqlClient;
using System.Configuration;

namespace Dyno
{
    public class DB : DynamicObject, IDisposable
    {
        public SqlConnection SqlConnection { get; set; }
        public DB(string connectionStringName)
        {
            SqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings[connectionStringName].ConnectionString);
            SqlConnection.Open();
        }

        public void Dispose()
        {
            SqlConnection.Dispose();
        }

        /// <summary>
        /// This method is to return a schema.
        /// </summary>
        /// <param name="binder"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        public override bool TryGetMember(GetMemberBinder binder, out object result)
        {
            result = new Schema(binder.Name, this);
            return true;
        }
    }
}
