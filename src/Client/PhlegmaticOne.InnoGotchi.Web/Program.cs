using FluentValidation;
using Microsoft.AspNetCore.Authentication.Cookies;
using PhlegmaticOne.ApiRequesting.Extensions;
using PhlegmaticOne.InnoGotchi.Web.Infrastructure.Extensions;
using PhlegmaticOne.InnoGotchi.Web.Infrastructure.MappersConfigurations;
using PhlegmaticOne.InnoGotchi.Web.Infrastructure.TagHelpers.PagedList.Helpers;
using PhlegmaticOne.InnoGotchi.Web.Infrastructure.Validators;
using PhlegmaticOne.InnoGotchi.Web.Infrastructure.ViewComponents.Base;
using PhlegmaticOne.InnoGotchi.Web.Requests.Collaborations;
using PhlegmaticOne.InnoGotchi.Web.Requests.Constructor;
using PhlegmaticOne.InnoGotchi.Web.Requests.Farms;
using PhlegmaticOne.InnoGotchi.Web.Requests.InnoGotchies;
using PhlegmaticOne.InnoGotchi.Web.Requests.Overviews;
using PhlegmaticOne.InnoGotchi.Web.Requests.Profile;
using PhlegmaticOne.InnoGotchi.Web.Requests.Profiles;
using PhlegmaticOne.LocalStorage.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

builder.Services
    .AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(x => { x.LoginPath = new PathString("/Auth/Login"); });

builder.Services.AddAutoMapper(x => { x.AddMaps(typeof(AccountMapperConfiguration).Assembly); });

builder.Services.AddValidatorsFromAssemblyContaining<RegisterViewModelValidator>();


builder.Services.AddClientRequestsService("https://localhost:7142/api/", a =>
{
    a.ConfigureRequest<RegisterProfileRequest>("Auth/Register");
    a.ConfigureRequest<LoginProfileRequest>("Auth/Login");

    a.ConfigureRequest<GetDetailedProfileRequest>("Profiles/GetAuthorized");
    a.ConfigureRequest<UpdateProfileRequest>("Profiles/Update");

    a.ConfigureRequest<SearchProfilesRequest>("Search/Profiles");

    a.ConfigureRequest<GetAvatarRequest>("Avatars/GetAuthorized");

    a.ConfigureRequest<GetDetailedFarmRequest>("Farms/GetAuthorized");
    a.ConfigureRequest<GetFarmRequest>("Farms/Get");
    a.ConfigureRequest<GetIsFarmExistsRequest>("Farms/Exists");
    a.ConfigureRequest<CreateFarmRequest>("Farms/Create");
    a.ConfigureRequest<GetCollaboratedFarmsRequest>("Farms/GetCollaborated");

    a.ConfigureRequest<GetPreviewFarmStatisticsRequest>("Statistics/GetAuthorized");
    a.ConfigureRequest<GetDetailedFarmStatisticsRequest>("Statistics/GetDetailed");
    a.ConfigureRequest<GetCollaboratedFarmStatisticsRequest>("Statistics/GetCollaborated");
    a.ConfigureRequest<GetGlobalStatisticsRequest>("Statistics/GetGlobal");

    a.ConfigureRequest<GetAllInnoGotchiComponentsRequest>("InnoGotchiComponents/GetAll");

    a.ConfigureRequest<CreateCollaborationRequest>("Collaborations/Create");

    a.ConfigureRequest<GetDetailedInnoGotchiRequest>("InnoGotchies/GetDetailed");
    a.ConfigureRequest<GetPreviewInnoGotchiRequest>("InnoGotchies/GetPreview");
    a.ConfigureRequest<CreateInnoGotchiRequest>("InnoGotchies/Create");
    a.ConfigureRequest<UpdateInnoGotchiRequest>("InnoGotchies/Update");
    a.ConfigureRequest<GetInnoGotchiesPagedListRequest>("InnoGotchies/GetPaged");
});

builder.Services.AddLocalStorage(startConf => { startConf.SetLoginPath("/Account/Login"); });

builder.Services.AddScoped<IPagedListPagesGenerator, PagedListPagesGenerator>();
builder.Services.AddScoped<IInnoGotchiStatusGenerator, InnoGotchiStatusGenerator>();


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

app.MapControllerRoute("default", "{controller=Home}/{action=Index}/{id?}");

app.Run();