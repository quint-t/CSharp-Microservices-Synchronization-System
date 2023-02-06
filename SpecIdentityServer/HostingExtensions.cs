using Duende.IdentityServer.AspNetIdentity;
using Duende.IdentityServer.Configuration;
using Duende.IdentityServer.EntityFramework.DbContexts;
using Duende.IdentityServer.EntityFramework.Mappers;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Serilog;
using SpecIdentityServer.Data;

namespace SpecIdentityServer;

internal static class HostingExtensions
{
    public static WebApplication ConfigureServices(this WebApplicationBuilder builder)
    {
        var migrationsAssembly = typeof(Program).Assembly.GetName().Name;

		builder.Services.AddDbContext<ApplicationDbContext>(options =>
            options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"))
        );

        builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
            .AddEntityFrameworkStores<ApplicationDbContext>();

        builder.Services.AddRazorPages();  // UI

        builder.Services.AddIdentityServer(options =>
            {
                options.Events.RaiseErrorEvents = true;
                options.Events.RaiseInformationEvents = true;
                options.Events.RaiseFailureEvents = true;
                options.Events.RaiseSuccessEvents = true;

                // see https://docs.duendesoftware.com/identityserver/v6/fundamentals/resources/
                options.EmitStaticAudienceClaim = true;
            })
            .AddConfigurationStore(options =>
            {
                options.ConfigureDbContext = b => b.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"),
                    sql => sql.MigrationsAssembly(migrationsAssembly));
            })
            .AddOperationalStore(options =>
            {
                options.ConfigureDbContext = b => b.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"),
                    sql => sql.MigrationsAssembly(migrationsAssembly));
            })
            .AddAspNetIdentity<IdentityUser>()
            .AddResourceOwnerValidator<ResourceOwnerPasswordValidator<IdentityUser>>();
		return builder.Build();
    }

    private static void InitializeDatabase(IApplicationBuilder app)
    {
		using (var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope())
        {
            serviceScope.ServiceProvider.GetRequiredService<PersistedGrantDbContext>().Database.Migrate();

            var context = serviceScope.ServiceProvider.GetRequiredService<ConfigurationDbContext>();
            context.Database.Migrate();
            if (!context.Clients.Any())
            {
                foreach (var client in Config.Clients)
                {
                    context.Clients.Add(client.ToEntity());
                }
                context.SaveChanges();
            }

            if (!context.IdentityResources.Any())
            {
                foreach (var resource in Config.IdentityResources)
                {
                    context.IdentityResources.Add(resource.ToEntity());
                }
                context.SaveChanges();
            }

            if (!context.ApiScopes.Any())
            {
                foreach (var resource in Config.ApiScopes)
                {
                    context.ApiScopes.Add(resource.ToEntity());
                }
                context.SaveChanges();
            }
        }
    }


    public static WebApplication ConfigurePipeline(this WebApplication app)
    {
        app.UseSerilogRequestLogging();


		if (app.Environment.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
            // app.UseMigrationsEndPoint();
        }
        else
        {
            app.UseExceptionHandler("/Error");
            app.UseHsts();
        }

        InitializeDatabase(app);

        app.UseHttpsRedirection();

        app.UseStaticFiles();  // UI
        app.UseRouting();  // UI

        app.UseIdentityServer();

        app.UseAuthorization();  // UI
        app.MapRazorPages().RequireAuthorization();  // UI

        return app;
    }
}
