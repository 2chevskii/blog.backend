using Amazon.Runtime;
using Amazon.S3;
using Dvchevskii.Blog.Admin.Services;
using Dvchevskii.Blog.Infrastructure;
using Dvchevskii.Blog.Shared.Authentication;
using Dvchevskii.Blog.Shared.Authentication.Context;
using Dvchevskii.Blog.Shared.Authentication.Passwords;
using Dvchevskii.Blog.Shared.Posts;
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

builder.Services.AddSetupHandlers()
    .AddSetupRunner();

builder.Services.AddPasswordHasher();

builder.Services.AddBlogDbContext(
    builder.Configuration.GetConnectionString("MySql") ??
    throw new Exception("MySql connection string not found")
);

builder.Services.AddSharedDataProtection();

builder.Services.AddScoped<PostService>();
builder.Services.AddScoped<Sluggifier>();
builder.Services.AddScoped<ImageService>();

builder.Services.AddScoped<IAmazonS3, AmazonS3Client>(_ => new AmazonS3Client(
    new BasicAWSCredentials(
        builder.Configuration.GetValue<string>("S3:AccessKey"),
        builder.Configuration.GetValue<string>("S3:SecretKey")
    ),
    new AmazonS3Config
    {
        ServiceURL = "http://localhost:9000",
        ForcePathStyle = true,
    }
));

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseAuthentication();
app.UseAuthenticationContextSetter();
app.UseAuthorization();

app.MapControllers();

app.Run();
