using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViFlix.Data.Movies;
using ViFlix.Data.Subscription;
using ViFlix.Data.Users;

namespace ViFlix.Data.Context
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<SiteUsers> Users { get; set; }
        public DbSet<Roles> Roles { get; set; }
        public DbSet<Permissions> Permissions { get; set; }
        public DbSet<RolePermissions> RolePermissions { get; set; }
        public DbSet<UserRoles> UserRoles { get; set; }

        public DbSet<SubscriptionPlan> SubscriptionPlan { get; set; }
        public DbSet<UserSubscription> UserSubscription { get; set; }


        public DbSet<Ganres> Ganres { get; set; }
        public DbSet<Actors> Actors { get; set; }
        public DbSet<Director> Directors { get; set; }
        public DbSet<Language> Language { get; set; }
        public DbSet<Movie> Movie { get; set; }
        public DbSet<DownloadLink> DownloadLinks { get; set; }
        public DbSet<MovieGanres> MovieGanres { get; set; }
        public DbSet<Reviews> Reviews { get; set; }
        public DbSet<Seasons> Seasons { get; set; }
        public DbSet<Series> Series { get; set; }
    }
}
