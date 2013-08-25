using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace Dyno
{
  public sealed class Set : ISet
  {
    private readonly SqlDataReader _reader;
    private readonly string[] _columns;
    private readonly bool _dispose;

    public Set(SqlDataReader reader, bool dispose = false)
    {
      _reader = reader;
      _dispose = dispose;
      var schemaTable = _reader.GetSchemaTable();
      if (schemaTable != null)
        _columns = schemaTable.Columns.Cast<DataColumn>().Select(x => x.ColumnName).ToArray();
    }

    public IEnumerator<IRow> GetEnumerator()
    {
      return new RowEnumerator(_reader, _dispose);
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
      return GetEnumerator();
    }

    public IEnumerable<string> Columns
    {
      get { return _columns; }
    }
  }
}