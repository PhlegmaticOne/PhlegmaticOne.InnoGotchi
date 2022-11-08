using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.IdentityModel.Tokens;
using PhlegmaticOne.InnoGotchi.Api.Helpers;
using PhlegmaticOne.InnoGotchi.Api.MapperConfigurations;
using PhlegmaticOne.InnoGotchi.Api.Services;
using PhlegmaticOne.InnoGotchi.Data.Core.Services;
using PhlegmaticOne.InnoGotchi.Data.EntityFramework.Context;
using PhlegmaticOne.InnoGotchi.Data.EntityFramework.Services;
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
})
.AddJwtBearer(o =>
{
    o.SaveToken = true;
    o.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = false,
        ValidateAudience = false,
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
    x.AddProfile<ProfileMapperConfiguration>();
    x.AddProfile<InnoGotchiComponentsMapperConfiguration>();
});

builder.Services.AddDbContext<ApplicationDbContext>(x =>
{
    //var connectionString = builder.Configuration.GetConnectionString("DbConnection");
    //x.UseSqlServer(connectionString);

    x.UseInMemoryDatabase("MEMORY");
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers();


builder.Services.AddPasswordHasher();
builder.Services.AddJwtTokenGeneration(jwtOptions);
builder.Services.AddScoped<IUserProfilesDataService, EfProfilesDataService>();
builder.Services.AddScoped<IUsersDataService, EfUsersDataService>();
builder.Services.AddScoped<IInnoGotchiComponentsDataService, EfInnoGotchiComponentsDataService>();


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