using ERP_API.Data;

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

using server.Interfaces.Repository;
using server.Interfaces.Services;
using server.Repository;
using server.Repositroy;
using server.Services;

using System.Text;


var builder = WebApplication.CreateBuilder(args);


// ===============================
// Controllers
// ===============================

builder.Services.AddControllers();


// ===============================
// Swagger + JWT Authorization
// ===============================

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1",
        new OpenApiInfo
        {
            Title = "ERP API",
            Version = "v1"
        });


    options.AddSecurityDefinition("Bearer",
        new OpenApiSecurityScheme
        {
            Name = "Authorization",
            Type = SecuritySchemeType.Http,
            Scheme = "Bearer",
            BearerFormat = "JWT",
            In = ParameterLocation.Header,
            Description =
            "Enter JWT token like: Bearer {token}"
        });


    options.AddSecurityRequirement(
        new OpenApiSecurityRequirement
        {
            {
                new OpenApiSecurityScheme
                {
                    Reference =
                    new OpenApiReference
                    {
                        Type =
                        ReferenceType.SecurityScheme,
                        Id="Bearer"
                    }
                },
                Array.Empty<string>()
            }
        });
});



// ===============================
// Database
// ===============================

builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseSqlServer(
        builder.Configuration
        .GetConnectionString("DefaultConnection"));
});



// ===============================
// Auto Mapper
// ===============================

builder.Services.AddAutoMapper(typeof(Program));




// ===============================
// Repository Dependency Injection
// ===============================

builder.Services.AddScoped<IUserRepository, UserRepository>();

builder.Services.AddScoped<IMenuRepository, MenuRepository>();

builder.Services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();




// ===============================
// Service Dependency Injection
// ===============================

builder.Services.AddScoped<IJwtService, JwtService>();

builder.Services.AddScoped<IAuthService, AuthService>();

builder.Services.AddScoped<IMenuService, MenuService>();




// ===============================
// JWT Authentication
// ===============================


var jwtSecret = builder.Configuration["Jwt:Secret"];

if (string.IsNullOrEmpty(jwtSecret))
{
    throw new Exception("JWT Secret is missing in appsettings.json");
}


var key = Encoding.UTF8.GetBytes(jwtSecret);



builder.Services
.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme =
        JwtBearerDefaults.AuthenticationScheme;


    options.DefaultChallengeScheme =
        JwtBearerDefaults.AuthenticationScheme;

})

.AddJwtBearer(options =>
{

    options.RequireHttpsMetadata = false;

    options.SaveToken = true;


    options.TokenValidationParameters =
    new TokenValidationParameters
    {

        ValidateIssuerSigningKey = true,

        IssuerSigningKey =
        new SymmetricSecurityKey(key),


        ValidateIssuer = true,

        ValidIssuer =
        builder.Configuration["Jwt:Issuer"],



        ValidateAudience = true,

        ValidAudience =
        builder.Configuration["Jwt:Audience"],



        ValidateLifetime = true,


        ClockSkew =
        TimeSpan.Zero
    };

});





// ===============================
// CORS Angular
// ===============================


builder.Services.AddCors(options =>
{

    options.AddPolicy("AllowAngularApp",
    policy =>
    {

        policy
        .WithOrigins(
            "http://localhost:4200",
            "https://localhost:4200"
        )

        .AllowAnyHeader()

        .AllowAnyMethod()

        .AllowCredentials();

    });

});





var app = builder.Build();




// ===============================
// Middleware
// ===============================


if (app.Environment.IsDevelopment())
{

    app.UseSwagger();

    app.UseSwaggerUI();

}



app.UseHttpsRedirection();



app.UseRouting();



app.UseCors("AllowAngularApp");



app.UseAuthentication();



app.UseAuthorization();



app.MapControllers();



app.Run();