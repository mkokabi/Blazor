using Hosted.Shared;
using Microsoft.AspNetCore.Components;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace Hosted.Client.Pages
{
    public partial class DynamicForm
    {
        Form form;
        internal static readonly Dictionary<string, string> ElementValues = new Dictionary<string, string>();

        [Inject]
        protected HttpClient Http { get; set; }

        protected override async Task OnInitializedAsync()
        {
            var st = await Http.GetStringAsync("Form");

            form = JsonConvert.DeserializeObject<Form>(st, settings: new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.Auto });
        }

        private async Task Submit()
        {
            await Http.PostJsonAsync("Form", ElementValues);
        }
    }
}
