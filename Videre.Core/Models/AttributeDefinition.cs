﻿using System.Collections.Generic;
using CodeEndeavors.Extensions;

namespace Videre.Core.Models
{
    public class AttributeDefinition 
    {
        public AttributeDefinition()
        {
            Values = new List<string>();
        }

        public string LabelKey { get; set; }
        public string LabelText { get; set; }
        public string Name { get; set; }
        public bool Required { get; set; }  //todo!

        public List<string> Values { get; set; }    //todo: if values is set then dropdown, otherwise textbox???
        public object DefaultValue { get; set; }
    }

}