using Hosted.Shared;
using Microsoft.AspNetCore.Components;
using Newtonsoft.Json;
using System.Net.Http;
using System.Threading.Tasks;

namespace Hosted.Client.Pages
{
    public partial class DynamicForm
    {
        Form form;

        [Inject]
        protected HttpClient Http { get; set; }

        protected override async Task OnInitializedAsync()
        {
            var st = await Http.GetStringAsync("Form");

            form = JsonConvert.DeserializeObject<Form>(st, settings: new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.Auto });
        }
    }
}
