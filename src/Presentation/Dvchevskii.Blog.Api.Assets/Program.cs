using Dvchevskii.Blog.Application;
using Dvchevskii.Blog.Application.Extensions.Mvc;
using Dvchevskii.Blog.Assets;
using Dvchevskii.Blog.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.ConfigureLowercaseRoutes()
    .ConfigureJsonHandling();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

ApplicationConfigurator.ConfigureServices(builder.Services);

builder.Services.AddBlogDbContext(
        builder.Configuration.GetConnectionString("MySql")
        ?? throw new Exception("No mysql connection string")
    ).AddRepositories()
    .ConfigurePersistedDataProtection();


builder.Services.AddS3(builder.Configuration.GetSection("S3"))
    .AddImageService();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

ApplicationConfigurator.Configure(app);

app.MapControllers();

app.UseCors(cors => cors.WithOrigins("http://localhost:3031")
    .AllowCredentials()
    .AllowAnyHeader()
    .AllowAnyMethod()
    .WithExposedHeaders("Location")
);

app.Run();
