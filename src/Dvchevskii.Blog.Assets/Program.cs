using Amazon.Runtime;
using Amazon.S3;
using Dvchevskii.Blog.Assets.Services;
using Dvchevskii.Blog.Infrastructure;
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

builder.Services.AddAuthenticationContext()
    .AddAuthenticationContextSetterMiddleware()
    .AddBlogAuthenticationScheme()
    .AddSharedDataProtection();

builder.Services.AddSetupHandlers()
    .AddSetupRunner()
    .AddPasswordHasher();

builder.Services.AddBlogDbContext(
    builder.Configuration.GetConnectionString("MySql")
    ?? throw new Exception("No mysql connection string")
);


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

builder.Services.AddScoped<ImageService>();


var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseAuthentication();
app.UseAuthenticationContextSetter();
app.UseAuthorization();

app.MapControllers();

app.UseCors(cors => cors.WithOrigins("http://localhost:3031")
    .AllowCredentials()
    .AllowAnyHeader()
    .AllowAnyMethod()
    .WithExposedHeaders("Location")
);

app.Run();
