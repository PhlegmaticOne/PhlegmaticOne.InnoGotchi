using FluentValidation;
using Microsoft.AspNetCore.Authentication.Cookies;
using PhlegmaticOne.InnoGotchi.Web.ClientRequests;
using PhlegmaticOne.InnoGotchi.Web.Infrastructure.Extensions;
using PhlegmaticOne.InnoGotchi.Web.Infrastructure.MappersConfigurations;
using PhlegmaticOne.InnoGotchi.Web.Infrastructure.Validators;
using PhlegmaticOne.LocalStorage.Extensions;
using PhlegmaticOne.ServerRequesting.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

builder.Services
    .AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(x =>
    {
        x.LoginPath = new PathString("/Account/Login");
    });

builder.Services.AddAutoMapper(x =>
{
    x.AddProfile<AccountMapperConfiguration>();
    x.AddProfile<FarmMapperConfiguration>();
});

builder.Services.AddValidatorsFromAssemblyContaining<RegisterViewModelValidator>();


builder.Services.AddClientRequestsService("https://localhost:7142/api/", a =>
{
    a.ConfigureRequest<RegisterProfileRequest>("Profiles/Register");
    a.ConfigureRequest<LoginRequest>("Profiles/Login");
    a.ConfigureRequest<GetAllInnoGotchiComponentsRequest>("InnoGotchiComponents/GetAll");
    a.ConfigureRequest<GetFarmRequest>("Farm/Get");
    a.ConfigureRequest<CreateFarmRequest>("Farm/Create");
});

builder.Services.AddLocalStorage(startConf =>
{
    startConf.SetServerAddress("https://localhost:7142");
    startConf.SetLoginPath("/Account/Login");
    startConf.SetErrorPath("/Home/Error");
});


var app = builder.Build();


if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();


app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllerRoute(name: "default", pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
