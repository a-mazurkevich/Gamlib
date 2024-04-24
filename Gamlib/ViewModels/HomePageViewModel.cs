using System.Windows.Input;
using Gamlib.Collections;
using Gamlib.Commands;
using Gamlib.Services.Api;
using Gamlib.ViewModels.Base;

namespace Gamlib.ViewModels;

public class HomePageViewModel : BasePageViewModel
{
    private readonly IGamesApi _gamesApi;

    private int _page = 1;
    private bool _isFinalPage = false;

    private TaskCompletionSource<bool>? _gamesPaginationLoadingSource;

    public HomePageViewModel(IGamesApi gamesApiService)
    {
        _gamesApi = gamesApiService;
        Title = "Games";
    }

    private bool _isRefreshing = false;
    public bool IsRefreshing
    {
        get => _isRefreshing;
        set => SetProperty(ref _isRefreshing, value);
    }

    private ObservableRangeCollection<GameCellViewModel>? _items;
    public ObservableRangeCollection<GameCellViewModel>? Items
    {
        get => _items;
        set => SetProperty(ref _items, value);
    }

    public ICommand LoadMoreCommand => new AsyncCommand(OnLoadMoreCommand,
        canExecute: () => _gamesPaginationLoadingSource?.Task.IsCompleted ?? true);

    public ICommand RefreshCommand => new AsyncCommand(OnRefreshCommand,
        canExecute: () => _gamesPaginationLoadingSource?.Task.IsCompleted ?? true);

    protected override Task LoadData()
    {
        return LoadGames(_page);
    }

    private async Task OnLoadMoreCommand()
    {
        if (!_isFinalPage)
        {
            await LoadGames(_page + 1);
        }
    }

    private async Task OnRefreshCommand()
    {
        IsRefreshing = true;
        await LoadGames(1);
        IsRefreshing = false;
    }

    private async Task LoadGames(int page)
    {
        try
        {
            _gamesPaginationLoadingSource = new TaskCompletionSource<bool>();

            var result = await _gamesApi.GetGames(page: page);

            var gamesViewModels = result.Results!.Select(g => new GameCellViewModel
            {
                Title = g.Name ?? "NO DATA",
                ImageUrl = g.BackgroundImage ?? "",
                Rating = g.Rating,
            });

            _isFinalPage = result.Next == null;

            if (_page == 1)
            {
                Items = new ObservableRangeCollection<GameCellViewModel>(gamesViewModels);
            }
            else
            {
                Items?.AddRange(gamesViewModels);
            }

            _page = page;
            _gamesPaginationLoadingSource.SetResult(true!);
        }
        catch (Exception e)
        {
            _gamesPaginationLoadingSource?.SetException(e);
        }
    }
}

