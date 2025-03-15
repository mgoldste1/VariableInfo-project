using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System.Reflection
{
    public static class Extensions
    {
        public static List<VariableInfo> GetAllVariables(this Type type, BindingFlags flags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
        {
            var props = type.GetProperties(flags);
            //backing fields are the private fields properties create behind the scenes. we dont want them here.
            var fields = type.GetFields(flags).Where(o => !o.Name.ToUpper().Contains("__BACKINGFIELD")).ToArray();
            List<VariableInfo> vars = new List<VariableInfo>();
            if (props.Length > 0)
                vars.AddRange(props.Select(o => new VariableInfo(o)).ToList());
            if (fields.Length > 0)
                vars.AddRange(fields.Select(o => new VariableInfo(o)).ToList());

            if (type.BaseType != null)
                vars.AddRange(GetAllVariables(type.BaseType, flags));
            vars = vars.Distinct().ToList();
            return vars;
        }
    }
}
