using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace Hosted.Shared
{
    [DataContract(Namespace = "")]
    public class Form
    {
        [DataMember(Name = "elements")]
        public List<Element> Elements { get; set; }
    }
}
