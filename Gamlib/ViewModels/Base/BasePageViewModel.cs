namespace Gamlib.ViewModels.Base
{
    public abstract class BasePageViewModel : BaseViewModel
    {
        private string? _title;
        public string? Title
        {
            get => _title;
            set => SetProperty(ref _title, value);
        }

        private bool _isLoading = false;
        public bool IsLoadong
        {
            get => _isLoading;
            set => SetProperty(ref _isLoading, value);
        }

        public BasePageViewModel()
        {
        }

        public virtual async void OnAppearing()
        {
            IsLoadong = true;
            await LoadData();
            IsLoadong = false;
        }

        public virtual void OnDisappearing()
        {
            
        }

        protected virtual Task LoadData()
        {
            return Task.CompletedTask;
        }
    }
}

