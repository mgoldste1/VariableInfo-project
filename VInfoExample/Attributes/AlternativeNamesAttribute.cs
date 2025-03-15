using System;
using System.Collections.Generic;
using System.Linq;

namespace VInfoExample.Attributes
{
    [AttributeUsage(AttributeTargets.All, AllowMultiple = false)]
    public class AlternativeNamesAttribute : Attribute
    {
        public List<string> AlternateNames = new List<string>();
        public AlternativeNamesAttribute(params string[] AlternateNamesIn)
        {
            AlternateNames = AlternateNamesIn.Select(o => o.Trim()).ToList();
        }
    }
}

