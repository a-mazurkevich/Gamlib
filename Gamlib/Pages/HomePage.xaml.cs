using Gamlib.Helpers;
using Gamlib.ViewModels;

namespace Gamlib.Pages;

public partial class HomePage
{
	public HomePage()
	{
		InitializeComponent();
        BindingContext = ServiceHelper.GetService<HomePageViewModel>()!;
    }
}
