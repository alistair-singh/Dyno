using System.Dynamic;

namespace Dyno
{
  public class StoredProcedure : DynamicObject
  {
    public Schema Schema { get; private set; }
    public string Name { get; private set; }
    public string FullName { get; private set; }

    public StoredProcedure(Schema schema, string storedProcedureName)
    {
      Schema = schema;
      Name = storedProcedureName;
      FullName = string.Format("{0}.{1}", Schema.SchemaName, Name);
    }

    public override bool TryInvoke(InvokeBinder binder, object[] args, out object result)
    {
      result = new StoredProcedureExecutor(this, Schema.MakeParams(binder.CallInfo, args));
      return true;
    }
  }
}
