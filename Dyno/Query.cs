using System.Dynamic;

namespace Dyno
{
  public class Query : DynamicObject
  {
    public Db Db { get; private set; }
    public string CommandText { get; private set; }

    public Query(Db db, string query)
    {
      Db = db;
      CommandText = query;
    }

    public override bool TryInvoke(InvokeBinder binder, object[] args, out object result)
    {
      result = new QueryExecutor(this, Schema.MakeParams(binder.CallInfo, args));
      return true;
    }
  }
}
