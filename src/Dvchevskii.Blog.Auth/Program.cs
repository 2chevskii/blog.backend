using Dvchevskii.Blog.Auth.Services;
using Dvchevskii.Blog.Infrastructure;
using Dvchevskii.Blog.Shared.Assets.Images;
using Dvchevskii.Blog.Shared.Authentication;
using Dvchevskii.Blog.Shared.Authentication.Context;
using Dvchevskii.Blog.Shared.Authentication.Passwords;
using Dvchevskii.Blog.Shared.Setup;
using Dvchevskii.Blog.Shared.WebApi;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers().AddInternalControllers();
builder.Services.ConfigureLowercaseRoutes()
    .ConfigureJsonHandling();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddAuthenticationContext()
    .AddAuthenticationContextSetterMiddleware()
    .AddBlogAuthenticationScheme();

builder.Services.AddSetupHandlers().AddSetupRunner();

builder.Services.AddBlogDbContext(
    builder.Configuration.GetConnectionString("MySql") ??
    throw new Exception("MySql connection string not found")
);

builder.Services.AddPasswordHasher();

builder.Services.AddSharedDataProtection();

builder.Services.AddScoped<LocalAuthService>();
builder.Services.AddScoped<UserProfileService>();

builder.Services.AddImageAssetServices();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseAuthentication();
app.UseAuthenticationContextSetter();
app.UseAuthorization();

app.MapControllers();

app.Run();
