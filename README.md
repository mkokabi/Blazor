# Blazor
## Introduction, Blazor 3 Models:
### WebAssembly
As it's name says this model is utilizing the [*WebAssembly*](https://webassembly.org/). The *WebAssembly* is a modern browser's feature which in short is the capability to support compiled high level languages such as C++ as binary instruction to run directly in the browser. This feature allows developers to use languages other than JavaScript for UI. Microsoft's implementation is instructing DotNet Core in WebAssembly to allow C# directly be used in the browser. At run time the DotNet Core and the project assemblies would get downloaded. 
![dlls](https://github.com/mkokabi/Blazor/blob/master/images/dlls.png?raw=true)

The template for this model is *blazorwasm*. If you don't have it, install it using:
```powershell
dotnet new -i Microsoft.AspNetCore.Blazor.Templates::3.0.0-preview9.19424.4
```
The main part of code is in defining the host in *Program.cs*
```csharp
public static IWebAssemblyHostBuilder CreateHostBuilder(string[] args) =>
    BlazorWebAssemblyHost.CreateDefaultBuilder()
                .UseBlazorStartup<Startup>();
```
The startup only has to define the starting component in *Configure* method:
```csharp
app.AddComponent<App>("app");
```
Similar to client side frameworks *app* component is used in the *index.html*
```html
<app>Loading...</app>
```

### WebAssembly Hosted
  This model is using the same concepts as above but it would create one separate DotNet core application to host and another DotNet standard application which has all the client side elements and components. 
The template for this model is *blazorwasm --hosted*.
The Server application would use the Startup of the client project in *Configure* method.
```csharp
app.UseClientSideBlazorFiles<Client.Startup>();
```
The client startup is just like the one in the above model. It would also add an endpoint mapping:
```csharp
endpoints.MapFallbackToClientSideBlazor<Client.Startup>("index.html");
```

### Blazor Server
  In this model, the application would be running entirely on DotNet Core on server and all the UI interaction would communicate to the server using SignalR. This model has some downsides like the application would be very chatty as every UI interaction would go back to server and also server has to maintain the state for each client so special care is needed to manage the memory consumption on the server.
The template for creating this model is *blazorserver*

## Sample
In this sample, we are going to create a form based on the layout stored on the server. The 2nd model (WebAssembly Hosted) is used as it has a better separation of client and server and also has the shared elements in separated project.

```powershell
dotnet new blazorwasm --hosted -o Hosted
```
## Shared classes
Add these classes to the *Hosted.Shared* project:
```csharp
public class Element
{
    public virtual string ElementType { get; set; }

    public string Name { get; set; }

    public string Label { get; set; }
}

public class TextInput : Element
{
    public override string ElementType { get => "TextInput"; }

    public string PlaceHolder { get; set; }
}

public class RadioButton : Element
{
    public override string ElementType { get => "RadioButton"; }

    public Dictionary<string, string> Options { get; set; }

}
public class Form
{
    public List<Element> Elements { get; set; }
}
```

## Server side
In *Hosted.Server* project edit the *ConfigureServices* method from
```csharp
services.AddMvc().AddNewtonsoftJson();
```
to
```csharp
services.AddMvc().AddNewtonsoftJson(options =>
{
    options.SerializerSettings.TypeNameHandling = Newtonsoft.Json.TypeNameHandling.Auto;
});

```
This will add the types of unknown elements while serialization:
![json](https://github.com/mkokabi/Blazor/blob/master/images/json.png?raw=true)

Now we can create a controller to return a *Form*:
```csharp
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
```
## Client side
To demonstrate the components which are the core of **Blazor** we are creating a corresponding component for each element. Component's name is coming from their file name (it should starts with Capital letter). They are a Html with a *@code* section which would have all the events, properties, logic and other things. While the syntax is similar to ASP.Net razor, under the hood it's technically very different. 

We are creating a folder called *Components* at the same level of *Pages*. For this sample we would only create *TextInput.razor* and *RadioButton.razor*.

### TextInput.razor
```html
<input type="text" name="@Name" placeholder="@PlaceHolder">

@code {
    [Parameter]
    public string Name { get; set; }
    
    [Parameter]
    public string PlaceHolder { get; set; }
}
```

### RadioButton.razor
```html
@foreach (var option in @Options)
{
    <input type="radio" id="@option.Key" name="@Name" value="@option.Key">
    <label for="male">@option.Value</label>
    <br>
}

@code {
    [Parameter]
    public string Name { get; set; }

    [Parameter]
    public Dictionary<string, string> Options { get; set; }
}
```

Add a page called *DynamicForm.razor* under Pages folder. 
```html
@page "/dynamicform"
@using Newtonsoft.Json
@using Hosted.Shared
@inject HttpClient Http

<h1>Dynamic form</h1>


@if (form == null)
{
    <p><em>Loading...</em></p>
}
else
{
  <table class="table">
    <tbody>
      @foreach (var element in form.Elements)
      {
        <tr>
          <td>@element.Label</td>
          @switch (element.ElementType)
          {
            case "TextInput":
            {
              <td><Hosted.Client.Components.TextInput Name="@element.Name" PlaceHolder="@((element as TextInput).PlaceHolder)" /></td>
              break;
            }
            case "RadioButton":
            {
              <td><Hosted.Client.Components.RadioButton Name="@element.Name" Options="@((element as RadioButton).Options)" /></td>
              break;
            }
            default:
            {
              <td>Unknow control</td>
              break;
              }
            }
        </tr>
      }
    </tbody>
  </table>
}

@code {
    Form form;

    protected override async Task OnInitializedAsync()
    {
        var st = await Http.GetStringAsync("Form");

        form = JsonConvert.DeserializeObject<Form>(st, settings: new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.Auto });
    }

}
```

Use this page in *NavMenu.razor* by adding a *li* as:
```html
<li class="nav-item px-3">
    <NavLink class="nav-link" href="dynamicform">
        <span class="oi oi-list-rich" aria-hidden="true"></span> dynamic form
    </NavLink>
</li>
```

The output would be like:
![running](https://github.com/mkokabi/Blazor/blob/master/images/form.png?raw=true)
