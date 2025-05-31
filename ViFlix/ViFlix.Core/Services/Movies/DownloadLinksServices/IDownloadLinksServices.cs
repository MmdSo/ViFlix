using ViFlix.Core.ViewModels.MoviesViewModel;
using ViFlix.Data.Movies;
using ViFlix.Data.Repository;

namespace ViFlix.Core.Services.Movies.DownloadLinks
{
    public interface IDownloadLinksServices : IGenericRepository<DownloadLink>
    {
        IEnumerable<DownloadLinksViewModel> GetAllDownloadLinks();
        DownloadLinksViewModel GetLinksById(long? Id);
        Task<long> AddDownloadLinkAsync(DownloadLinksViewModel link);
        Task EditDownloadLinkAsync(DownloadLinksViewModel link);
        Task DeleteDownloadLinkAsync(long id);
        List<DownloadLinksViewModel> GetLinksByMovieId(long movieId);
    }
}
