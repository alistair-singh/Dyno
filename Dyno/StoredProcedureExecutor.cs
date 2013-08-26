using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics.Eventing.Reader;

namespace Dyno
{
  public class StoredProcedureExecutor : ExecutorBase
  {
    private readonly StoredProcedure _sp;
    private readonly Dictionary<string, object> _args;

    public StoredProcedureExecutor(StoredProcedure sp, Dictionary<string, object> args)
    {
      _sp = sp;
      _args = args;
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
      return MakeCommand(_sp, _args).ExecuteReader(behavior);
    }

    public override SqlDataAdapter Adapter()
    {
      return new SqlDataAdapter(MakeCommand(_sp, _args));
    }

    public override T Scalar<T>()
    {
      using (var command = MakeCommand(_sp, _args))
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
      using (var command = MakeCommand(_sp, _args))
        return command.ExecuteNonQuery();
    }
  }
}
