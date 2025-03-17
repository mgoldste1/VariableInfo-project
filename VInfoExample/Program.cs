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

            DistinctBroken();

        }
        public class A { public string s1,s2,s3;  }
        public class B : A { public string s4; }
        public class C : B { public string s5; }
        public void DistinctBroken()
        {
            C childClass = new C();

            //i commented out the distinct in this get all variables so you can see it here.
            var ourVars_CUSTOM = childClass.GetType().GetAllVariables_CUSTOM();
            string varsInit = string.Join(",", ourVars_CUSTOM.Select(o => o.Name).OrderBy(o=>o));

            var distinctVars_CUSTOM = ourVars_CUSTOM.Distinct().ToList();
            string varsAfterDistinct_CUSTOM = string.Join(",", distinctVars_CUSTOM.Select(o => o.Name).OrderBy(o => o));


            var ourVars_PASSTHROUGH = childClass.GetType().GetAllVariables_PASSTHROUGH();
            string varsInit_PASSTHROUGH = string.Join(",", ourVars_PASSTHROUGH.Select(o => o.Name).OrderBy(o => o));

            var distinctVars_PASSTHROUGH = ourVars_PASSTHROUGH.Distinct().ToList();
            string varsAfterDistinct_PASSTHROUGH = string.Join(",", distinctVars_PASSTHROUGH.Select(o => o.Name).OrderBy(o => o));

            if (varsAfterDistinct_CUSTOM != varsAfterDistinct_PASSTHROUGH)
                throw new Exception("i am so very confused");

        }
        public void ExampleCode()
        {
            VariableInfo_CUSTOM p_vi = new VariableInfo_CUSTOM(this.GetType().GetProperty("property"));
            VariableInfo_CUSTOM f_vi = new VariableInfo_CUSTOM(this.GetType().GetField("field"));
            bool works1 = p_vi.IsDefined(typeof(AlternativeNamesAttribute));
            bool works2 = f_vi.IsDefined(typeof(AlternativeNamesAttribute));
            bool works3 = p_vi.IsAttributeDefined(typeof(AlternativeNamesAttribute));
            bool works4 = f_vi.IsAttributeDefined(typeof(AlternativeNamesAttribute));
            bool works5 = Attribute.IsDefined(f_vi, typeof(AlternativeNamesAttribute));

            //this works.
            PropertyInfo asPropInfo = (PropertyInfo)p_vi;
            FieldInfo asFieldInfo = (FieldInfo)f_vi;

            //also works
            VariableInfo_CUSTOM propBackToVarInfo = (VariableInfo_CUSTOM)asPropInfo;
            VariableInfo_CUSTOM fieldBackToVarInfo = (VariableInfo_CUSTOM)asFieldInfo;

            try
            {

                bool usedToFailThankYouStackOverflow = Attribute.IsDefined(p_vi, typeof(AlternativeNamesAttribute));
            }
            catch { }

            foreach (var v in this.GetType().GetAllVariables_CUSTOM())
            {
                if (v.Name == "Aardvark")
                    this.SaveToVar(v, "ants");
                else if (v.IsAttributeDefined(typeof(AlternativeNamesAttribute)) && v.GetCustomAttribute<AlternativeNamesAttribute>().AlternateNames.Contains("Aardvark"))
                    this.SaveToVar(v, "ants");
                else if (v.Name == "HouseCat")
                    v.SetValue(this, "mouse");
                else if (v.IsAttributeDefined(typeof(AlternativeNamesAttribute)) && v.GetCustomAttribute<AlternativeNamesAttribute>().AlternateNames.Contains("HouseCat"))
                    v.SetValue(this, "mouse");
            }
            if (property != "ants")
                throw new Exception("i am a terrible programmer");
            if (field != "mouse")
                throw new Exception("i am a terrible programmer");
        }
    }
   
}
