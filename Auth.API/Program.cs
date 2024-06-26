using Infrastructure;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

var loggerFactory = LoggerFactory.Create(builder =>
{
    builder.AddConsole(); // Add the Console logging provider
});

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

Application.DependencyResolvement.DependencyResolverService.RegisterApplicationLayer(builder.Services, builder.Configuration);
Infrastructure.DependencyResolvement.DependencyResolverService.RegisterApplicationLayer(builder.Services);

builder.Services.AddDbContext<DatabaseContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("ConnectionString")));



var app = builder.Build();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();