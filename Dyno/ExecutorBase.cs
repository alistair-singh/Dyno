using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;

namespace Dyno
{
  public abstract class ExecutorBase : DynamicObject
  {
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

      if (binder.ReturnType == typeof(Set))
      {
        result = new Set(Reader(), true);
        return true;
      }

      return base.TryConvert(binder, out result);
    }

    public override bool TryInvoke(InvokeBinder binder, object[] args, out object result)
    {
      if (args.Length != 0)
        return base.TryInvoke(binder, args, out result);

      result = this;
      return true;
    }

    public abstract DataSet DataSet();
    public abstract SqlDataReader Reader(CommandBehavior behavior = CommandBehavior.Default);
    public abstract SqlDataAdapter Adapter();
    public abstract T Scalar<T>();
    public abstract object Scalar();
    public abstract int ExecuteNonQuery();

    internal static SqlCommand MakeCommand(Query query, Dictionary<string, object> args)
    {
      var command = new SqlCommand(query.CommandText, query.Db.SqlConnection)
      {
        CommandType = CommandType.Text
      };

      foreach (var kvPair in args)
        command.Parameters.AddWithValue(kvPair.Key, kvPair.Value);

      return command;
    }

    internal static SqlCommand MakeCommand(StoredProcedure sp, Dictionary<string, object> args)
    {
      var command = new SqlCommand(sp.FullName, sp.Schema.Db.SqlConnection)
      {
        CommandType = CommandType.StoredProcedure
      };

      foreach (var kvPair in args)
        command.Parameters.AddWithValue(kvPair.Key, kvPair.Value);

      return command;
    }
  }
}