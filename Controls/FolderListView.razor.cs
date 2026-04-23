using DalleR.Models;
namespace DalleR.Controls;

public partial class FolderListView
{
    public override bool HasActions => false;

    private bool addingFolder = false;
    private string newFolderPath = "";

    private async Task GoToDashboard()
    {
        var result = await RunAction<DashboardPayload>("ShowDashboard");
        await ParentFrame.Show(result);
    }

    private async Task ConfirmAddFolder()
    {
        if (string.IsNullOrWhiteSpace(newFolderPath)) return;
        var req = new AddFolderRequest { FolderPath = newFolderPath };
        var result = await RunAction<FolderListPayload>("AddFolder", req);
        newFolderPath = "";
        addingFolder = false;
        await ParentFrame.Show(result);
    }

    private async Task RemoveFolder(string path)
    {
        var req = new FolderPathRequest { FolderPath = path };
        var result = await RunAction<FolderListPayload>("RemoveFolder", req);
        await ParentFrame.Show(result);
    }

    private async Task ScanFolder(string path)
    {
        var req = new FolderPathRequest { FolderPath = path };
        var result = await RunAction<FolderListPayload>("ScanFolder", req);
        await ParentFrame.Show(result);
    }
}
