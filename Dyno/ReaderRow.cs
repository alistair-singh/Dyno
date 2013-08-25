using System.Data.SqlClient;
using System.Dynamic;

namespace Dyno
{
  public class ReaderRow : DynamicObject, IRow
  {
    private readonly SqlDataReader _reader;

    public ReaderRow(SqlDataReader reader)
    {
      _reader = reader;
    }

    public override bool TryGetMember(GetMemberBinder binder, out object result)
    {
      var columnName = binder.Name;
      result = _reader[columnName];
      return true;
    }

    public T Get<T>(string columnName)
    {
      return (T)_reader[columnName];
    }
  }
}