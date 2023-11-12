using Autofac.Extensions.DependencyInjection;
using ChatApp.Persistence.Database;
using Microsoft.EntityFrameworkCore;
using ChatApp.Persistence.Extentions;
using Serilog;
using Serilog.Events;
using System.Reflection;
using Autofac;
using ChatApp.Persistence;
using Microsoft.AspNetCore.Authentication.Cookies;
using ChatApp.Infrastructure.Feature.Services.Email.Settings;
using ChatApp.Infrastructure;

var builder = WebApplication.CreateBuilder(args);
builder.Host.UseSerilog((ctx, lc) => lc
    .MinimumLevel.Debug()
    .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
    .Enrich.FromLogContext()
    .ReadFrom.Configuration(builder.Configuration));

// Add services to the container.
try
{
    var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
    var migrationAssembly = Assembly.GetExecutingAssembly().FullName;

   

    builder.Services.AddDbContext<ApplicationDbContext>(options =>
        options.UseSqlServer(connectionString, x => x.MigrationsAssembly(migrationAssembly)));

    builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());
    builder.Host.ConfigureContainer<ContainerBuilder>(containerBuilder =>
    {
        containerBuilder.RegisterModule(new PersistenceModule(connectionString,
            migrationAssembly));
        containerBuilder.RegisterModule(new WebModule());
        containerBuilder.RegisterModule(new InfrastructureModule());
    });
    builder.Services.AddDatabaseDeveloperPageExceptionFilter();

    builder.Services.AddIdentityUser();

    builder.Services.AddRazorPages();

    builder.Services.AddControllersWithViews();

    builder.Services.AddAuthentication()
       .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, options =>
       {
           options.LoginPath = new PathString("/auth/login");
           options.AccessDeniedPath = new PathString("/auth/login");
           options.LogoutPath = new PathString("/auth/logout");
           options.Cookie.Name = "ChatApp.Identity";
           options.SlidingExpiration = true;
           options.ExpireTimeSpan = TimeSpan.FromDays(7);
       });

    builder.Services.Configure<EmailSettings>(builder.Configuration.GetSection("EmailServiceSettings"));

    var app = builder.Build();

    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        app.UseMigrationsEndPoint();
    }
    else
    {
        app.UseExceptionHandler("/Home/Error");
        // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
        app.UseHsts();
    }

    app.UseHttpsRedirection();
    app.UseStaticFiles();

    app.UseRouting();
    app.UseAuthentication();
    app.UseAuthorization();

    app.MapControllerRoute(
        name: "default",
        pattern: "{controller=home}/{action=index}/{id?}");

    app.MapRazorPages();

    app.Run();
    Log.Information("Starting web host");

}
catch(Exception ex)
{
    Log.Fatal(ex, "Host terminated unexpectedly");
}
finally
{
    Log.CloseAndFlush();
}