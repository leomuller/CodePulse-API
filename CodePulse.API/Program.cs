using CodePulse.API.Data;
using Microsoft.Extensions.FileProviders;
using Microsoft.EntityFrameworkCore;
using CodePulse.API.Repositories.Interface;
using CodePulse.API.Repositories.Implementation;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddHttpContextAccessor();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.AddDbContext<ApplicationDbContext>(options => {
	options.UseSqlServer(builder.Configuration.GetConnectionString("CodePulseConnectionString"));
});

builder.Services.AddScoped<ICategoryRespository, CategoryRepository>();
builder.Services.AddScoped<IBlogPostRepository, BlogPostRepository>();
builder.Services.AddScoped<IImageRespository, ImageRepository>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors(options => 
{
	options.AllowAnyOrigin();
	options.AllowAnyMethod();
	options.AllowAnyHeader();
});

app.UseAuthorization();


// Serve files from the ImgLib folder at the URL path /ImgLib
var imgLibPath = Path.Combine(builder.Environment.ContentRootPath, "ImgLib");
Directory.CreateDirectory(imgLibPath);
app.UseStaticFiles(new StaticFileOptions
{
	FileProvider = new PhysicalFileProvider(imgLibPath),
	RequestPath = "/ImgLib"
});



app.MapControllers();

app.Run();
