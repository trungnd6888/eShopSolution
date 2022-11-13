using eShopSolution.Application.Catalog.Brands;
using eShopSolution.Application.Catalog.Categories;
using eShopSolution.Application.Catalog.Customers;
using eShopSolution.Application.Catalog.Distributors;
using eShopSolution.Application.Catalog.Newses;
using eShopSolution.Application.Catalog.OrderDetails;
using eShopSolution.Application.Catalog.Orders;
using eShopSolution.Application.Catalog.ProductCategories;
using eShopSolution.Application.Catalog.ProductDistributors;
using eShopSolution.Application.Catalog.ProductImages;
using eShopSolution.Application.Catalog.Products;
using eShopSolution.Application.Catalog.Statuses;
using eShopSolution.Application.Common.FileStorage;
using eShopSolution.Application.Common.Mail;
using eShopSolution.Application.System.Actions;
using eShopSolution.Application.System.Auth;
using eShopSolution.Application.System.Banners;
using eShopSolution.Application.System.Forms;
using eShopSolution.Application.System.Histories;
using eShopSolution.Application.System.RoleClaims;
using eShopSolution.Application.System.Roles;
using eShopSolution.Application.System.UserRoles;
using eShopSolution.Application.System.Users;
using eShopSolution.Data.EF;
using eShopSolution.Data.Entities;
using eShopSolution.Utilities.Contants;
using eShopSolution.ViewModel.System.Auth;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connString = builder.Configuration.GetConnectionString(SystemContants.MainConnectionString);
builder.Services.AddDbContext<EShopDbContext>(options => options.UseSqlServer(connString));

builder.Services.AddDefaultIdentity<AppUser>()
                .AddRoles<AppRole>()
                .AddEntityFrameworkStores<EShopDbContext>()
                .AddDefaultTokenProviders();

//Declare DI
builder.Services.AddTransient<IMailService, MailService>();
builder.Services.AddTransient<IOrderDetailService, OrderDetailService>();
builder.Services.AddTransient<IStatusService, StatusService>();
builder.Services.AddTransient<IOrderDetailService, OrderDetailService>();
builder.Services.AddTransient<IOrderService, OrderService>();
builder.Services.AddTransient<INewsService, NewsService>();
builder.Services.AddTransient<IBannerService, BannerService>();
builder.Services.AddTransient<ICustomerService, CustomerService>();
builder.Services.AddTransient<IFormService, FormService>();
builder.Services.AddTransient<IActionService, ActionService>();
builder.Services.AddTransient<IHistoryService, HistoryService>();
builder.Services.AddTransient<IRoleClaimService, RoleClaimService>();
builder.Services.AddTransient<IUserRoleService, UserRoleService>();
builder.Services.AddTransient<IProductService, ProductService>();
builder.Services.AddTransient<IRoleService, RoleService>();
builder.Services.AddTransient<IUserService, UserService>();
builder.Services.AddTransient<IBrandService, BrandService>();
builder.Services.AddTransient<IProductCategoryService, ProductCategoryService>();
builder.Services.AddTransient<IProductDistributorService, ProductDistributorService>();
builder.Services.AddTransient<IProductImageService, ProductImageService>();
builder.Services.AddTransient<IDistributorService, DistributorService>();
builder.Services.AddTransient<ICategoryService, CategoryService>();
builder.Services.AddTransient<IStorageService, FileStorageService>();
builder.Services.AddTransient<UserManager<AppUser>, UserManager<AppUser>>();
builder.Services.AddTransient<SignInManager<AppUser>, SignInManager<AppUser>>();
builder.Services.AddTransient<RoleManager<AppRole>, RoleManager<AppRole>>();
builder.Services.AddTransient<IAuthService, AuthService>();
builder.Services.AddTransient<IValidator<LoginRequest>, LoginRequestValidator>();

builder.Services.AddControllers()
                .AddFluentValidation(options => options.RegisterValidatorsFromAssemblyContaining<LoginRequestValidator>())
                .AddJsonOptions(options =>
                 {
                     options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
                     options.JsonSerializerOptions.WriteIndented = true;
                 });
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "eShopSolution API",
    });

    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = @"JWT Authorization header using the Bearer scheme. \r\n\r\n
                      Enter 'Bearer' [space] and then your token in the text input below.
                      \r\n\r\nExample: 'Bearer 12345abcdef'",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement()
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                },
                Scheme = "oauth2",
                Name = "Bearer",
                In = ParameterLocation.Header,
            },
            new List<string>()
        }
    });
});

string issuer = builder.Configuration.GetValue<string>("Tokens:Issuer");
string signingKey = builder.Configuration.GetValue<string>("Tokens:Key");
byte[] signingKeyBytes = System.Text.Encoding.UTF8.GetBytes(signingKey);

builder.Services.AddAuthentication(opt =>
                {
                    opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(options =>
                {
                    options.RequireHttpsMetadata = false;
                    options.SaveToken = true;
                    options.TokenValidationParameters = new TokenValidationParameters()
                    {
                        ValidateIssuer = true,
                        ValidIssuer = issuer,
                        ValidateAudience = true,
                        ValidAudience = issuer,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ClockSkew = System.TimeSpan.Zero,
                        IssuerSigningKey = new SymmetricSecurityKey(signingKeyBytes)
                    };
                });

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("ProductView", policy => policy.RequireClaim("product", "product.view"));
    options.AddPolicy("ProductCreate", policy => policy.RequireClaim("product", "product.create"));
    options.AddPolicy("ProductUpdate", policy => policy.RequireClaim("product", "product.update"));
    options.AddPolicy("ProductRemove", policy => policy.RequireClaim("product", "product.remove"));

    options.AddPolicy("CategoryView", policy => policy.RequireClaim("category", "category.view"));
    options.AddPolicy("CategoryCreate", policy => policy.RequireClaim("category", "category.create"));
    options.AddPolicy("CategoryUpdate", policy => policy.RequireClaim("category", "category.update"));
    options.AddPolicy("CategoryRemove", policy => policy.RequireClaim("category", "category.remove"));

    options.AddPolicy("DistributorView", policy => policy.RequireClaim("distributor", "distributor.view"));
    options.AddPolicy("DistributorCreate", policy => policy.RequireClaim("distributor", "distributor.create"));
    options.AddPolicy("DistributorUpdate", policy => policy.RequireClaim("distributor", "distributor.update"));
    options.AddPolicy("DistributorRemove", policy => policy.RequireClaim("distributor", "distributor.remove"));

    options.AddPolicy("NewsView", policy => policy.RequireClaim("news", "news.view"));
    options.AddPolicy("NewsCreate", policy => policy.RequireClaim("news", "news.create"));
    options.AddPolicy("NewsUpdate", policy => policy.RequireClaim("news", "news.update"));
    options.AddPolicy("NewsRemove", policy => policy.RequireClaim("news", "news.remove"));

    options.AddPolicy("UserView", policy => policy.RequireClaim("user", "user.view"));
    options.AddPolicy("UserCreate", policy => policy.RequireClaim("user", "user.create"));
    options.AddPolicy("UserUpdate", policy => policy.RequireClaim("user", "user.update"));
    options.AddPolicy("UserRemove", policy => policy.RequireClaim("user", "user.remove"));

    options.AddPolicy("CustomerView", policy => policy.RequireClaim("customer", "customer.view"));
    options.AddPolicy("CustomerCreate", policy => policy.RequireClaim("customer", "customer.create"));
    options.AddPolicy("CustomerUpdate", policy => policy.RequireClaim("customer", "customer.update"));
    options.AddPolicy("CustomerRemove", policy => policy.RequireClaim("customer", "customer.remove"));

    options.AddPolicy("OrderView", policy => policy.RequireClaim("order", "order.view"));
    options.AddPolicy("OrderCreate", policy => policy.RequireClaim("order", "order.create"));
    options.AddPolicy("OrderUpdate", policy => policy.RequireClaim("order", "order.update"));
    options.AddPolicy("OrderRemove", policy => policy.RequireClaim("order", "order.remove"));

    options.AddPolicy("PermissionView", policy => policy.RequireClaim("permission", "permission.view"));
    options.AddPolicy("PermissionCreate", policy => policy.RequireClaim("permission", "permission.create"));
    options.AddPolicy("PermissionUpdate", policy => policy.RequireClaim("permission", "permission.update"));
    options.AddPolicy("PermissionRemove", policy => policy.RequireClaim("permission", "permission.remove"));

    options.AddPolicy("BannerView", policy => policy.RequireClaim("banner", "banner.view"));
    options.AddPolicy("BannerCreate", policy => policy.RequireClaim("banner", "banner.create"));
    options.AddPolicy("BannerUpdate", policy => policy.RequireClaim("banner", "banner.update"));
    options.AddPolicy("BannerRemove", policy => policy.RequireClaim("banner", "banner.remove"));

    options.AddPolicy("NewsView", policy => policy.RequireClaim("news", "news.view"));
    options.AddPolicy("NewsCreate", policy => policy.RequireClaim("news", "news.create"));
    options.AddPolicy("NewsUpdate", policy => policy.RequireClaim("news", "news.update"));
    options.AddPolicy("NewsRemove", policy => policy.RequireClaim("news", "news.remove"));

    options.AddPolicy("OrderView", policy => policy.RequireClaim("order", "order.view"));
    options.AddPolicy("OrderCreate", policy => policy.RequireClaim("order", "order.create"));
    options.AddPolicy("OrderUpdate", policy => policy.RequireClaim("order", "order.update"));
    options.AddPolicy("OrderRemove", policy => policy.RequireClaim("order", "order.remove"));
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "eShopSolution v1");
    });
}

//Show UseCors with CorsPolicyBuilder
app.UseCors(builder =>
{
    builder.AllowAnyOrigin()
           .AllowAnyMethod()
           .AllowAnyHeader();
});

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();