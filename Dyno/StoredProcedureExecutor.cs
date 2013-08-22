using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;

namespace Dyno
{
  public class StoredProcedureExecutor : DynamicObject
  {
    private readonly StoredProcedure _sp;
    private readonly Dictionary<string, object> _args;

    public StoredProcedureExecutor(StoredProcedure sp, Dictionary<string, object> args)
    {
      _sp = sp;
      _args = args;
    }

    public override bool TryConvert(ConvertBinder binder, out object result)
    {
      if (binder.ReturnType == typeof(DataSet))
      {
        result = DataSet();
        return true;
      }

      if (binder.ReturnType == typeof(SqlDataAdapter))
      {
        result = Adapter();
        return true;
      }

      if (binder.ReturnType == typeof(int))
      {
        result = ExecuteNonQuery();
        return true;
      }

      if (binder.ReturnType == typeof(object))
      {
        result = Scalar();
        return true;
      }

      if (binder.ReturnType == typeof(Result))
      {
        result = new Result(this);
        return true;
      }

      return base.TryConvert(binder, out result);
    }

    /// <summary>
    /// This method executes the stored procedure and return a DataSet 
    /// </summary>
    /// <returns></returns>
    public DataSet DataSet()
    {
      using (var da = Adapter())
      {
        var ds = new DataSet();
        da.Fill(ds);
        return ds;
      }
    }

    public SqlDataReader Reader(CommandBehavior behavior = CommandBehavior.Default)
    {
      return MakeCommand().ExecuteReader(behavior);
    }

    public SqlDataAdapter Adapter()
    {
      return new SqlDataAdapter(MakeCommand());
    }

    public T Scalar<T>()
    {
      using (var command = MakeCommand())
      {
        var obj = command.ExecuteScalar();
        if (obj is T)
          return (T)obj;
      }

      //TODO: Custom Exception
      throw new Exception("result is not of type T");
    }

    public object Scalar()
    {
      return Scalar<object>();
    }

    public int ExecuteNonQuery()
    {
      using (var command = MakeCommand())
        return command.ExecuteNonQuery();
    }

    private SqlCommand MakeCommand()
    {
      var command = new SqlCommand(_sp.FullName, _sp.Schema.Db.SqlConnection)
      {
        CommandType = CommandType.StoredProcedure
      };

      foreach (var key in _args.Keys)
        command.Parameters.AddWithValue(key, _args[key]);

      return command;
    }
  }
}
