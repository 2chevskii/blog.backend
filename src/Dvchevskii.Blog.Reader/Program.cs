using Dvchevskii.Blog.Infrastructure;
using Dvchevskii.Blog.Reader.Services;
using Dvchevskii.Blog.Shared.Assets.Images;
using Dvchevskii.Blog.Shared.Authentication;
using Dvchevskii.Blog.Shared.Authentication.Context;
using Dvchevskii.Blog.Shared.Authentication.Passwords;
using Dvchevskii.Blog.Shared.Setup;
using Dvchevskii.Blog.Shared.WebApi;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers()
    .AddInternalControllers();

builder.Services.ConfigureLowercaseRoutes()
    .ConfigureJsonHandling();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddBlogDbContext(
    builder.Configuration.GetConnectionString("MySql") ??
    throw new Exception("mysql connection string is missing")
);

builder.Services.AddBlogAuthenticationScheme()
    .AddAuthenticationContext()
    .AddAuthenticationContextSetterMiddleware()
    .AddSharedDataProtection();

builder.Services.AddSetupHandlers()
    .AddSetupRunner()
    .AddPasswordHasher();

builder.Services.AddScoped<PostService>();
builder.Services.AddScoped<UserService>();

builder.Services.AddImageAssetServices();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseAuthentication();
app.UseAuthenticationContextSetter();
app.UseAuthorization();

app.MapControllers();

app.UseCors(
    cors => cors.WithOrigins("http://localhost:3035")
        .AllowCredentials()
        .AllowAnyMethod()
        .AllowAnyHeader()
);

app.Run();
