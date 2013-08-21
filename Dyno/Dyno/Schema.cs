using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Dynamic;

namespace Dyno
{
    public class Schema : DynamicObject
    {
        public string SchemaName { get; set; }
        public DB DB { get; set; }

        public Schema(string schemaName, DB db)
        {
            SchemaName = schemaName;
            DB = db;
        }

        public override bool TryInvokeMember(InvokeMemberBinder binder, object[] args, out object result)
        {
            var parameters = new Dictionary<string, object>();
            
            if(args.Length != binder.CallInfo.ArgumentNames.Count)
                throw new Exception("Error : number of argument names does not match number of argument values.");

            for(int loop = 0; loop < binder.CallInfo.ArgumentCount; loop++)
            {
                parameters.Add(string.Format("@{0}", binder.CallInfo.ArgumentNames[loop]), args[loop]);
            }

            result = new StoredProcedure(this, binder.Name, parameters);

            return true;
        }
    }
}
