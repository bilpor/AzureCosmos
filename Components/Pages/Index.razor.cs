using AzureCosmos.Data;
using AzureCosmos.Services;
using Microsoft.AspNetCore.Components;

namespace AzureCosmos.Components.Pages;

public partial class Index
{
    [Inject]
    public IEngineerService engineerService { get; set; }

    private List<Engineer> engineers = new();
    private bool fetchingData = false;

    protected override async Task OnInitializedAsync()
    {
        fetchingData = true;
        engineers = await engineerService.GetEngineerDetails();
        fetchingData = false;
    }

    protected async Task DeleteEngineer(Guid? id)
    {
        await engineerService.DeleteEngineer(id.ToString(), id.ToString());
        engineers = await engineerService.GetEngineerDetails();
    }
}
