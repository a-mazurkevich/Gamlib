using Gamlib.Helpers;
using Gamlib.Pages;
using Gamlib.Services;

namespace Gamlib;

public partial class App : Application
{
    public App()
    {
        InitializeComponent();
        MainPage = new NavigationPage(new HomePage());
    }

    protected override void OnStart()
    {
        base.OnStart();
        ServiceHelper.GetService<ImageCacheService>().Initialize();
    }
}
