using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using PhlegmaticOne.DataService.Extensions;
using PhlegmaticOne.InnoGotchi.Api.Infrastructure.Helpers;
using PhlegmaticOne.InnoGotchi.Api.Infrastructure.MapperConfigurations;
using PhlegmaticOne.InnoGotchi.Api.Infrastructure.MapperResolvers;
using PhlegmaticOne.InnoGotchi.Api.Infrastructure.Validators;
using PhlegmaticOne.InnoGotchi.Api.Models;
using PhlegmaticOne.InnoGotchi.Api.Services;
using PhlegmaticOne.InnoGotchi.Api.Services.Mapping;
using PhlegmaticOne.InnoGotchi.Api.Services.Mapping.Base;
using PhlegmaticOne.InnoGotchi.Data.EntityFramework.Context;
using PhlegmaticOne.InnoGotchi.Data.Models;
using PhlegmaticOne.InnoGotchi.Shared.Users;
using PhlegmaticOne.JwtTokensGeneration.Extensions;
using PhlegmaticOne.JwtTokensGeneration.Options;
using PhlegmaticOne.PasswordHasher.Extensions;

var builder = WebApplication.CreateBuilder(args);

var configuration = builder.Configuration;
var jwtSecrets = configuration.GetSection("JwtSecrets");
var jwtOptions = new SymmetricKeyJwtOptions(jwtSecrets["Issuer"],
    jwtSecrets["Audience"],
    int.Parse(jwtSecrets["ExpirationDurationInMinutes"]),
    jwtSecrets["SecretKey"]);

builder.Services.AddAuthentication(x =>
{
    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(o =>
{
    o.SaveToken = true;
    o.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtOptions.Issuer,
        ValidAudience = jwtOptions.Audience,
        IssuerSigningKey = jwtOptions.GetSecretKey(),
        ClockSkew = TimeSpan.Zero
    };
});
builder.Services.AddAutoMapper(x =>
{
    x.AddMaps(typeof(ProfileMapperConfiguration).Assembly);
});
builder.Services.AddDbContext<ApplicationDbContext>(x =>
{
    //var connectionString = builder.Configuration.GetConnectionString("DbConnection");
    //x.UseSqlServer(connectionString);

    x.UseInMemoryDatabase("MEMORY");
});
builder.Services.AddValidatorsFromAssemblyContaining<UserProfileValidator>();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers();


builder.Services.AddPasswordHasher();
builder.Services.AddJwtTokenGeneration(jwtOptions);
builder.Services.AddDataService<ApplicationDbContext>();


builder.Services.AddTransient<ProfileDtoJwtTokenPropertyResolver>();
builder.Services.AddTransient<ProfileAvatarPropertyResolver>();
builder.Services.AddTransient<IAvatarConvertingService, AvatarConvertingService>();
builder.Services.AddScoped<IVerifyingService<ProfileFarmModel, Farm>, FarmVerifyingService>();
builder.Services.AddScoped<IVerifyingService<ProfileInnoGotchiModel, InnoGotchiModel>, InnoGotchiVerifyingService>();
builder.Services.AddScoped<IVerifyingService<RegisterProfileDto, UserProfile>, UserProfileVerifyingService>();
builder.Services.AddScoped<IVerifyingService<UpdateProfileDto, UserProfile>, UpdateProfileVerifyingService>();

var app = builder.Build();

await DatabaseInitializer.SeedAsync(app.Services);

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();