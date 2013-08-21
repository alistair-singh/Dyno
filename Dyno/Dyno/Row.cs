using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Dynamic;

namespace Dyno
{
    public class Row : DynamicObject
    {
        private System.Data.DataRow dataRow;

        public Row(System.Data.DataRow dataRow)
        {
            // TODO: Complete member initialization
            this.dataRow = dataRow;
        }

        public override bool TryGetMember(GetMemberBinder binder, out object result)
        {
            var columnName = binder.Name;

            if (dataRow.Table.Columns.Contains(columnName))
            {
                result = dataRow[columnName];
                return true;
            }
            else
            {
                result = null;
                return false;
            }
        }
    }
}
