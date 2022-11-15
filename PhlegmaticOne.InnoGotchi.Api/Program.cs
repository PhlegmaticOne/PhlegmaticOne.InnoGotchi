using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using PhlegmaticOne.DataService.Extensions;
using PhlegmaticOne.InnoGotchi.Api.Infrastructure.Helpers;
using PhlegmaticOne.InnoGotchi.Api.Infrastructure.MapperConfigurations;
using PhlegmaticOne.InnoGotchi.Api.Infrastructure.MapperConverters;
using PhlegmaticOne.InnoGotchi.Api.Infrastructure.MapperResolvers;
using PhlegmaticOne.InnoGotchi.Api.Infrastructure.Validators;
using PhlegmaticOne.InnoGotchi.Api.Models;
using PhlegmaticOne.InnoGotchi.Api.Services.InnoGotchiPolicies;
using PhlegmaticOne.InnoGotchi.Api.Services.Other;
using PhlegmaticOne.InnoGotchi.Api.Services.Time;
using PhlegmaticOne.InnoGotchi.Api.Services.Verifying;
using PhlegmaticOne.InnoGotchi.Api.Services.Verifying.Base;
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
    if (builder.Environment.IsDevelopment())
    {
        x.UseInMemoryDatabase("MEMORY");
    }
    else
    {
        var connectionString = builder.Configuration.GetConnectionString("DbConnection");
        x.UseSqlServer(connectionString);
    }
});
builder.Services.AddValidatorsFromAssemblyContaining<UserProfileValidator>();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers();


builder.Services.AddPasswordHasher();
builder.Services.AddJwtTokenGeneration(jwtOptions);
builder.Services.AddDataService<ApplicationDbContext>();


builder.Services.AddTransient<ITimeProvider, TimeProvider>();
builder.Services.AddTransient<IInnoGotchiActionsPolicy>(_ =>
{
    return new InnoGotchiActionsPolicy(TimeSpan.FromDays(1), TimeSpan.FromDays(1), TimeSpan.FromDays(1));
});

builder.Services.AddTransient<ProfileDtoJwtTokenPropertyResolver>();
builder.Services.AddTransient<ProfileAvatarPropertyResolver>();
builder.Services.AddTransient<ComponentsRemoveSiteAddressMapperResolver>();
builder.Services.AddSingleton<ImageUrlConverter>();

builder.Services.AddTransient<IAvatarConvertingService, AvatarConvertingService>();
builder.Services.AddScoped<IVerifyingService<IdentityFarmModel, Farm>, FarmVerifyingService>();
builder.Services.AddScoped<IVerifyingService<IdentityInnoGotchiModel, InnoGotchiModel>, InnoGotchiVerifyingService>();
builder.Services.AddScoped<IVerifyingService<RegisterProfileDto, UserProfile>, UserProfileVerifyingService>();
builder.Services.AddScoped<IVerifyingService<UpdateProfileDto, UserProfile>, UpdateProfileVerifyingService>();
builder.Services.AddSingleton<IServerAddressProvider>(_ => new ServerAddressProvider("https://localhost:7142"));

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