using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using ShopAppAPI.Apps.AdminApp.Validators.ProductValidators;
using ShopAppAPI.Data;
using ShopAppAPI.Entities;
using ShopAppAPI.Profiles;
using System.Text;

var builder = WebApplication.CreateBuilder(args);
var config=builder.Configuration;
// Add services to the container.

builder.Services.AddControllers().AddFluentValidation(config => config.RegisterValidatorsFromAssemblyContaining<ProductCreateDtoValidator>());
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<ShopAppContext>(options =>
{
    options.UseSqlServer(config.GetConnectionString("DefaultConnection"));
});
builder.Services.AddAutoMapper(opt =>
{
    opt.AddProfile(new MapperProfile(new HttpContextAccessor()));
});
//builder.Services.AddAutoMapper(typeof(MapperProfile).Assembly);
builder.Services.AddHttpContextAccessor();
builder.Services.AddIdentity<AppUser, IdentityRole>(options =>
{
    options.Password.RequireNonAlphanumeric = true;
}).AddEntityFrameworkStores<ShopAppContext>().AddDefaultTokenProviders();
builder.Services.AddAuthentication(x =>
{
    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(x =>
{
    x.TokenValidationParameters = new TokenValidationParameters
    {        
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config.GetSection("Jwt:SecretKey").Value)),
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = config["Jwt:Issuer"],
        ValidAudience = config["Jwt:Audience"],
    };
});

var app = builder.Build();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseStaticFiles();
app.UseAuthorization();

app.MapControllers();

app.Run();
