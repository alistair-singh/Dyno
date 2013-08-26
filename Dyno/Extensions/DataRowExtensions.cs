using System.Data;

namespace Dyno
{
  public static class DataRowExtensions
  {
    public static IRow Dyno(this DataRow dataRow)
    {
      return new DataTableRow(dataRow);
    }
  }
}