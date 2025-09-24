using AZR_RED_THREAD_BLL.Profiles;
using AZR_RED_THREAD_BLL.Services.ProjectServices;
using AZR_RED_THREAD_BLL.Services.TaskServices;
using AZR_RED_THREAD_BLL.Services.UserServices;
using AZR_RED_THREAD_DAL.Models.Data;
using AZR_RED_THREAD_DAL.Services.ProjectDAServices;
using AZR_RED_THREAD_DAL.Services.TaskDAServices;
using AZR_RED_THREAD_DAL.Services.UserDAServices;
using AZR_RED_THREAD_DAL.Services.RoleDAServices;
using AZR_RED_THREAD_DAL.Services.PrivilegeDAServices;
using AZR_RED_THREAD_DAL.Services.RolePrivilegeDAServices;
using AZR_RED_THREAD_BLL.Services.Access;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Microsoft.Identity.Web;
using Microsoft.AspNetCore.Authentication.JwtBearer;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy.WithOrigins("http://localhost:3000") // ton Next.js
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddMicrosoftIdentityWebApi(builder.Configuration.GetSection("AzureAd"));

// Configure token validation events to provision user after successful token validation
builder.Services.Configure<JwtBearerOptions>(JwtBearerDefaults.AuthenticationScheme, options =>
{
    // Save existing events to chain if needed
    var existingEvents = options.Events;

    options.Events = new JwtBearerEvents
    {
        OnTokenValidated = async context =>
        {
            try
            {
                // Resolve provisioning service (scoped)
                var provisioning = context.HttpContext.RequestServices.GetRequiredService<IUserProvisioningService>();
                // Ensure user exists (create if necessary)
                await provisioning.EnsureUserExistsFromClaimsAsync(context.Principal);
            }
            catch (Exception ex)
            {
                // Log but do not fail the whole auth pipeline for transient errors
                var logger = context.HttpContext.RequestServices.GetRequiredService<ILogger<Program>>();
                logger.LogError(ex, "Error provisioning user on token validated.");
            }

            // call original events if any
            if (existingEvents?.OnTokenValidated != null)
            {
                await existingEvents.OnTokenValidated(context);
            }
        },

        OnAuthenticationFailed = async context =>
        {
            var logger = context.HttpContext.RequestServices.GetRequiredService<ILogger<Program>>();
            logger.LogError(context.Exception, "JWT authentication failed.");
            await Task.CompletedTask;
        }
    };
});

builder.Services.AddDbContext<DataContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<IDataContext>(provider => provider.GetRequiredService<DataContext>());


builder.Services.AddScoped<IProjectDAServices, ProjectDAServices>();
builder.Services.AddScoped<IProjectServices, ProjectServices>();
builder.Services.AddScoped<ITaskDAServices, TaskDAServices>();
builder.Services.AddScoped<ITaskServices, TaskServices>();
builder.Services.AddScoped<IUserDAServices, UserDAServices>();
builder.Services.AddScoped<IUserServices, UserServices>();
builder.Services.AddScoped<IRoleDAServices, RoleDAServices>();
builder.Services.AddScoped<IRoleServices, RoleServices>();
builder.Services.AddScoped<IPrivilegeDAServices, PrivilegeDAServices>();
builder.Services.AddScoped<IRolePrivilegeDAServices, RolePrivilegeDAServices>();
builder.Services.AddScoped<IPrivilegeServices, PrivilegeServices>();

// AutoMapper
builder.Services.AddAutoMapper(typeof(ProjectProfile), typeof(TaskProfile), typeof(AccessProfile), typeof(UserProfile));

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

app.UseCors("AllowFrontend");

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
