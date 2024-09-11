
using MovieBringer.Core.Models.MovieModels;

namespace MovieBringer.Core.Models.ViewModel.Home
{
    public class HomePageViewModel
    {
        public List<Movie>? TrendingInWeeks { get; set; }
        public List<Movie>? NowPlayings{ get; set; }

        public List<Movie>? UpComing { get; set; }

        public List<Movie>? Popular { get; set; }

        public List<Movie>? TopRated { get; set; }

        public ListDetailViewModel MostPopulerList { get; set; }
        public ListDetailViewModel LastAddedList { get; set; }
    }

}
