using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Dynamic;
using System.Linq;

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

    public override bool TryInvoke(InvokeBinder binder, object[] args, out object result)
    {
      if (args.Length == 1 && args[0] is string)
      {
        result = Query(args[0] as string);
        return true;
      }

      if (args.Length == 1 && args[0] is IQueryable)
      {
        result = Query(args[0] as IQueryable);
        return true;
      }

      return base.TryInvoke(binder, args, out result);
    }

    private object Query(string query)
    {
      return new Query(this, query);
    }

    private object Query(IQueryable p)
    {
      return null;
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
