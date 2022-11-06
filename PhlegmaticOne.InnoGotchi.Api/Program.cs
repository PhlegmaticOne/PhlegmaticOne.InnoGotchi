using Microsoft.IdentityModel.Tokens;
using PhlegmaticOne.InnoGotchi.Api.Helpers;
using PhlegmaticOne.InnoGotchi.Data.Core.Services;
using PhlegmaticOne.InnoGotchi.Data.EntityFramework.Services;
using PhlegmaticOne.PasswordHasher.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers();
builder.Services.AddPasswordHasher();

builder.Services.AddSingleton<IUsersDataService, EfUsersDataService>();

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


var app = builder.Build();

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