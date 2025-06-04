using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViFlix.Core.Services.Movies.DownloadLinks;
using ViFlix.Core.ViewModels.MoviesViewModel;
using ViFlix.Data.Context;
using ViFlix.Data.Movies;
using ViFlix.Data.Repository;

namespace ViFlix.Core.Services.Movies.DownloadLinksServices
{
    public class DownloadLinkServices : GenericRepository<DownloadLink>, IDownloadLinksServices
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        public DownloadLinkServices(AppDbContext context , IMapper mapper) : base(context)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task DeleteDownloadLinkAsync(long id)
        {
            DownloadLinksViewModel link = GetLinksById(id);

            var li = _mapper.Map<DownloadLinksViewModel, DownloadLink>(link);

            li.IsDelete = true;

            EditEntity(li);
            await SaveChanges();
        }

        public async Task EditDownloadLinkAsync(DownloadLinksViewModel link)
        {
            var li = _mapper.Map<DownloadLinksViewModel, DownloadLink>(link);
            EditEntity(li);
            await SaveChanges();
        }

        public IEnumerable<DownloadLinksViewModel> GetAllDownloadLinks()
        {
            var link = _mapper.Map<IEnumerable<DownloadLink>, IEnumerable<DownloadLinksViewModel>>(GetAll());
            return link;
        }

        public DownloadLinksViewModel GetLinksById(long? Id)
        {
            var link = _mapper.Map<DownloadLink, DownloadLinksViewModel>(GetEntityById(Id));
            return link;
        }

        public List<DownloadLinksViewModel> GetLinksByMovieId(long movieId)
        {
            var link = _mapper.Map<IEnumerable<DownloadLink>, IEnumerable<DownloadLinksViewModel>>(GetAll().Where(p => p.MovieId == movieId)).ToList();
            return link;
        }
    }
}
