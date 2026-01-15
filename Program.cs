using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using Microsoft.IdentityModel.Tokens;
using server.Controllers;
using server.Data;
using server.Helpers;
using server.Interfaces.Repository;
using server.Interfaces.Services;
using server.Reposistory;
using server.Repository;
using server.Services;
using System.Security.Claims;
using System.Text;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;


builder.Services.AddDbContext<DataContext>(options =>
    options.UseSqlServer(configuration.GetConnectionString("DefaultConnectionLive"))
);


builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new() { Title = "API", Version = "v1" });


    c.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = Microsoft.OpenApi.Models.SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = Microsoft.OpenApi.Models.ParameterLocation.Header,
        Description = "Enter: Bearer {your JWT token}"
    });

    c.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
    {
        {
            new Microsoft.OpenApi.Models.OpenApiSecurityScheme
            {
                Reference = new Microsoft.OpenApi.Models.OpenApiReference
                {
                    Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});


var jwtKey = configuration["Jwt:Key"];
var jwtIssuer = configuration["Jwt:Issuer"];
var jwtAudience = configuration["Jwt:Audience"];

if (string.IsNullOrEmpty(jwtKey))
    throw new Exception("JWT Key is missing in appsettings.json");

var key = Encoding.UTF8.GetBytes(jwtKey);

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(key),
        ValidateIssuer = true,
        ValidIssuer = jwtIssuer,
        ValidateAudience = true,
        ValidAudience = jwtAudience,
        ValidateLifetime = true,
        RequireExpirationTime = true,
        NameClaimType = ClaimTypes.Name,
        RoleClaimType = ClaimTypes.Role,
        ClockSkew = TimeSpan.Zero // strict expiration
    };
});


builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigins", policy =>
    {
        policy.WithOrigins(
                "http://localhost:4200",                       // Angular dev
                "https://ecommerce-haseeb-sigma.vercel.app"   // Production
            )
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials();
    });
});

builder.Services.AddScoped<IJwtHelper, JwtHelper>();

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddScoped(typeof(IGenericReposistroy<>), typeof(GenericRepository<>));
builder.Services.AddScoped<IProductRepository, ProductRepositry>();
builder.Services.AddScoped<IImageReposistory, ImageReposistory>();
builder.Services.AddScoped<IImageServies, ImageService>();
builder.Services.AddScoped<IBrandReposistory, BrandRepository>();
builder.Services.AddScoped<ICatalogService, CatalogService>();
builder.Services.AddScoped<ICategoryReposistory, CategoryReposistory>();
builder.Services.AddScoped<IWishListItemReposistory, WishListItemRepository>();
builder.Services.AddScoped<IWishListReposistory, WishListReposistory>();
builder.Services.AddScoped<IWishListService, WishListService>();
builder.Services.AddScoped<ICartReposistory, CartReposistory>();
builder.Services.AddScoped<ICartService, CartService>();
builder.Services.AddScoped<ICartItemReposistory, CartItemReposistory>();
builder.Services.AddScoped<IUserReposistory, UserReposistory>();




var app = builder.Build();


app.UseSwagger();
app.UseSwaggerUI();

var uploadsPath = Path.Combine(builder.Environment.ContentRootPath, "Uploads");
if (!Directory.Exists(uploadsPath))
    Directory.CreateDirectory(uploadsPath);

app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(uploadsPath),
    RequestPath = "/Image"
});

app.UseRouting();
app.UseCors("AllowSpecificOrigins");
app.UseAuthentication();            
app.UseAuthorization();
app.MapControllers();
app.Run();
