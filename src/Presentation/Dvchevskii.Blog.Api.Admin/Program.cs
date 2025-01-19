using Dvchevskii.Blog.Application;
using Dvchevskii.Blog.Application.Extensions.Mvc;
using Dvchevskii.Blog.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.ConfigureLowercaseRoutes()
    .ConfigureJsonHandling();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

ApplicationConfigurator.ConfigureServices(builder.Services);

builder.Services.AddBlogDbContext(builder.Configuration.GetConnectionString("MySql") ?? throw new Exception())
    .AddRepositories()
    .ConfigurePersistedDataProtection();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

ApplicationConfigurator.Configure(app);

app.MapControllers();

app.UseCors(cors => cors.WithOrigins("http://localhost:3003")
    .AllowCredentials()
    .AllowAnyHeader()
    .AllowAnyMethod()
);

app.Run();
