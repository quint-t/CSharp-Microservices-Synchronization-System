using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using System.IdentityModel.Tokens.Jwt;
using SpecProfiles.Data;
using Microsoft.EntityFrameworkCore;
using SpecProfiles.Services;
using Microsoft.Extensions.DependencyInjection;
using Settings;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();

JwtSecurityTokenHandler.DefaultMapInboundClaims = false;

builder.Services.AddDbContext<ApplicationDbContext>(options =>
	options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"))
);

builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = "Cookies";
    options.DefaultChallengeScheme = "oidc";
})
    .AddCookie("Cookies")
    .AddOpenIdConnect("oidc", options =>
    {
        options.Authority = "https://localhost:5001";

        options.ClientId = "web";
        options.ClientSecret = "secret";
        options.ResponseType = OpenIdConnectResponseType.Code;

        options.Scope.Clear();
        options.Scope.Add("openid");
        options.Scope.Add("profile");
        options.Scope.Add("verification");
        options.GetClaimsFromUserInfoEndpoint = true;

        options.SaveTokens = true;

        options.Events = new OpenIdConnectEvents
        {
            OnRemoteFailure = (context) =>
            {
                context.Response.Redirect("/Error");
                context.HandleResponse();
                return Task.CompletedTask;
            }
        };
    });

builder.Services.AddSingleton<IConfiguration>(builder.Configuration);
builder.Services.AddSingleton<EventProvider>();
builder.Services.AddHostedService<GrpcSubscriberService>();
builder.Services.AddHostedService<LoginService>();

builder.Services.AddOptions();
builder.Services.Configure<AppSettings>(builder.Configuration.GetSection("AppSettings"));

var app = builder.Build();

AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapRazorPages().RequireAuthorization();

app.Run();
