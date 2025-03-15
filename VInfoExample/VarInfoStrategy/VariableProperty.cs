using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System.Reflection.Internal
{
    internal class VariableProperty : InfoParent
    {
        internal VariableProperty(MemberInfo mi) :base(mi) { }

        private PropertyInfo property;

        internal override void Init(MemberInfo mi)
        {
            property = mi as PropertyInfo;
        }

        public override object GetValue(object obj)
        {
            return property.GetValue(obj);
        }
        public override void SetValue(object obj, object value)
        {
            property.SetValue(obj, value);
        }
        public override Type VariableType => property.PropertyType;

        public override MemberTypes MemberType => property.MemberType;

        public override string Name => property.Name;

        public override Type DeclaringType => property.DeclaringType;

        public override Type ReflectedType => property.ReflectedType;

        public override object[] GetCustomAttributes(bool inherit)
        {
            return property.GetCustomAttributes(inherit);
        }

        public override object[] GetCustomAttributes(Type attributeType, bool inherit)
        {
            return property.GetCustomAttributes(attributeType, inherit);
        }

        public override bool IsDefined(Type attributeType, bool inherit)
        {
            return property.IsDefined(attributeType, inherit);
        }
    }
}
