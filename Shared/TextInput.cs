using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace Hosted.Shared
{
    [DataContract(Namespace = "")]
    public class TextInput : Element
    {
        public override string ElementType { get => "TextInput"; }

        [DataMember(Name = "placeHolder")]
        public string PlaceHolder { get; set; }
    }
}
