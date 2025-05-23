using ViFlix.Core.ViewModels.MoviesViewModel;
using ViFlix.Data.Movies;
using ViFlix.Data.Repository;

namespace ViFlix.Core.Services.Movies.LanguageServices
{
    public interface ILanguagesServices : IGenericRepository<Language>
    {
        IEnumerable<LanguageViewModel> GetAllLanguages();
        LanguageViewModel GetLanguageById(long? Id);
        Task<long> AddLanguage(LanguageViewModel language);
        Task EditLanguages(LanguageViewModel language);
        Task DeleteLanguage(long Id);
    }
}
