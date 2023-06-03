using CommunityToolkit.Maui.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using MoodMate.Components.Entities;
using MoodMate.Components.Entities.Abstractions;
using MoodMate.Components.Factory;
using MoodMate.Messages;
using MoodMate.Pages.MoodNote;
using MoodMate.Pages.Other;
using MoodMate.Templates;
using System.Collections.ObjectModel;

namespace MoodMate.ViewModels;

public partial class AnalysisMoodViewModel : ObservableObject
{
    private readonly MoodNote MoodNote;
    private readonly MyKeyValue ForCollection = new();
    private readonly IUser User;
    private readonly UpdateMoodNoteMessage UpdateMoodNoteMessage;
    private bool IsFirst = true;
    public ObservableCollection<MyKeyValue> AnalysisMood { get; set; } = new();

    [ObservableProperty] DateTime selectedDate = new(DateTime.Now.Year, DateTime.Now.Month, 1);
    [ObservableProperty] int count;
    [ObservableProperty] bool isRefreshing = false;

    public AnalysisMoodViewModel(Note[] note, IUser user,
        UpdateMoodNoteMessage update)
    {
        MoodNote = note[0].note;
        User = user;
        UpdateMoodNoteMessage = update;
    }


    [RelayCommand]
    async Task NextMonth()
    {
        IsRefreshing = true;

        if (DateTime.Parse(SelectedDate.Month + ".01." + SelectedDate.Year) <
            DateTime.Parse(DateTime.Now.Month + ".01." + DateTime.Now.Year))
        {
            SelectedDate = SelectedDate.AddMonths(1);
            await UpdateAnalyse();
        }

        IsRefreshing = false;
    }

    [RelayCommand]
    async Task PreviousMonth()
    {
        IsRefreshing = true;

        if (DateTime.Parse(SelectedDate.Month + ".01." + SelectedDate.Year) >
            DateTime.Parse("01.01.2023"))
        {
            SelectedDate = SelectedDate.AddMonths(-1);
            await UpdateAnalyse();
        }

        IsRefreshing = false;
    }

    [RelayCommand]
    async Task UpdateAnalyse()
    {
        IsRefreshing = true;
        try
        {
            await MoodNote.InitAnalyse(SelectedDate, User.Client.User);

            await Task.Run(() =>
            {
                MoodNote.FindPercentsMood();
                Count = MoodNote.GetCountMood();

                var items = MoodNote.GetAnalysedData();

                AnalysisMood.Clear();
                foreach (var item in items)
                {
                    AnalysisMood.Add(item);
                }
            });


            if (IsFirst)
            {
                if (AnalysisMood.Count == 0)
                {
                    AnalysisMood.Add(new MyKeyValue());
                    AnalysisMood.RemoveAt(0);
                }
                IsFirst = false;
            }
        }
        catch
        {
            if ((bool)await Shell.Current.ShowPopupAsync(new GoOfflinePage()))
            {
                User.SignOut();
                MoodNote.ClearNotes();
                await BackClick();
                WeakReferenceMessenger.Default.Send(UpdateMoodNoteMessage);
            }
        }
        IsRefreshing = false;
    }

    [RelayCommand]
    public async Task BackClick()
    {
        await Shell.Current.GoToAsync("//" + nameof(MoodListPage), false);
        SelectedDate = new(DateTime.Now.Year, DateTime.Now.Month, 1);
    }
}
