using System.Data.SqlClient;

namespace Dyno
{
  public static class SqlDataReaderExtensions
  {
    public static IRow Dyno(this SqlDataReader reader)
    {
      return new ReaderRow(reader);
    }
  }
}