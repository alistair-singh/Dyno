using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Dynamic;

namespace Dyno
{
  public class Db : DynamicObject, IDisposable
  {
    public SqlConnection SqlConnection { get; private set; }

    public Db(string connectionStringName)
    {
      SqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings[connectionStringName].ConnectionString);
      SqlConnection.Open();
    }

    public void Dispose()
    {
      SqlConnection.Dispose();
    }

    public override bool TryGetIndex(GetIndexBinder binder, object[] indexes, out object result)
    {
      var index = indexes[0].ToString();
      result = new Schema(string.Format("[{0}]", index), this);
      return true;
    }

    public override bool TryGetMember(GetMemberBinder binder, out object result)
    {
      result = new Schema(binder.Name, this);
      return true;
    }
  }
}
