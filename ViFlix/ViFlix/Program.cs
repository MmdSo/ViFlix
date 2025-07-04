﻿using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Net;
using System.Text;
using ViFlix.Core.Services.Movies.ActorServices;
using ViFlix.Core.Services.Movies.DirectorServices;
using ViFlix.Core.Services.Movies.DownloadLinks;
using ViFlix.Core.Services.Movies.DownloadLinksServices;
using ViFlix.Core.Services.Movies.Ganereservices;
using ViFlix.Core.Services.Movies.LanguageServices;
using ViFlix.Core.Services.Movies.MovieServices;
using ViFlix.Core.Services.Movies.ReviewServices;
using ViFlix.Core.Services.Movies.SeassonServices;
using ViFlix.Core.Services.Movies.SerieServices;
using ViFlix.Core.Services.RefreshTokens;
using ViFlix.Core.Services.Subscription.SubscriptionPlanService;
using ViFlix.Core.Services.Subscription.UserSubscriptionService;
using ViFlix.Core.Services.User.PermissionsServices;
using ViFlix.Core.Services.User.RolesServices;
using ViFlix.Core.Services.User.UserServices;
using ViFlix.Data.Repository;


var builder = WebApplication.CreateBuilder(args);

//builder.WebHost.ConfigureKestrel(serverOptions =>
//{
//    // آدرس IP محلی رو دستی وارد کن، اینجوری همه چی شفافه
//    serverOptions.Listen(IPAddress.Parse("192.168.1.1"), 5000); // ← آدرس آی‌پی لپ‌تاپ اولت
//});



// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddControllers().AddXmlSerializerFormatters();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllersWithViews();
builder.Services.AddSession();
builder.Services.AddMvc();
builder.Services.AddRazorPages();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(option =>
{
    var jwtScurityScheme = new OpenApiSecurityScheme
    {
        BearerFormat = "JWT",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = JwtBearerDefaults.AuthenticationScheme,
        Description = "Enter your token :",
        Reference = new OpenApiReference
        {
            Id = JwtBearerDefaults.AuthenticationScheme,
            Type = ReferenceType.SecurityScheme
        }
    };
    option.AddSecurityDefinition("Bearer", jwtScurityScheme);
    option.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {jwtScurityScheme ,Array.Empty<string>() }
    });
});


builder.Services.AddDbContext<ViFlix.Data.Context.AppDbContext>(options =>
options.UseSqlServer(builder.Configuration.GetConnectionString("ViFlixConectionString"))
.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking)
, ServiceLifetime.Transient);


#region Authorization
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, config =>
{
    config.Cookie.Name = "Viflix.CookieAuth";
    config.LoginPath = "/Login";
    config.LogoutPath = "/logout";
    config.SlidingExpiration = true;
    config.Cookie.MaxAge = TimeSpan.FromMinutes(60);
    config.ExpireTimeSpan = TimeSpan.FromMinutes(60);
    config.Cookie.HttpOnly = false;
    config.Cookie.SameSite = Microsoft.AspNetCore.Http.SameSiteMode.Strict;
    config.Cookie.SecurePolicy = Microsoft.AspNetCore.Http.CookieSecurePolicy.Always;
    config.Cookie.IsEssential = true;
})
.AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
{
    options.RequireHttpsMetadata = false;
    options.SaveToken = true;
    var config = builder.Configuration.GetSection("JwtSettings");
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = config["Issuer"],
        ValidAudience = config["Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["Key"]))
    };
});
#endregion

#region GetToken
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "FirstShop API", Version = "v1" });

    var jwtSecurityScheme = new OpenApiSecurityScheme
    {
        Scheme = "bearer",
        BearerFormat = "JWT",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Description = "Add your token here : Bearer {your token}",

        Reference = new OpenApiReference
        {
            Id = JwtBearerDefaults.AuthenticationScheme,
            Type = ReferenceType.SecurityScheme
        }
    };

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        { jwtSecurityScheme, Array.Empty<string>() }
    });
});
#endregion


builder.Services.AddSingleton(typeof(IGenericRepository<>), typeof(GenericRepository<>));
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

builder.Services.AddTransient<IUserService, UserService>();
builder.Services.AddTransient<IRoleServices, RoleServices>();
builder.Services.AddTransient<IPermissionServices, PermissionServices>();

builder.Services.AddTransient<IGanreServices, GaneresServices>();
builder.Services.AddTransient<ILanguagesServices, LanguagesServices>();
builder.Services.AddTransient<IMoviesServices , MoviesServices>();
builder.Services.AddTransient<IDownloadLinksServices, DownloadLinkServices>();
builder.Services.AddTransient<IReviewsServices , ReviwsServices>();
builder.Services.AddTransient<ISeasonServices , SeasonsServices>();
builder.Services.AddTransient<ISeriesServices , SeriesServices>();
builder.Services.AddTransient<IActorsServices , ActorsServices>();
builder.Services.AddTransient<IDirectorsServices , DirectorsServices>();

builder.Services.AddTransient<IsubscriptionPlanServices , SubscriptionPlanServices>();
builder.Services.AddTransient<IUserSubscriptionServices , UserSubscriptionServices>();

builder.Services.AddScoped<IRefreshTokenServices, RefreshTokenServices>();

//builder.Services.AddCors(options =>
//{
//    options.AddPolicy("AllowAll", policy =>
//    {
//        policy.AllowAnyOrigin()
//              .AllowAnyMethod()
//              .AllowAnyHeader();
//    });
//});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//app.UseCors("AllowAll");

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
