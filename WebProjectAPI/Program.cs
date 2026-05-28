using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using WebProjectAPI.Data;
using WebProjectAPI.Features.booking.Interfaces;
using WebProjectAPI.Features.booking.Repositories;
using WebProjectAPI.Features.booking.Services;
using WebProjectAPI.Features.brands.Interfaces;
using WebProjectAPI.Features.brands.Mappings;
using WebProjectAPI.Features.brands.Repositories;
using WebProjectAPI.Features.brands.Services;
using WebProjectAPI.Features.carts.Interfaces;
using WebProjectAPI.Features.carts.Repositories;
using WebProjectAPI.Features.carts.Services;
using WebProjectAPI.Features.Categories.Interfaces;
using WebProjectAPI.Features.Categories.Mappings;
using WebProjectAPI.Features.Categories.Repositories;
using WebProjectAPI.Features.Categories.Services;
using WebProjectAPI.Features.colors.Interfaces;
using WebProjectAPI.Features.colors.Repositories;
using WebProjectAPI.Features.colors.Services;
using WebProjectAPI.Features.Common.Interfaces;
using WebProjectAPI.Features.Common.Services;
using WebProjectAPI.Features.orders.services;
using WebProjectAPI.Features.orders.Services;
using WebProjectAPI.Features.product_images.Interfaces;
using WebProjectAPI.Features.product_images.Repositories;
using WebProjectAPI.Features.product_images.Services;
using WebProjectAPI.Features.product_variant.Interfaces;
using WebProjectAPI.Features.product_variant.Models;
using WebProjectAPI.Features.product_variant.Repositories;
using WebProjectAPI.Features.product_variant.Services;
using WebProjectAPI.Features.products.Interfaces;
using WebProjectAPI.Features.products.Mappings;
using WebProjectAPI.Features.products.Repositories;
using WebProjectAPI.Features.products.Services;
using WebProjectAPI.Features.sizes.Interfaces;
using WebProjectAPI.Features.sizes.Repositories;
using WebProjectAPI.Features.sizes.Services;
using WebProjectAPI.Features.sub_categories.Interfaces;
using WebProjectAPI.Features.sub_categories.Mappings;
using WebProjectAPI.Features.sub_categories.Repositories;
using WebProjectAPI.Features.sub_categories.Services;
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
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<ISubCategoryRepository, SubCategoryRepository>();
builder.Services.AddScoped<ISubCategoryService, SubCategoryService>();
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<IBrandRepository, BrandRepository>();
builder.Services.AddScoped<IBrandService, BrandService>();
builder.Services.AddScoped<IProductImageRepository, ProductImageRepository>();
builder.Services.AddScoped<IProductImageService, ProductImageService>();
builder.Services.AddScoped<ICartRepository,CartRepository>();
builder.Services.AddScoped<ICartService,CartService>();
builder.Services.AddScoped<IImageService, ImageService>();
builder.Services.AddScoped<OrderService>();
builder.Services.AddScoped<RazorpayService>();
builder.Services.AddScoped<IColorRepository, ColorRepository>();
builder.Services.AddScoped<IColorService, ColorService>();
builder.Services.AddScoped<ISizeRepository, SizeRepository>();
builder.Services.AddScoped<ISizeService, SizeService>();
builder.Services.AddScoped<IProductVariantRepository, ProductVariantRepository>();
builder.Services.AddScoped<IProductVariantService, ProductVariantService>();
builder.Services.AddScoped<IBookingRepository, BookingRepository>();
builder.Services.AddScoped<IBookingService, BookingService>();
builder.Services.AddScoped<IServiceRepository, ServiceRepository>();
builder.Services.AddScoped<IServiceService, ServiceService>();
//Add Services
builder.Services.AddScoped<IJwtService, JwtService>();










// 🔹 AutoMapper
builder.Services.AddAutoMapper(typeof(Program));
builder.Services.AddAutoMapper(typeof(CategoryProfile));
builder.Services.AddAutoMapper(typeof(SubCategoryProfile));
builder.Services.AddAutoMapper(typeof(ProductProfile));
builder.Services.AddAutoMapper(typeof(BrandProfile));
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
//Image show
app.UseStaticFiles();
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



//Register Middleware
app.UseMiddleware<PermissionMiddleware>();


// 🔐 Then Authorization
app.UseAuthorization();
app.MapControllers();

app.Run();