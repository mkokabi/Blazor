using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace Hosted.Shared
{
    [DataContract(Namespace = "")]
    public class RadioButton : Element
    {
        public override string ElementType { get => "RadioButton"; }

        [DataMember(Name = "options")]
        public Dictionary<string, string> Options { get; set; }

    }
}
