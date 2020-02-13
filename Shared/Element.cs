using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace Hosted.Shared
{
    [DataContract(Namespace = "")]
    [KnownType(typeof(TextInput))]
    [KnownType(typeof(RadioButton))]
    public class Element
    {
        [DataMember(Name = "elementType")]
        public virtual string ElementType { get; set; }

        [DataMember(Name = "name")]
        public string Name { get; set; }

        [DataMember(Name = "label")]
        public string Label { get; set; }
    }
}
