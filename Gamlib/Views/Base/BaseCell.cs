using Gamlib.ViewModels.Base;

namespace Gamlib.Views.Base;

public abstract class BaseCell<T> : ContentView where T : BaseViewModel
{
	public BaseCell()
	{
		Content = BuildLayout();
    }

	protected abstract View BuildLayout();
}

