using Microsoft.EntityFrameworkCore;
using eShopSolution.Data.EF;
using eShopSolution.Utilities.Contants;
using eShopSolution.Application.Catalog.Products;
using eShopSolution.Application.Common;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Identity;
using eShopSolution.Data.Entities;
using eShopSolution.Application.System.User;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connString = builder.Configuration.GetConnectionString(SystemContants.MainConnectionString);
builder.Services.AddDbContext<EShopDbContext>(options => options.UseSqlServer(connString));

builder.Services.AddIdentity<AppUser, AppRole>()
    .AddEntityFrameworkStores<EShopDbContext>()
    .AddDefaultTokenProviders();

//Declare DI
builder.Services.AddTransient<IManageProductService, ManageProductService>();
builder.Services.AddTransient<IStorageService, FileStorageService>();
builder.Services.AddTransient<UserManager<AppUser>, UserManager<AppUser>>();
builder.Services.AddTransient<SignInManager<AppUser>, SignInManager<AppUser>>();
builder.Services.AddTransient<RoleManager<AppRole>, RoleManager<AppRole>>();
builder.Services.AddTransient<IUserService, UserService>();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "eShopSolution API",
    });
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

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseAuthorization();

app.MapControllers();

app.Run();