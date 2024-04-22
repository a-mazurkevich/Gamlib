using Gamlib.ViewModels.Base;

namespace Gamlib.Pages.Base;

public abstract class BasePage : ContentPage
{
    protected BasePageViewModel? BaseBinding => BindingContext as BasePageViewModel;

    protected override void OnAppearing()
    {
        base.OnAppearing();
        BaseBinding?.OnAppearing();
    }

    protected override void OnDisappearing()
    {
        base.OnDisappearing();
        BaseBinding?.OnDisappearing();
    }
}

