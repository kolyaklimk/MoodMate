using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MoodMate.Components.Factory;
using MoodMate.Pages.MoodNote;
using MoodMate.Templates;
using System.Collections.ObjectModel;

namespace MoodMate.ViewModels;

public partial class AnalysisMoodViewModel : ObservableObject
{
    private readonly Note MoodNote;
    private bool IsFirst = true;
    private readonly MyKeyValue ForCollection = new();
    public AnalysisMoodViewModel(Note[] note)
    {
        MoodNote = note[0];
    }

    [ObservableProperty] DateTime selectedDate = new(DateTime.Now.Year, DateTime.Now.Month, 1);
    [ObservableProperty] int count;
    public ObservableCollection<MyKeyValue> AnalysisMood { get; set; } = new();

    [RelayCommand]
    async Task NextMonth()
    {
        if (DateTime.Parse(SelectedDate.Month + ".01." + SelectedDate.Year) <
            DateTime.Parse(DateTime.Now.Month + ".01." + DateTime.Now.Year))
        {
            SelectedDate = SelectedDate.AddMonths(1);
            await UpdateAnalyse();
        }
    }

    [RelayCommand]
    async Task PreviousMonth()
    {
        if (DateTime.Parse(SelectedDate.Month + ".01." + SelectedDate.Year) >
            DateTime.Parse("01.01.2023"))
        {
            SelectedDate = SelectedDate.AddMonths(-1);
            await UpdateAnalyse();
        }
    }

    [RelayCommand]
    async Task UpdateAnalyse()
    {
        await MoodNote.note.InitAnalyse(SelectedDate);

        await Task.Run(() =>
        {
            MoodNote.note.FindPercentsMood();
            Count = MoodNote.note.GetCountMood();

            var items = MoodNote.note.GetAnalysedData();

            AnalysisMood.Clear();
            foreach (var item in items)
            {
                AnalysisMood.Add(item);
            }

            if (IsFirst)
            {
                if (AnalysisMood.Count == 0)
                {
                    AnalysisMood.Add(new MyKeyValue());
                    AnalysisMood.RemoveAt(0);
                }
                IsFirst = false;
            }
        });
    }

    [RelayCommand]
    public async Task BackClick()
    {
        await Shell.Current.GoToAsync("//" + nameof(MoodListPage), false);
        SelectedDate = new(DateTime.Now.Year, DateTime.Now.Month, 1);
    }
}
