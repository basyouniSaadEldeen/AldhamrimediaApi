using AldhamrimediaApi.DbContext;
using AldhamrimediaApi.Dtos.UserDto;
using AldhamrimediaApi.Models;
using AldhamrimediaApi.Service.interfaces;
using AldhamrimediaApi.Service.Repositories;

using AldhamrimediaApi.Settings;
using Bookify.Web.Settings;
using Ecommerce;
using FluentValidation.AspNetCore;
using HealthCare.Services.Repositories;
using JsonBasedLocalization.Api;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
//using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using Microsoft.IdentityModel.Tokens;
using System.Globalization;
using System.Reflection;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers()
         .AddFluentValidation(s =>
                s.RegisterValidatorsFromAssembly(Assembly.GetExecutingAssembly()));

builder.Services.AddLocalization();
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSingleton<IStringLocalizerFactory, JsonStringLocalizerFactory>();

builder.Services.AddMvc()
    .AddDataAnnotationsLocalization(options =>
    {
        options.DataAnnotationLocalizerProvider = (type, factory) =>
            factory.Create(typeof(JsonStringLocalizerFactory));
    });

builder.Services.Configure<RequestLocalizationOptions>(options =>
{
    var supportedCultures = new[]
    {
        new CultureInfo("en-US"),
        new CultureInfo("ar-EG"),
        new CultureInfo("de-DE")
    };

    options.DefaultRequestCulture = new RequestCulture(culture: supportedCultures[0]);
    options.SupportedCultures = supportedCultures;
});
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.AddDbContext<ApplicationDbContext>(
           options =>
           {
               options.UseLazyLoadingProxies().UseSqlServer(builder.Configuration.GetConnectionString("MediaDB")
                  );

           });

builder.Services.Configure<CloudinarySettings>(builder.Configuration.GetSection(nameof(CloudinarySettings)));

builder.Services.AddScoped<IAccountRepository, AccountRepository>();
builder.Services.AddScoped<IServicesRepository, ServicesRepository>();
//builder.Services.Configure<MailSettings>(builder.Configuration.GetSection(nameof(MailSettings)));

builder.Services.Configure<ApiBehaviorOptions>(options
              => options.SuppressModelStateInvalidFilter = true);

builder.Services.AddScoped<ValidationFilterAttribute>();


builder.Services
.AddIdentity<User, Role>(options =>
{
    options.Password.RequireDigit = false;

    options.Password.RequireUppercase = false;

    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireLowercase = false;

    options.User.RequireUniqueEmail = true;
    options.SignIn.RequireConfirmedAccount = false;

}).AddEntityFrameworkStores<ApplicationDbContext>().AddDefaultTokenProviders();
builder.Services.Configure<Jwt>(builder.Configuration.GetSection("Jwt"));

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    //options.DefaultChallengeScheme = GoogleDefaults.AuthenticationScheme;

    //options.DefaultAuthenticateScheme = GoogleDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    var key = Encoding.ASCII.GetBytes(builder.Configuration.GetSection("jwt:Key").Value);
    options.RequireHttpsMetadata = false;
    options.SaveToken = false;


    options.TokenValidationParameters = new TokenValidationParameters()

    {

        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(key),
        ValidateIssuer = false,
        ValidateAudience = false,
        RequireExpirationTime = false,
        ValidateLifetime = true

    };
});
 //.AddGoogle(options =>
 //{


 //    IConfigurationSection googleAuthSection = builder.Configuration.GetSection("Authentication:Google");

 //    options.ClientId = googleAuthSection["ClientId"];
 //    options.ClientSecret = googleAuthSection["ClientSecret"];
 //});


//services cors
builder.Services.AddCors(p => p.AddPolicy("corspolicy", build =>
{
    build.WithOrigins("*").AllowAnyMethod().AllowAnyHeader();
}));


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseCors("corspolicy");
app.UseHttpsRedirection();

var supportedCultures = new[] { "en-US", "ar-EG", "de-DE" };
var localizationOptions = new RequestLocalizationOptions()
    .SetDefaultCulture(supportedCultures[0])
    .AddSupportedCultures(supportedCultures);

app.UseRequestLocalization(localizationOptions);
app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
