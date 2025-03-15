using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System.Reflection.Internal
{
    internal abstract class InfoParent : MemberInfo
    {
        internal InfoParent(MemberInfo mi)
        {
            this.mi = mi;
            Init(mi);
        }
        internal MemberInfo mi;
        internal abstract void Init(MemberInfo mi);

        public abstract object GetValue(object obj);
        public abstract void SetValue(object obj, object value);

        public abstract Type VariableType { get; }

        public T GetCustomAttribute<T>() where T : Attribute
        {

            MethodInfo[] methods = typeof(CustomAttributeExtensions).GetMethods(BindingFlags.Public | BindingFlags.Static);
            MethodInfo method = methods.Where(o => o.Name == "GetCustomAttribute" && o.GetParameters().Count() == 1 && o.GetParameters().First().ParameterType == typeof(MemberInfo) && o.IsGenericMethod).Single();
            MethodInfo generic = method.MakeGenericMethod(typeof(T));
            object[] input = new object[1] { mi };
            T result = (T)generic.Invoke(null, input);
            return result;
            
        }
        public override MemberTypes MemberType => mi.MemberType; 

        public override string Name => mi.Name;

        public override Type DeclaringType => mi.DeclaringType;

        public override Type ReflectedType => mi.ReflectedType;

        public override object[] GetCustomAttributes(bool inherit) => mi.GetCustomAttributes(inherit);

        public override object[] GetCustomAttributes(Type attributeType, bool inherit) => mi.GetCustomAttributes(attributeType, inherit);

        public override bool IsDefined(Type attributeType, bool inherit) => mi.IsDefined(attributeType, inherit);
    }
}
