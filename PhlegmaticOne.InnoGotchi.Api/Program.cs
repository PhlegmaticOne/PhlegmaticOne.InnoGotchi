using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using PhlegmaticOne.InnoGotchi.Api.Helpers;
using PhlegmaticOne.InnoGotchi.Api.MapperConfigurations;
using PhlegmaticOne.InnoGotchi.Api.Services;
using PhlegmaticOne.InnoGotchi.Data.Core.Services;
using PhlegmaticOne.InnoGotchi.Data.EntityFramework.Context;
using PhlegmaticOne.InnoGotchi.Data.EntityFramework.Services;
using PhlegmaticOne.PasswordHasher.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAuthentication("OAuth")
    .AddJwtBearer("OAuth", config =>
    {
        config.TokenValidationParameters = new TokenValidationParameters
        {
            ValidIssuer = JwtAuthenticationHelper.Issuer,
            ValidAudience = JwtAuthenticationHelper.Audience,
            IssuerSigningKey = new SymmetricSecurityKey(JwtAuthenticationHelper.GetSecretBytes()),
        };
    });

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers();
builder.Services.AddPasswordHasher();

builder.Services.AddJwtTokenGeneration(TimeSpan.FromMinutes(1));

builder.Services.AddAutoMapper(x =>
{
    x.AddProfile<ProfileMapperConfiguration>();
});

builder.Services.AddDbContext<ApplicationDbContext>(x =>
{
    //var connectionString = builder.Configuration.GetConnectionString("DbConnection");
    //x.UseSqlServer(connectionString);

    x.UseInMemoryDatabase("MEMORY");
});
builder.Services.AddScoped<IUserProfilesDataService, EfProfilesDataService>();
builder.Services.AddScoped<IUsersDataService, EfUsersDataService>();


var app = builder.Build();

//await DatabaseInitializer.SeedAsync(app.Services);

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