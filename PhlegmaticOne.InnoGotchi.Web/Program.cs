using FluentValidation;
using Microsoft.AspNetCore.Authentication.Cookies;
using PhlegmaticOne.InnoGotchi.Web.ClientRequests;
using PhlegmaticOne.InnoGotchi.Web.Infrastructure.Extensions;
using PhlegmaticOne.InnoGotchi.Web.Infrastructure.MappersConfigurations;
using PhlegmaticOne.InnoGotchi.Web.Infrastructure.TagHelpers.PagedList.Helpers;
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
    x.AddMaps(typeof(AccountMapperConfiguration).Assembly);
});

builder.Services.AddValidatorsFromAssemblyContaining<RegisterViewModelValidator>();


builder.Services.AddClientRequestsService("https://localhost:7142/api/", a =>
{
    a.ConfigureRequest<RegisterProfileRequest>("AnonymousProfiles/Register");
    a.ConfigureRequest<LoginRequest>("AnonymousProfiles/Login");

    a.ConfigureRequest<DetailedProfileGetRequest>("Profiles/GetDetailed");
    a.ConfigureRequest<UpdateAccountRequest>("Profiles/Update");
    a.ConfigureRequest<SearchProfilesRequest>("Profiles/Search");

    a.ConfigureRequest<GetAvatarGetRequest>("Avatars/Get");

    a.ConfigureRequest<GetFarmRequest>("Farms/GetForAuthorized");
    a.ConfigureRequest<GetProfileFarmGetRequest>("Farms/Get");
    a.ConfigureRequest<CreateFarmRequest>("Farms/Create");
    a.ConfigureRequest<GetCollaboratorsFarmPreviewsGetRequest>("Farms/GetCollaboratedFarms");

    a.ConfigureRequest<GetPreviewFarmStatisticsGetRequest>("FarmStatistics/Get");
    a.ConfigureRequest<GetDetailedFarmStatisticsGetRequest>("FarmStatistics/GetDetailed");
    a.ConfigureRequest<GetCollaboratorsFarmStatisticsGetRequest>("FarmStatistics/GetForAllCollaboratedFarms");

    a.ConfigureRequest<GetAllInnoGotchiComponentsRequest>("InnoGotchiComponents/GetAll");

    a.ConfigureRequest<CreateCollaborationRequest>("Collaborations/Create");

    a.ConfigureRequest<GetInnoGotchiGetRequest>("InnoGotchies/Get");
    a.ConfigureRequest<CreateInnoGotchiRequest>("InnoGotchies/Create");
    a.ConfigureRequest<DrinkInnoGotchiRequest>("InnoGotchies/Drink");
    a.ConfigureRequest<FeedInnoGotchiRequest>("InnoGotchies/Feed");
    a.ConfigureRequest<GetPagedListRequest>("InnoGotchies/GetPaged");
});

builder.Services.AddLocalStorage(startConf =>
{
    startConf.SetServerAddress("https://localhost:7142");
    startConf.SetLoginPath("/Account/Login");
});

builder.Services.AddTransient<IPagedListPagesGenerator, PagedListPagesGenerator>();


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
