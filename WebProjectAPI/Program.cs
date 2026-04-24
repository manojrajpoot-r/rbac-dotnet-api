using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using WebProjectAPI.Data;
using WebProjectAPI.Middleware;
using WebProjectAPI.Repositories.Implementations;
using WebProjectAPI.Repositories.Interfaces;
using WebProjectAPI.Services;
using WebProjectAPI.Services.Implementations;
using WebProjectAPI.Services.Interfaces;


var builder = WebApplication.CreateBuilder(args);



// 🔹 Database
builder.Services.AddDbContext<AppDbContext>(options =>
	options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// 🔹 Controllers (API)
builder.Services.AddControllers();

// 🔹 Dependency Injection
builder.Services.AddScoped<IRoleRepository, RoleRepository>();
builder.Services.AddScoped<IRoleService, RoleService>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IPermissionRepository, PermissionRepository>();
builder.Services.AddScoped<IPermissionService, PermissionService>();

//Add Services
builder.Services.AddScoped<IJwtService, JwtService>();

// 🔹 AutoMapper
builder.Services.AddAutoMapper(typeof(Program));
//foronted se backend api include krne ek liye permissom allow/deney
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});
//swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSwaggerGen(options =>
{
	options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
	{
		Name = "Authorization",
		Type = SecuritySchemeType.Http,
		Scheme = "bearer",
		BearerFormat = "JWT",
		In = ParameterLocation.Header,
		Description = "Enter 'Bearer YOUR_TOKEN'"
	});

	options.AddSecurityRequirement(new OpenApiSecurityRequirement
	{
		{
			new OpenApiSecurityScheme
			{
				Reference = new OpenApiReference
				{
					Type = ReferenceType.SecurityScheme,
					Id = "Bearer"
				}
			},
			new string[] {}
		}
	});
});


// 🔐 JWT Authentication (IMPORTANT 🔥)
var key = Encoding.ASCII.GetBytes(builder.Configuration["Jwt:Key"]);

builder.Services.AddAuthentication(options =>
{
	options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
	options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
	options.RequireHttpsMetadata = false;
	options.SaveToken = true;
	options.TokenValidationParameters = new TokenValidationParameters
	{
		ValidateIssuer = false,
		ValidateAudience = false,
		ValidateIssuerSigningKey = true,
		ValidateLifetime = true,
		IssuerSigningKey = new SymmetricSecurityKey(key)
	};
});

var app = builder.Build();

//fronted add
app.UseCors("AllowAll");
//Swagger
if (app.Environment.IsDevelopment())
{
	app.UseDeveloperExceptionPage(); // 🔥 error details dikhayega
	app.UseSwagger();
	app.UseSwaggerUI();
}
// 🔹 Middleware
app.UseHttpsRedirection();

app.UseRouting();

// 🔐 Authentication FIRST
app.UseAuthentication();

// 🔐 Then Authorization
app.UseAuthorization();

//Register Middleware
app.UseMiddleware<PermissionMiddleware>();

app.MapControllers();

app.Run();