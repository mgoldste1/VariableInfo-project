using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VInfoExample.Attributes;

namespace System.Reflection
{
    public static class Extensions
    {
        public static List<VariableInfo_CUSTOM> GetAllVariables_CUSTOM(this Type type, BindingFlags flags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
        {
            var props = type.GetProperties(flags);
            //backing fields are the private fields properties create behind the scenes. we dont want them here.
            var fields = type.GetFields(flags).Where(o => !o.Name.ToUpper().Contains("__BACKINGFIELD")).ToArray();
            List<VariableInfo_CUSTOM> vars = new List<VariableInfo_CUSTOM>();
            if (props.Length > 0)
                vars.AddRange(props.Select(o => new VariableInfo_CUSTOM(o)).ToList());
            if (fields.Length > 0)
                vars.AddRange(fields.Select(o => new VariableInfo_CUSTOM(o)).ToList());

            if (type.BaseType != null)
                vars.AddRange(GetAllVariables_CUSTOM(type.BaseType, flags));
            //vars = vars.Distinct().ToList();

            return vars;
        }
        public static List<VariableInfo_PASSTHROUGH> GetAllVariables_PASSTHROUGH(this Type type, BindingFlags flags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
        {
            var props = type.GetProperties(flags);
            //backing fields are the private fields properties create behind the scenes. we dont want them here.
            var fields = type.GetFields(flags).Where(o => !o.Name.ToUpper().Contains("__BACKINGFIELD")).ToArray();
            List<VariableInfo_PASSTHROUGH> vars = new List<VariableInfo_PASSTHROUGH>();
            if (props.Length > 0)
                vars.AddRange(props.Select(o => new VariableInfo_PASSTHROUGH(o)).ToList());
            if (fields.Length > 0)
                vars.AddRange(fields.Select(o => new VariableInfo_PASSTHROUGH(o)).ToList());

            if (type.BaseType != null)
                vars.AddRange(GetAllVariables_PASSTHROUGH(type.BaseType, flags));
            //vars = vars.Distinct().ToList();

            return vars;
        }

        public static List<VariableInfo_CUSTOM> GetAllVariables_ORIGINAL(this Type type, BindingFlags flags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
        {
            var props = type.GetProperties(flags);
            //backing fields are the private fields properties create behind the scenes. we dont want them here.
            var fields = type.GetFields(flags).Where(o => !o.Name.ToUpper().Contains("__BACKINGFIELD")).ToArray();
            List<VariableInfo_CUSTOM> vars = new List<VariableInfo_CUSTOM>();
            if (props.Length > 0)
                vars.AddRange(props.Select(o => new VariableInfo_CUSTOM(o)).ToList());
            if (fields.Length > 0)
                vars.AddRange(fields.Select(o => new VariableInfo_CUSTOM(o)).ToList());

            if (type.BaseType != null)
                vars.AddRange(GetAllVariables_ORIGINAL(type.BaseType, flags));
            vars = vars.Distinct().ToList();

            return vars;
        }
        public static void SaveToVar(this object Obj, string VariableName, object VariableValue)
        {
            Type t = Obj.GetType();
            if (!t.IsClass)
                throw new Exception("SaveToVar only works on classes");

            //changed to use the variable stategy pattern and reused the method below this one.
            var vars = t.GetAllVariables_CUSTOM(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            var ourVar = vars.Where(o => o.Name.ToUpper() == VariableName.ToUpper()).SingleOrDefault();

            if (ourVar == null)
            {
                //see if it has an alt name. if so use that instead.
                if (!vars.Any(o => o.Name.ToUpper() == VariableName.ToUpper()))
                {
                    foreach (var v in vars)
                    {
                        if (v.IsAttributeDefined(typeof(AlternativeNamesAttribute)))
                        {
                            var altNames = v.GetCustomAttribute<AlternativeNamesAttribute>().AlternateNames.ToList();
                            if (altNames.Contains(VariableName.ToUpper()))
                            {
                                ourVar = v;
                                break;
                            }
                        }
                    }
                }
            }
            if (ourVar != null)
                SaveToVar(Obj, ourVar, VariableValue);
            else
                throw new Exception($"Object of type '{t.Name}' does not contain an instanced variable named '{VariableName}'");
            return;
        }

        public static void SaveToVar(this object Obj, VariableInfo_CUSTOM VI, object VariableValue)
        {
            Type t = Obj.GetType();
            if (!t.IsClass)
                throw new Exception("SaveToVar only works on classes");
            if (t != VI.ReflectedType)
            {
                //they passed in a variable info object for an object type other than whatever Obj is.
                throw new Exception($"Object type is {t.FullName}, where as the variable info object passed in here came from {VI.DeclaringType}. They need to match.");
            }
            if (VariableValue == null && VI.IsNullable())
            {
                //its fine...
                _ = 436;
            }
            else if (VI.VariableType != VariableValue.GetType())
            {
                //try converting it.
                var converter = TypeDescriptor.GetConverter(VI.VariableType);
                if (converter != null)
                {
                    VariableValue = converter.ConvertFrom(VariableValue);
                }
            }
            VI.SetValue(Obj, VariableValue);
            return;
        }
    }
}
