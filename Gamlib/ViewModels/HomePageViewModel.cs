using System.Threading.Tasks;
using System.Windows.Input;
using Gamlib.Collections;
using Gamlib.Commands;
using Gamlib.Models.Api;
using Gamlib.Services.Api;
using Gamlib.ViewModels.Base;

namespace Gamlib.ViewModels;

public class HomePageViewModel : BasePageViewModel
{
    private readonly IGamesApi _gamesApi;

    private int _page = 1;
    private bool _isFinalPage = false;
    private TaskCompletionSource<GamesChunkModel>? _taskCompletionSource; 

    public HomePageViewModel(IGamesApi gamesApiService)
    {
        _gamesApi = gamesApiService;
        Title = "Games";
    }

    private ObservableRangeCollection<GameCellViewModel>? _items;
    public ObservableRangeCollection<GameCellViewModel>? Items
    {
        get => _items;
        set => SetProperty(ref _items, value);
    }

    public ICommand LoadMoreCommand => new AsyncCommand(OnLoadMoreCommand,
        canExecute: () => _taskCompletionSource?.Task.IsCompleted ?? true);

    protected override Task LoadData()
    {
        return LoadGames(_page);
    }

    private async Task LoadGames(int page)
    {
        try
        {
            _taskCompletionSource = new TaskCompletionSource<GamesChunkModel>();

            var result = await _gamesApi.GetGames(page: page);

            var gamesViewModels = result.Results!.Select(g => new GameCellViewModel
            {
                Title = g.Name ?? "NO DATA",
                ImageUrl = "",//g.BackgroundImage ?? "",
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
            _taskCompletionSource.SetResult(result!);

        }
        catch (Exception e)
        {
            System.Diagnostics.Debug.WriteLine($"ERROR.... {e.Message}");
            _taskCompletionSource?.SetException(e);
        }
    }

    private async Task OnLoadMoreCommand()
    {
       if (!_isFinalPage)
        {
            var p =_page + 1;
            System.Diagnostics.Debug.WriteLine($"LOADING.... {p}");

            await LoadGames(_page + 1);
            System.Diagnostics.Debug.WriteLine($"LOADED!!! {p}");

        }
    }
}

