using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace Dyno
{
  public class QueryExecutor : ExecutorBase
  {
    private readonly Query _query;
    private readonly Dictionary<string, object> _args;

    public QueryExecutor(Query query, Dictionary<string, object> arg)
    {
      _query = query;
      _args = arg;
    }

    public override DataSet DataSet()
    {
      using (var da = Adapter())
      {
        var ds = new DataSet();
        da.Fill(ds);
        return ds;
      }
    }

    public override SqlDataReader Reader(CommandBehavior behavior = CommandBehavior.Default)
    {
      return MakeCommand(_query, _args).ExecuteReader(behavior);
    }

    public override SqlDataAdapter Adapter()
    {
      return new SqlDataAdapter(MakeCommand(_query, _args));
    }

    public override T Scalar<T>()
    {
      using (var command = MakeCommand(_query, _args))
      {
        var obj = command.ExecuteScalar();
        if (obj is T)
          return (T)obj;
      }

      //TODO: Custom Exception
      throw new Exception("result is not of type T");
    }

    public override object Scalar()
    {
      return Scalar<object>();
    }

    public override int ExecuteNonQuery()
    {
      using (var command = MakeCommand(_query, _args))
        return command.ExecuteNonQuery();
    }
  }
}
