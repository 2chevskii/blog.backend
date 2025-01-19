using Dvchevskii.Blog.Application;
using Dvchevskii.Blog.Application.Extensions.Mvc;
using Dvchevskii.Blog.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.ConfigureLowercaseRoutes()
    .ConfigureJsonHandling();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddBlogDbContext(
        builder.Configuration.GetConnectionString("MySql") ??
        throw new Exception("mysql connection string is missing")
    ).AddRepositories()
    .ConfigurePersistedDataProtection();

ApplicationConfigurator.ConfigureServices(builder.Services);

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

ApplicationConfigurator.Configure(app);

app.MapControllers();

app.UseCors(
    cors => cors.WithOrigins("http://localhost:3004")
        .AllowCredentials()
        .AllowAnyMethod()
        .AllowAnyHeader()
);

app.Run();
