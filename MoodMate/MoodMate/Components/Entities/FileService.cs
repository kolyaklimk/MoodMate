﻿using MoodMate.Components.Data;
using MoodMate.Components.Entities.Abstractions;

namespace MoodMate.Components.Entities;

public class FileService : IFileService
{
    private DataLoading<FileService> ServiceLoading = new();
    public string Name { get; set; }
    public string Source { get; set; }
    public string Description { get; set; }

    public FileService() { }
    public async Task LoadService()
    {
        await ServiceLoading.Load(Constants.PathMoods, true);
    }
    public List<FileService> GetServiceData()
    {
        return ServiceLoading.Data;
    }
}