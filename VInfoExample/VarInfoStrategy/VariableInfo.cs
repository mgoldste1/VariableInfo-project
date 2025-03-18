using System;
using System.Linq;
using System.Reflection.Internal;
using System.Text;
using System.Threading.Tasks;

namespace System.Reflection
{
    public class VariableInfo : MemberInfo
    {
        private InfoParent variable;
        public VariableInfo(PropertyInfo pi) 
        {
            variable = new VariableProperty(pi);
        }
        public VariableInfo(FieldInfo fi)
        {
            variable = new VariableField(fi);
        }
        public object GetValue(object obj)
        {
            return variable.GetValue(obj);
        }
        public void SetValue(object obj, object value)
        {
            variable.SetValue(obj, value);
        }
        public Type VariableType => variable.VariableType;
        public T GetCustomAttribute<T>()  where T : Attribute
        {
            MethodInfo[] methods = variable.GetType().GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
            MethodInfo method = methods.Where(o => o.Name == "GetCustomAttribute").Single();
            MethodInfo generic = method.MakeGenericMethod(typeof(T));
            object[] input = new object[0];
            T result = (T)generic.Invoke(variable, input);
            return result;
        }
        public override MemberTypes MemberType => MemberTypes.Custom;

        public override string Name => variable.Name;

        public override Type DeclaringType => variable.DeclaringType;

        public override Type ReflectedType => variable.ReflectedType;

        public override object[] GetCustomAttributes(bool inherit) => variable.GetCustomAttributes(inherit);

        public override object[] GetCustomAttributes(Type attributeType, bool inherit) => variable.GetCustomAttributes(attributeType, inherit);

        public override bool IsDefined(Type attributeType, bool inherit=true) => variable.IsDefined(attributeType, inherit);

        public bool IsAttributeDefined(Type attributeType)
        {
            if (variable.mi is PropertyInfo pi)
                return Attribute.IsDefined(pi, attributeType);
            else if (variable.mi is FieldInfo fi)
                return Attribute.IsDefined(fi, attributeType);
            else
                throw new Exception("Unsupported var type");
        }
        public bool IsAttributeDefined(Type attributeType, bool inheritted)
        {
            if (variable.mi is PropertyInfo pi)
                return Attribute.IsDefined(pi, attributeType, inheritted);
            else if (variable.mi is FieldInfo fi)
                return Attribute.IsDefined(fi, attributeType, inheritted);
            else
                throw new Exception("Unsupported var type");
        }
        public bool IsNullable()
        {
            if (VariableType == typeof(string))
                return true;

            if (VariableType.IsClass)
                return true;

            if (VariableType.IsGenericType && VariableType.GetGenericTypeDefinition() == typeof(Nullable<>))
                return true;

            return false;
        }
        public override string ToString()
        {
            return variable.mi.ToString();
        }
        /// <summary>
        /// lets you do things like - if(vari.MemberType == MemberTypes.Property)
        ///                           {
        ///                               PropertyInfo pi = (PropertyInfo)vari;
        ///                           }
        /// doesnt work for "vari as PropertyInfo pi" though :(
        /// </summary>
        /// <param name="a"></param>
        public static explicit operator PropertyInfo(VariableInfo a)
        {
            return a.variable.mi as PropertyInfo;
        }
        public static explicit operator FieldInfo(VariableInfo a)
        {
            return a.variable.mi as FieldInfo;
        }

        public static implicit operator VariableInfo(PropertyInfo a)
        {
            return new VariableInfo(a);
        }
        public static implicit operator VariableInfo(FieldInfo a)
        {
            return new VariableInfo(a);
        }
        // based on MS doc: https://learn.microsoft.com/en-us/dotnet/api/system.iequatable-1.equals?view=netframework-4.7.2
        public static bool operator ==(VariableInfo FirstOne, VariableInfo OtherOne)
        {
            if (((object)FirstOne) == null || ((object)OtherOne) == null)
                return Object.Equals(FirstOne, OtherOne);

            return FirstOne.Equals(OtherOne);
        }

        public static bool operator !=(VariableInfo FirstOne, VariableInfo OtherOne)
        {
            if (((object)FirstOne) == null || ((object)OtherOne) == null)
                return !Object.Equals(FirstOne, OtherOne);

            return !(FirstOne.Equals(OtherOne));
        }
        public override bool Equals(Object obj)
        {
            if (obj == null)
                return false;

            VariableInfo lwObj = obj as VariableInfo;
            if (lwObj == null)
                return false;
            else
                return Equals(lwObj);
        }

        public bool Equals(VariableInfo other)
        {
            if (other == null)
                return false;
            if (variable.MemberType != other.variable.MemberType)
                return false;
            //both are either fields or properties now.
            if (variable.mi.Name == other.variable.mi.Name && variable.mi.DeclaringType == other.variable.mi.DeclaringType)
                return true;
            else
                return false;
        }
        /// <summary>
        /// note: once this moves to .net core there is a built in hashcode class. use that one instead
        /// https://learn.microsoft.com/en-us/dotnet/api/system.hashcode.combine?view=net-8.0
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            unchecked
            {
                int hash = 269;
                if (variable.mi.Name != null)
                    hash *= 23 + variable.mi.Name.GetHashCode();
                if (variable.mi.DeclaringType != null)
                    hash *= 23 + variable.mi.DeclaringType.GetHashCode();
                return hash;
            }
        }
    }
}
