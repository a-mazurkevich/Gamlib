using Gamlib.ViewModels.Base;

namespace Gamlib.ViewModels
{
	public class GameCellViewModel : BaseViewModel
	{
		public required string Title { get; set; }

        public required double Rating { get; set; }

        public required string ImageUrl { get; set; }
	}
}

