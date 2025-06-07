using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViFlix.Core.ViewModels.MoviesViewModel;
using ViFlix.Data.Movies;
using ViFlix.Data.Repository;

namespace ViFlix.Core.Services.Movies.ReviewServices
{
    public interface IReviewsServices : IGenericRepository<Reviews>
    {
        Task<IEnumerable<DisplayReviewViewModel>> GetReviewsForMovieAsync(long movieId);
        Task<IEnumerable<DisplayReviewViewModel>> GetReviewsForSeriesAsync(long seriesId);
        Task<DisplayReviewViewModel> AddReviewAsync(CreateReviewViewModel review, long userId);
        Task<bool> ApproveReviewAsync(long reviewId);
        Task<bool> DeleteReviewAsync(long reviewId);
    }
}
