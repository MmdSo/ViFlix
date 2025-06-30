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
        DisplayReviewViewModel GetReviewById(long id);
        Task<IEnumerable<DisplayReviewViewModel>> GetReviewsAsync();
        Task<IEnumerable<DisplayReviewViewModel>> GetReviewsForMovieAsync(long movieId);
        Task<IEnumerable<DisplayReviewViewModel>> GetReviewsForSeriesAsync(long seriesId);
        Task<DisplayReviewViewModel> AddReviewAsync(CreateReviewViewModel review, long userId);
        Task<bool> ApproveReviewAsync(long reviewId, bool isApproved);
        Task EditReview(DisplayReviewViewModel review);
        Task<bool> DeleteReviewAsync(long reviewId);
    }
}
