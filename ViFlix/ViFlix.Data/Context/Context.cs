using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViFlix.Data.Movies;
using ViFlix.Data.RefreshTokens;
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

        public DbSet<RefreshToken> RefreshToken { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            foreach(var Relation in builder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys()))
            {
                Relation.DeleteBehavior = DeleteBehavior.NoAction;
            }
            builder.Entity<SiteUsers>().HasQueryFilter(e => !e.IsDelete);
            builder.Entity<Roles>().HasQueryFilter(e => !e.IsDelete);
            builder.Entity<Permissions>().HasQueryFilter(e => !e.IsDelete);
            builder.Entity<UserRoles>().HasQueryFilter(e => !e.IsDelete);
            builder.Entity<RolePermissions>().HasQueryFilter(e => !e.IsDelete);


            builder.Entity<SubscriptionPlan>().HasQueryFilter(e => !e.IsDelete);
            builder.Entity<UserSubscription>().HasQueryFilter(e => !e.IsDelete);


            builder.Entity<Ganres>().HasQueryFilter(e => !e.IsDelete);
            builder.Entity<Actors>().HasQueryFilter(e => !e.IsDelete);
            builder.Entity<Director>().HasQueryFilter(e => !e.IsDelete);
            builder.Entity<Language>().HasQueryFilter(e => !e.IsDelete);
            builder.Entity<Movie>().HasQueryFilter(e => !e.IsDelete);
            builder.Entity<DownloadLink>().HasQueryFilter(e => !e.IsDelete);
            builder.Entity<MovieGanres>().HasQueryFilter(e => !e.IsDelete);
            builder.Entity<Reviews>().HasQueryFilter(e => !e.IsDelete);
            builder.Entity<Seasons>().HasQueryFilter(e => !e.IsDelete);
            builder.Entity<Series>().HasQueryFilter(e => !e.IsDelete);
        }
    }
}
