using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace Dyno
{
  public static class DataRowCollectionExtensions
  {
    public static IEnumerable<IRow> Dyno(this DataRowCollection dataRowCollection)
    {
      return dataRowCollection.Cast<DataRow>().Select(row => row.Dyno());
    }
  }
}