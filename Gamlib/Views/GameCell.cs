using Gamlib.ViewModels;
using Gamlib.Views.Base;
using Microsoft.Maui.Controls.Shapes;

namespace Gamlib.Views;

public class GameCell : BaseCell<GameCellViewModel>
{
    protected override View BuildLayout()
    {
        var title = new Label
        {
            HorizontalOptions = LayoutOptions.Start,
            VerticalOptions = LayoutOptions.Center,
            MaxLines = 2,
        };

        title.SetBinding(Label.TextProperty, nameof(GameCellViewModel.Title));

        var rating = new Label
        {
            HorizontalOptions = LayoutOptions.End,
            VerticalOptions = LayoutOptions.Center,
        };

        rating.SetBinding(Label.TextProperty, nameof(GameCellViewModel.Rating));

        var cellContent = new Grid
        {
            Margin = new Thickness { Right = 8 },
            ColumnSpacing = 8,
            ColumnDefinitions =
            {
                new ColumnDefinition { Width = 90 },
                new ColumnDefinition { Width = GridLength.Star },
                new ColumnDefinition { Width = 60 },
            },
            RowDefinitions =
            {
                new RowDefinition { Height = 60 },
            }
        };

        var image = new Image
        {
            Aspect = Aspect.AspectFill,
        };
        image.SetBinding(Image.SourceProperty, nameof(GameCellViewModel.ImageUrl));

        cellContent.Add(image, 0, 0);
        cellContent.Add(title, 1, 0);
        cellContent.Add(rating, 2, 0);

        var border = new Border
        {
            Content = cellContent,
            BackgroundColor = Colors.White,
            Margin = new Thickness(8, 4),
            Stroke = Brush.Transparent,
            StrokeShape = new RoundRectangle
            {
                CornerRadius = 8,
            },
            Shadow = new Shadow
            {
                Brush = Colors.LightGray,
                Offset = new Point(2, 1),
                Radius = 4f,
                Opacity = 0.6f,
            }
        };


        return border;
    }
}

public class GameCellTemplate : DataTemplate
{
    public GameCellTemplate() : base(() => new GameCell())
    {
    }
}
