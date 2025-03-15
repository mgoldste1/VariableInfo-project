using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace VInfoExample
{
    internal class Program
    {
        //random attribute type on here just for demonstration
        [XmlAttribute]
        public string property {  get; set; }
        [XmlAttribute("id")]
        public string field;
        static void Main(string[] args)
        {
            new Program();
        }
        Program()
        {
            VariableInfo p_vi = new VariableInfo(this.GetType().GetProperty("property"));
            VariableInfo f_vi = new VariableInfo(this.GetType().GetField("field"));
            bool works1 = p_vi.IsDefined(typeof(XmlAttributeAttribute));
            bool works2 = f_vi.IsDefined(typeof(XmlAttributeAttribute));
            bool works3 = p_vi.IsAttributeDefined(typeof(XmlAttributeAttribute));
            bool works4 = f_vi.IsAttributeDefined(typeof(XmlAttributeAttribute));
            bool works5 = Attribute.IsDefined(f_vi, typeof(XmlAttributeAttribute));

            //this works.
            PropertyInfo asPropInfo = (PropertyInfo)p_vi;
            //inside Attribute.IsDefined it casts the inputted object (my variableinfo) to a property info, just like the above line.
            //that fails --- System.InvalidCastException: 'Unable to cast object of type 'System.Reflection.VariableInfo' to type 'System.Reflection.PropertyInfo'.'
            bool fails1 = Attribute.IsDefined(p_vi, typeof(XmlAttributeAttribute));



        }
    }
}
