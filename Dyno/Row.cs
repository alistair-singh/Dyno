using System;
using System.Data;
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

  public class DataTableRow : DynamicObject, IRow
  {
    private readonly DataRow _dataRow;

    public DataTableRow(DataRow dataRow)
    {
      _dataRow = dataRow;
    }

    public override bool TryGetMember(GetMemberBinder binder, out object result)
    {
      var columnName = binder.Name;

      if (!_dataRow.Table.Columns.Contains(columnName)) 
        return base.TryGetMember(binder, out result);

      result = _dataRow[columnName];
      return true;
    }

    public T Get<T>(string columnName)
    {
      if (_dataRow.Table.Columns.Contains(columnName))
      {
        return (T)_dataRow[columnName];
      }

      //TODO: Custom exception
      throw new Exception("column does not exist");
    }
  }
}
