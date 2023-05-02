using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MoodMate.Components.Factory;
using System.Collections.ObjectModel;

namespace MoodMate.ViewModels;

public partial class AnalysisMoodViewModel : ObservableObject
{
    private readonly Note MoodNote;
    public AnalysisMoodViewModel(Note[] note)
    {
        MoodNote = note[0];
    }

    [ObservableProperty] DateTime selectedDate = new(DateTime.Now.Year, DateTime.Now.Month, 1);
    [ObservableProperty] KeyValuePair<string, (string, int, int)> template = new();
    [ObservableProperty] int count = new();
    public ObservableCollection<KeyValuePair<string, (string, int, int)>> AnalysisMood { get; set; } = new();

    [RelayCommand]
    void NextMonth()
    {
        if (DateTime.Parse(SelectedDate.Month + ".01." + SelectedDate.Year) <
            DateTime.Parse(DateTime.Now.Month + ".01." + DateTime.Now.Year))
        {
            SelectedDate = SelectedDate.AddMonths(1);
            UpdateAnalyse();
        }
    }

    [RelayCommand]
    void PreviousMonth()
    {
        if (DateTime.Parse(SelectedDate.Month + ".01." + SelectedDate.Year) >
            DateTime.Parse("01.01.2023"))
        {
            SelectedDate = SelectedDate.AddMonths(-1);
            UpdateAnalyse();
        }
    }

    [RelayCommand]
    void UpdateAnalyse()
    {
        MoodNote.note.InitAnalyse(SelectedDate);
        MoodNote.note.FindPercentsMood();
        Count = MoodNote.note.GetCountMood();

        var items = MoodNote.note.GetAnalysedData();
        AnalysisMood.Clear();
        foreach (var item in items)
        {
            AnalysisMood.Add(item);
        }
        OnPropertyChanged(nameof(AnalysisMood));
    }

    [RelayCommand]
    async void BackClick()
    {
        await Shell.Current.Navigation.PopAsync();
    }
}
