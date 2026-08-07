using CodePulse.API.Data;
using Microsoft.Extensions.FileProviders;
using Microsoft.EntityFrameworkCore;
using CodePulse.API.Repositories.Interface;
using CodePulse.API.Repositories.Implementation;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;


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

builder.Services.AddDbContext<AuthDbContext>(options => {
	options.UseSqlServer(builder.Configuration.GetConnectionString("CodePulseConnectionString"));	//use different connection if you want this in dedicated DB.
});


builder.Services.AddScoped<ICategoryRespository, CategoryRepository>();
builder.Services.AddScoped<IBlogPostRepository, BlogPostRepository>();
builder.Services.AddScoped<IImageRespository, ImageRepository>();
builder.Services.AddScoped<ITokenRepository, TokenRepository>();

//config for auth.
builder.Services.AddIdentityCore<IdentityUser>()
	.AddRoles<IdentityRole>()
	.AddTokenProvider<DataProtectorTokenProvider<IdentityUser>>("CodePulse")
	.AddEntityFrameworkStores<AuthDbContext>()
	.AddDefaultTokenProviders();

builder.Services.Configure<IdentityOptions>(options =>
{
	options.Password.RequireDigit = false;
	options.Password.RequireLowercase = false;
	options.Password.RequireNonAlphanumeric = false;
	options.Password.RequireUppercase = false;
	options.Password.RequiredLength = 4;
	options.Password.RequiredUniqueChars = 2;
});

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
{
	options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
	{
		AuthenticationType = "Jwt",
		ValidateIssuer = true,
		ValidateAudience = true,
		ValidateLifetime = true,
		ValidateIssuerSigningKey = true,
		ValidIssuer = builder.Configuration["Jwt:Issuer"],
		ValidAudience = builder.Configuration["Jwt:Audience"],
		IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
	};
});

//end config for auth.



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

app.UseAuthentication();
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
