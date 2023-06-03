using MoodMate.ViewModels.Other;

namespace MoodMate.Pages.Other;

public partial class AuthenticationPage : ContentPage
{
    public AuthenticationPage(AuthenticationViewModel model)
    {
        InitializeComponent();
        BindingContext = model;
    }
}