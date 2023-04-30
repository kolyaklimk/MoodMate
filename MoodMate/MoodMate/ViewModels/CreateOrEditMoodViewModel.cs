using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MoodMate.Components.Entities;
using MoodMate.Components.Factory;

namespace MoodMate.ViewModels;

[QueryProperty(nameof(MoodNote), "Note")]
[QueryProperty(nameof(SelectedMood), "MoodNote")]
public partial class CreateOrEditMoodViewModel : ObservableObject
{
    private readonly Note MoodNote;

    [ObservableProperty] public MoodNote selectedMood;

}
