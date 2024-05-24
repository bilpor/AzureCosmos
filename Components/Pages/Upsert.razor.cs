using AzureCosmos.Data;
using AzureCosmos.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;

namespace AzureCosmos.Components.Pages;

public partial class Upsert
{
    public Engineer engineer = new Engineer();
  

    [Inject]
    public IEngineerService engineerService { get; set; }

    [Inject]
    public required NavigationManager NavManager { get; set; }

    [Parameter]
    public string? id { get; set; }

    protected override async Task OnInitializedAsync()
    {
        

        if (!string.IsNullOrWhiteSpace(id))
            engineer = await engineerService.GetEngineerDetailsById(id, id);

        
    }

    private async Task SaveEngineer()
    {
        await engineerService.UpsertEngineer(engineer);
        NavManager.NavigateTo("/");
    }
}
