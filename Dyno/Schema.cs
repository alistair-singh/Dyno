using System;
using System.Collections.Generic;
using System.Dynamic;

namespace Dyno
{
  public class Schema : DynamicObject
  {
    public string SchemaName { get; private set; }
    public Db Db { get; private set; }

    public Schema(string schemaName, Db db)
    {
      SchemaName = schemaName;
      Db = db;
    }

    public override bool TryGetMember(GetMemberBinder binder, out object result)
    {
      result = new StoredProcedure(this, binder.Name);
      return true;
    }

    public override bool TryInvokeMember(InvokeMemberBinder binder, object[] args, out object result)
    {
      var sp = new StoredProcedure(this, binder.Name);
      result = new StoredProcedureExecutor(sp, MakeParams(binder.CallInfo, args));
      return true;
    }

    public override bool TryGetIndex(GetIndexBinder binder, object[] indexes, out object result)
    {
      var index = indexes[0].ToString();
      result = new StoredProcedure(this, string.Format("[{0}]", index));
      return true;
    }

    internal static Dictionary<string, object> MakeParams(CallInfo info, object[] args)
    {
      var parameters = new Dictionary<string, object>();

      if (args.Length != info.ArgumentNames.Count)
        throw new Exception("Error : number of argument names does not match number of argument values.");

      for (int loop = 0; loop < info.ArgumentCount; loop++)
      {
        parameters.Add(string.Format("@{0}", info.ArgumentNames[loop]), args[loop] ?? DBNull.Value);
      }
      return parameters;
    }
  }
}
