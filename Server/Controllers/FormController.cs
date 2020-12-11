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
                        Name = "txtFName",  Label = "First Name", PlaceHolder="Enter your first name"
                    },
                    new TextInput
                    {
                        Name = "txtLName",  Label = "Last Name", PlaceHolder="Enter your last name",
                        Value="Kokabi"
                    },
                    new RadioButton 
                    { 
                        Name = "radGender", Label = "Gender", 
                        Options = new Dictionary<string, string> { { "M", "Male" }, {"F", "Female"} }                        
                    }
                }
            };
        }

        [HttpPost]
        public string Submit([FromBody] Dictionary<string, string> formValues)
        {
            return $"Hello {formValues["txtFName"]} {formValues["txtLName"]}";
        }
    }
}