using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
        public ReviwsServices(AppDbContext context, IMapper mapper) : base(context)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<long> AddReview(ReviewViewModel review)
        {
            var rw = _mapper.Map<ReviewViewModel, Reviews>(review);
            await AddEntity(rw);
            await SaveChanges();
            return rw.Id;
        }

        public bool ApprovedReview(long? id, bool isApproved)
        {
            var rw = GetReviwById(id);
            rw.IsApproved = isApproved;
            EditReview(rw);
            return rw.IsApproved;
        }

        public void DeleteReview(ReviewViewModel review)
        {
            throw new NotImplementedException();
        }

        public async Task EditReview(ReviewViewModel review)
        {
            review.IsDelete = true;
            EditReview(review);
        }

        public List<ReviewViewModel> GetAllMovieReviewByMovieId(long? id)
        {
            var rw = GetAllReviews().Where(p => p.MovieId == id && p.IsApproved == true).ToList();
            return rw;
        }

        public IEnumerable<ReviewViewModel> GetAllReviews()
        {
            var rw = _mapper.Map<IEnumerable<Reviews>, IEnumerable<ReviewViewModel>>(GetAll().OrderBy(p => p.DateCreated));
            return rw;
        }

        public ReviewViewModel GetReviewByMovieId(long? id)
        {
            var pd = _mapper.Map<Reviews,ReviewViewModel>(GetEntityById(id));
            return pd;
        }

        public ReviewViewModel GetReviwById(long? id)
        {
            var pd = _mapper.Map<Reviews, ReviewViewModel>(GetEntityById(id));
            return pd;
        }

        public async Task SendReview(ReviewViewModel review)
        {
            await AddReview(review);
        }
    }
}
