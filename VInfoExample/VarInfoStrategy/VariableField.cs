using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System.Reflection.Internal
{
    internal class VariableField : InfoParent
    {
        private FieldInfo field;
        internal VariableField(MemberInfo mi) : base(mi) { }

        internal override void Init(MemberInfo mi)
        {
            field = mi as FieldInfo;
        }
        public override object GetValue(object obj)
        {
            return field.GetValue(obj);
        }
        public override void SetValue(object obj, object value)
        {
            field.SetValue(obj, value);
        }
        public override Type VariableType =>  field.FieldType; 
        public override MemberTypes MemberType => field.MemberType;

        public override string Name => field.Name;

        public override Type DeclaringType => field.DeclaringType;

        public override Type ReflectedType => field.ReflectedType;

        public override object[] GetCustomAttributes(bool inherit)
        {
            return field.GetCustomAttributes(inherit);
        }

        public override object[] GetCustomAttributes(Type attributeType, bool inherit)
        {
            return field.GetCustomAttributes(attributeType, inherit);
        }

        public override bool IsDefined(Type attributeType, bool inherit)
        {
            return field.IsDefined(attributeType, inherit);
        }
    }
}
