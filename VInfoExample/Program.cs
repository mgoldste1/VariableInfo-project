using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;
using VInfoExample.Attributes;

namespace VInfoExample
{
    internal class Program
    {
        //random attribute type on here just for demonstration
        [AlternativeNames("Aardvark")]
        public string property {  get; set; }
        [AlternativeNames("Manatee","HouseCat")]
        public string field;
        static void Main(string[] args)
        {
            new Program();
        }
        Program()
        {
            VariableInfo p_vi = new VariableInfo(this.GetType().GetProperty("property"));
            VariableInfo f_vi = new VariableInfo(this.GetType().GetField("field"));
            bool works1 = p_vi.IsDefined(typeof(AlternativeNamesAttribute));
            bool works2 = f_vi.IsDefined(typeof(AlternativeNamesAttribute));
            bool works3 = p_vi.IsAttributeDefined(typeof(AlternativeNamesAttribute));
            bool works4 = f_vi.IsAttributeDefined(typeof(AlternativeNamesAttribute));
            bool works5 = Attribute.IsDefined(f_vi, typeof(AlternativeNamesAttribute));

            //this works.
            PropertyInfo asPropInfo = (PropertyInfo)p_vi;
            FieldInfo asFieldInfo = (FieldInfo)f_vi;

            //also works
            VariableInfo propBackToVarInfo = (VariableInfo)asPropInfo;
            VariableInfo fieldBackToVarInfo = (VariableInfo)asFieldInfo;

            try
            {
                //inside Attribute.IsDefined it casts the inputted object (my variableinfo) to a property info, just like the above line.
                //it fails --- System.InvalidCastException: 'Unable to cast object of type 'System.Reflection.VariableInfo' to type 'System.Reflection.PropertyInfo'.'
                //dont use this :(
                bool fails1 = Attribute.IsDefined(p_vi, typeof(AlternativeNamesAttribute));
            }
            catch { }
            
            foreach(var v in this.GetType().GetAllVariables())
            {
                if (v.Name == "Aardvark")
                    this.SaveToVar(v, "ants");
                else if (v.IsAttributeDefined(typeof(AlternativeNamesAttribute)) && v.GetCustomAttribute<AlternativeNamesAttribute>().AlternateNames.Contains("Aardvark"))
                    this.SaveToVar(v, "ants");
                else if(v.Name == "HouseCat")
                    v.SetValue(this, "mouse");
                else if (v.IsAttributeDefined(typeof(AlternativeNamesAttribute)) && v.GetCustomAttribute<AlternativeNamesAttribute>().AlternateNames.Contains("HouseCat"))
                    v.SetValue(this, "mouse");
            }
            if (property != "ants")
                throw new Exception("i am a terrible programmer");
            if(field != "mouse")
                throw new Exception("i am a terrible programmer");


        }
    }
}
