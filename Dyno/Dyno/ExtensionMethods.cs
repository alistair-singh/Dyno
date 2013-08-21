using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace Dyno
{
    public static class ExtensionMethods
    {
        public static Row Dyno(this DataRow dataRow)
        {
            return new Row(dataRow);
        }

        public static Row[] Dyno(this DataRowCollection dataRowCollection)
        {
            var result = new Row[dataRowCollection.Count];
            for (int loop = 0; loop < dataRowCollection.Count; loop++)
            {
                result[loop] = dataRowCollection[loop].Dyno();
            }
            return result;
        }
    }
}
