using PhlegmaticOne.InnoGotchi.Web.ClientRequests;
using PhlegmaticOne.InnoGotchi.Web.MappersConfigurations;
using PhlegmaticOne.InnoGotchi.Web.Middlewares;
using PhlegmaticOne.LocalStorage.Extensions;
using PhlegmaticOne.ServerRequesting.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();
builder.Services.AddAutoMapper(x =>
{
    x.AddProfile<AccountMapperConfiguration>();
});


builder.Services.AddClientRequestsService("https://localhost:7142/api/", a =>
{
    a.ConfigureRequest<RegisterProfileRequest>("Profiles/Register");
    a.ConfigureRequest<LoginRequest>("Profiles/Login");
    a.ConfigureRequest<GetAllInnoGotchiComponentsRequest>("InnoGotchiComponents/GetAll");
});


builder.Services.AddLocalStorage(startConf =>
{
    startConf.SetServerAddress("https://localhost:7142");
    startConf.SetAnonymousEndpoints("/Account/Login", "/Account/Register", "/");
    startConf.SetLoginUrl("/Account/Login");
    startConf.SetIsAuthenticationRequired(true);
});


var app = builder.Build();


if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseCustomAuthentication();

app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(name: "default", pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
