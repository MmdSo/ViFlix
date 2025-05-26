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
        IEnumerable<ReviewViewModel> GetAllReviews();
        ReviewViewModel GetReviwById(long? id);
        List<ReviewViewModel> GetAllMovieReviewByMovieId(long? id);
        ReviewViewModel GetReviewByMovieId(long? id);
        bool ApprovedReview(long? id, bool isApproved);
        Task<long> AddReview(ReviewViewModel review);
        Task EditReview(ReviewViewModel review);
        void DeleteReview(ReviewViewModel review);
        Task SendReview(ReviewViewModel review);
    }
}
