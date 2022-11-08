using PhlegmaticOne.InnoGotchi.Web.ClientRequests;
using PhlegmaticOne.InnoGotchi.Web.MappersConfigurations;
using PhlegmaticOne.InnoGotchi.Web.Services.Storage;
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
builder.Services.AddLocalStorage("https://localhost:7142");


var app = builder.Build();


if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
