using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MoodMate.Components.Entities;
using System.Collections.ObjectModel;

namespace MoodMate.ViewModels.Music;

public partial class MusicListViewModel : ObservableObject
{
    private readonly FileService Music;
    private readonly FileService Sound;
    public MusicListViewModel(FileService[] fileService)
    {
        Music = fileService[1];
        Sound = fileService[2];
    }
    public ObservableCollection<FileService> MusicList { get; set; } = new();
    public ObservableCollection<FileService> SoundList { get; set; } = new();

    [ObservableProperty] FileService selectedMusic;
    [ObservableProperty] FileService selectedSound;
    [ObservableProperty] TimeSpan selectedTime=new(0,1,0);

    [RelayCommand]
    void LoadPage()
    {
        var M = Music.GetServiceData();
        var S = Sound.GetServiceData();
        
        foreach(var m in M) 
        {
            MusicList.Add(m);
        }
        foreach (var s in S)
        {
            SoundList.Add(s);
        }
    }

    [RelayCommand]
    async void GoToPlay()
    {
        if(SelectedMusic!=null && SelectedSound != null)
        {
            SelectedMusic = null;
            SelectedSound = null;
        }
    }
}
