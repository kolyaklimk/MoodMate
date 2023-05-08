using CommunityToolkit.Mvvm.ComponentModel;
using MoodMate.Components.Data;
using MoodMate.Components.Entities.Abstractions;

namespace MoodMate.Components.Entities;

public partial class FileService : ObservableObject, IFileService
{
    private DataLoading<FileService> ServiceLoading = new();
    [ObservableProperty] public string name;
    [ObservableProperty] public string source;
    [ObservableProperty] public string description;

    public FileService() { }
    public async Task LoadService(string path)
    {
        await ServiceLoading.Load(path, true);
    }
    public List<FileService> GetServiceData()
    {
        return ServiceLoading.Data;
    }
}