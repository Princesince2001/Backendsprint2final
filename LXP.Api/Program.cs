
using LXP.Api.Interceptors;

using LXP.Data.Repository;
using LXP.Data.IRepository;

using LXP.Core.Services;
using LXP.Core.IServices;

using Serilog;
using LXP.Data.DBContexts;
using System.Reflection;
using LXP.Common.ViewModels;
using FluentValidation.AspNetCore;
using Microsoft.Extensions.FileProviders;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .CreateLogger();

builder.Host.UseSerilog(); // Set up Serilog as the logging provider

//builder.Services.AddMvc(options =>
//{
//    options.Filters.Add<ApiExceptionInterceptor>();
//});
builder.Services.AddScoped<ICategoryServices, CategoryServices>();
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
//builder.Services.AddSingleton<LXPDbContext>();
builder.Services.AddScoped<ILearnerRepository, LearnerRepository>();
builder.Services.AddScoped<IProfileRepository, ProfileRepository>();
builder.Services.AddScoped<ILearnerService, LearnerService>();
builder.Services.AddScoped<IProfileService, ProfileService>();
builder.Services.AddScoped<IPasswordHistoryService, PasswordHistoryService>();
builder.Services.AddScoped<IPasswordHistoryRepository, PasswordHistoryRepository>();

builder.Services.AddScoped<IProfilePasswordHistoryRepository, ProfilePasswordHistoryRepository>();
builder.Services.AddScoped<ICourseLevelServices, CourseLevelServices>();
builder.Services.AddScoped<ICourseLevelRepository, CourseLevelRepository>();
builder.Services.AddScoped<ICourseTopicRepository, CourseTopicRepository>();
builder.Services.AddScoped<ICourseTopicServices, CourseTopicServices>();
builder.Services.AddScoped<ICourseRepository, CourseRepository>();
builder.Services.AddScoped<IEnrollmentService, EnrollmentService>();
builder.Services.AddScoped<IEnrollmentRepository, EnrollmentRepository>();
builder.Services.AddScoped<ICourseServices, CourseServices>();
builder.Services.Configure<EmailSettings>(builder.Configuration.GetSection("EmailSettings"));
builder.Services.AddScoped<IEmailService, EmailService>();
builder.Services.AddSingleton<LXPDbContext>();
builder.Services.AddHttpContextAccessor();//nrw HTTP

builder.Services.AddControllers()
    .AddFluentValidation(v =>
    {
        v.RegisterValidatorsFromAssembly(Assembly.GetExecutingAssembly());
    });


builder.Services.AddControllers();
builder.Services.AddHttpContextAccessor();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, $"{Assembly.GetExecutingAssembly().GetName().Name}.xml"));
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(Path.Combine(app.Environment.WebRootPath, "CourseThumbnailImages")),
    RequestPath = "/wwwroot/CourseThumbnailImages"
}); 

app.UseSerilogRequestLogging();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
