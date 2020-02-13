using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Hosted.Shared;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Hosted.Server.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class FormController : ControllerBase
    {
        [HttpGet]
        public Form Get()
        {
            return new Form 
            { 
                Elements = new List<Element> 
                { 
                    new TextInput 
                    { 
                        Name = "txtName",  Label = "Name", PlaceHolder="Enter your name"
                    },
                    new RadioButton 
                    { 
                        Name = "radGender", Label = "Gender", 
                        Options = new Dictionary<string, string> { { "M", "Male" }, {"F", "Female"} } 
                    }
                }
            };
        }
    }
}