using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViFlix.Core.ViewModels.MoviesViewModel;
using ViFlix.Core.ViewModels.SubscriptionViewModels;
using ViFlix.Core.ViewModels.UsersViewModels;
using ViFlix.Data.Movies;
using ViFlix.Data.Subscription;
using ViFlix.Data.Users;

namespace ViFlix.Core.Mapper
{
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            #region UserMapper
            CreateMap<SiteUsers, UserViewModel>();
            CreateMap<UserViewModel, SiteUsers>();

            CreateMap<SiteUsers, RegisterViewModel>();
            CreateMap<RegisterViewModel, SiteUsers>();

            CreateMap<SiteUsers, LoginViewModel>();
            CreateMap<LoginViewModel, SiteUsers>();

            CreateMap<SiteUsers, ProfileViewModel>();
            CreateMap<ProfileViewModel, SiteUsers>();
            CreateMap<UserViewModel, ProfileViewModel>();

            CreateMap<SiteUsers, ForgetPasswordViewModel>();
            CreateMap<ForgetPasswordViewModel, SiteUsers>();

            CreateMap<SiteUsers, ChangeEmailViewModel>();
            CreateMap<ChangeEmailViewModel, SiteUsers>();

            CreateMap<SiteUsers, ChangePasswordViewModel>();
            CreateMap<ChangePasswordViewModel, SiteUsers>();

            CreateMap<WishList, WishListViewModel>();
            CreateMap<WishListViewModel, WishList>();

            CreateMap<Roles, RoleViewModel>();
            CreateMap<RoleViewModel, Roles>();

            CreateMap<Permissions, PermissionViewModel>();
            CreateMap<PermissionViewModel, Permissions>();
            #endregion

            #region Subscription
            CreateMap<SubscriptionPlan, SuscriptionPlanViewModel>();
            CreateMap<SuscriptionPlanViewModel, SubscriptionPlan>();

            CreateMap<UserSubscription, UserSubscriptionViewModel>();
            CreateMap<UserSubscriptionViewModel, UserSubscription>();
            #endregion

            #region movies
            CreateMap<Ganres, GanresViewModel>();
            CreateMap<GanresViewModel, Ganres>();

            CreateMap<Actors, ActorsViewModel>();
            CreateMap<ActorsViewModel, Actors>();

            CreateMap<Director, DirectorViewModel>();
            CreateMap<DirectorViewModel, Director>();

            CreateMap<DownloadLink, DownloadLinksViewModel>();
            CreateMap<DownloadLinksViewModel, DownloadLink>();

            CreateMap<Language, LanguageViewModel>();
            CreateMap<LanguageViewModel, Language>();

            CreateMap<Movie, MovieViewModel>();
            CreateMap<MovieViewModel, Movie>();

            CreateMap<CreateReviewViewModel, Reviews>();

            CreateMap<Reviews, DisplayReviewViewModel>();
                //.ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.users.UserName?? ""))
                //.ForMember(dest => dest.Avatar, opt => opt.MapFrom(src => src.users.avatar ?? ""));
            CreateMap<DisplayReviewViewModel, Reviews>();

            CreateMap<Seasons, SeasonsViewModel>();
            CreateMap<SeasonsViewModel, Seasons>();

            CreateMap<Series, SeriesViewModel>();
            CreateMap<SeriesViewModel, Series>();
            #endregion
        }
    }
}
