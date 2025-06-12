using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViFlix.Core.Services.User.UserServices;
using ViFlix.Core.ViewModels.MoviesViewModel;
using ViFlix.Data.Context;
using ViFlix.Data.Movies;
using ViFlix.Data.Repository;

namespace ViFlix.Core.Services.Movies.ReviewServices
{
    public class ReviwsServices : GenericRepository<Reviews>, IReviewsServices
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        public readonly IUserService _userServices;
        public ReviwsServices(AppDbContext context, IMapper mapper , IUserService userServices) : base(context)
        {
            _context = context;
            _mapper = mapper;
            _userServices = userServices;
        }

        public async Task<DisplayReviewViewModel> AddReviewAsync(CreateReviewViewModel review, long userId)
        {
            if (!review.MovieId.HasValue && !review.SeriesId.HasValue)
            {
                throw new ArgumentException("you should write a comment!");
            }
            var re = _mapper.Map<Reviews>(review);

            re.UserId = userId;
            re.CreateAt = DateTime.UtcNow;
            re.IsApproved = false;

            await _context.Reviews.AddAsync(re); 
            await _context.SaveChangesAsync();


            var displayModel = _mapper.Map<DisplayReviewViewModel>(re);
            var user = _userServices.GetUserById(re.UserId);

            if (user != null)
            {
                displayModel.UserName = user.UserName; 
                displayModel.Avatar = user.avatar; 
            }

            return displayModel;
        }

        public async Task<bool> ApproveReviewAsync(long reviewId)
        {
            var review = await _context.Reviews.FindAsync(reviewId);

            if (review == null)
            {
                return false;
            }

            review.IsApproved = true;

            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> DeleteReviewAsync(long reviewId)
        {
           
            var review = await _context.Reviews.FindAsync(reviewId);

            
            if (review == null)
            {
                return false;
            }

            
            _context.Reviews.Remove(review);

            
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<IEnumerable<DisplayReviewViewModel>> GetReviewsForMovieAsync(long movieId)
        {
            var reviews = await _context.Reviews.Include(r => r.users).Include(r => r.Replies).ThenInclude(reply => reply.users)
                .Where(r => r.MovieId == movieId && r.IsApproved && r.ParentId == null).OrderByDescending(r => r.CreateAt).ToListAsync();

            return _mapper.Map<IEnumerable<DisplayReviewViewModel>>(reviews);
        }

        public async Task<IEnumerable<DisplayReviewViewModel>> GetReviewsForSeriesAsync(long seriesId)
        {
            var reviews = await _context.Reviews
            .Include(r => r.users)
            .Include(r => r.Replies)
                .ThenInclude(reply => reply.users)
            .Where(r => r.SeriesId == seriesId && r.IsApproved && r.ParentId == null)
            .OrderByDescending(r => r.CreateAt)
            .ToListAsync();

            return _mapper.Map<IEnumerable<DisplayReviewViewModel>>(reviews);
        }
    }
}
