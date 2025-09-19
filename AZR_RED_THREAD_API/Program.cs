using AZR_RED_THREAD_BLL.Profiles;
using AZR_RED_THREAD_BLL.Services.ProjectServices;
using AZR_RED_THREAD_DAL.Models.Data;
using AZR_RED_THREAD_DAL.Services;
using AZR_RED_THREAD_DAL.Services.ProjectDAServices;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<DataContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<IDataContext>(provider => provider.GetRequiredService<DataContext>());

builder.Services.AddAutoMapper(typeof(ProjectProfile));

builder.Services.AddScoped<IProjectDAServices, ProjectDAServices>();
builder.Services.AddScoped<IProjectServices, ProjectServices>();

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "RED THREAD API",
        Version = "v1",
        Description = "API pour la gestion des projets et tâches"
    });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "RED_THREAD_API v1");
    });
}

app.UseHttpsRedirection();

app.MapControllers();

app.Run();
